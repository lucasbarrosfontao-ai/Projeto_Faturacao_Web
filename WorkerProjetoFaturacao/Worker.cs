using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProjetoFaturacao.Models;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PdfGeneratorService _pdfService = new();
    private IConnection? _connection;
    private IChannel? _channel;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker aguardando inicialização do RabbitMQ...");

        // 1. Tentar conectar ao RabbitMQ (Retry Logic)
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["RABBITMQ_HOST"] ?? "localhost",
                    UserName = _configuration["RABBITMQ_USER"] ?? "guest",
                    Password = _configuration["RABBITMQ_PASSWORD"] ?? "guest"
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
                
                _logger.LogInformation("Conectado ao RabbitMQ com sucesso!");
                break; 
            }
            catch (Exception)
            {
                _logger.LogWarning("RabbitMQ ainda não está pronto. Tentando novamente em 5 segundos...");
                await Task.Delay(5000, stoppingToken);
            }
        }

        // 2. Configurar a Fila
        await _channel!.QueueDeclareAsync(queue: "faturas_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            try 
            {
                var faturaMsg = JsonSerializer.Deserialize<FaturaMessage>(messageJson);
                
                if (faturaMsg != null)
                {
                    _logger.LogInformation($"[x] Fatura Recebida: ID {faturaMsg.Id_Fatura}");

                    // CHAMADA DO MÉTODO REAL (ProcessarFatura em vez de EnviarEmailComPdf)
                    await ProcessarFatura(faturaMsg);
                }

                // Confirma que a mensagem foi processada
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                // Se der erro, a mensagem volta para a fila para tentar de novo
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: "faturas_queue", autoAck: false, consumer: consumer);
        
        // Mantém o worker rodando
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessarFatura(FaturaMessage msg)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailService = new EmailService(_configuration); 

            // 1. Buscar dados detalhados na BD (Importante para o PDF ter produtos e nomes)
            var fatura = await dbContext.Faturas
                .Include(f => f.Cliente)
                .Include(f => f.LinhasFatura).ThenInclude(l => l.Produto)
                .FirstOrDefaultAsync(f => f.Id_Fatura == msg.Id_Fatura);

            if (fatura == null)
            {
                _logger.LogError($"Fatura {msg.Id_Fatura} não encontrada na base de dados.");
                return;
            }

            // 2. Gerar o PDF real em memória
            _logger.LogInformation($"Gerando PDF para {fatura.Numero_Fatura}...");
            byte[] pdfBytes = _pdfService.GerarFaturaPdf(fatura);

            // 3. Verificação de Email e Envio Real
            if (!string.IsNullOrWhiteSpace(msg.EmailCliente))
            {
                _logger.LogInformation($"Iniciando envio de email para {msg.EmailCliente}...");
                try 
                {
                    await emailService.EnviarFaturaEmailAsync(
                        msg.EmailCliente, 
                        fatura.Cliente?.Nome ?? "Cliente", 
                        pdfBytes, 
                        fatura.Numero_Fatura
                    );
                    _logger.LogInformation("[SMTP] Email enviado com sucesso!");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[SMTP] Falha ao enviar email: {ex.Message}");
                    throw; // Lança a exceção para o Nack tratar
                }
            }
            else
            {
                _logger.LogWarning($"Fatura {fatura.Numero_Fatura} não tem email associado. O envio foi cancelado.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync();
        if (_connection != null) await _connection.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}