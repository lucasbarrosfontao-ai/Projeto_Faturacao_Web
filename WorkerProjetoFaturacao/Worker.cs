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
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IServiceScopeFactory _scopeFactory; // Para aceder à BD
    private readonly PdfGeneratorService _pdfService = new();

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker iniciado. Aguardando faturas...");

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RABBITMQ_HOST"] ?? "localhost",
            UserName = _configuration["RABBITMQ_USER"] ?? "guest",
            Password = _configuration["RABBITMQ_PASSWORD"] ?? "guest"
        };

        // Ligação assíncrona (v7.2)
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue: "faturas_queue",
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

                    // VERIFICAÇÃO DE EMAIL
                    if (!string.IsNullOrWhiteSpace(faturaMsg.EmailCliente))
                    {
                        _logger.LogInformation($"[OK] Email detetado: {faturaMsg.EmailCliente}. Iniciando envio...");
                        await EnviarEmailComPdf(faturaMsg);
                    }
                    else
                    {
                        _logger.LogWarning($"[AVISO] Fatura {faturaMsg.Id_Fatura} (Cliente: {faturaMsg.NomeCliente}) não possui email. O PDF não será enviado.");
                        // Aqui podias, por exemplo, guardar o PDF apenas numa pasta local
                    }
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: "faturas_queue", autoAck: false, consumer: consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessarFatura(FaturaMessage msg)
{
    using (var scope = _scopeFactory.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var emailService = new EmailService(_configuration); // Ou injetado via DI

        // 1. Buscar dados na BD
        var fatura = await dbContext.Faturas
            .Include(f => f.Cliente)
            .Include(f => f.LinhasFatura).ThenInclude(l => l.Produto)
            .FirstOrDefaultAsync(f => f.Id_Fatura == msg.Id_Fatura);

        if (fatura == null) return;

        // 2. Gerar PDF
        byte[] pdfBytes = _pdfService.GerarFaturaPdf(fatura);

        // 3. Enviar Email se o destinatário existir
        if (!string.IsNullOrWhiteSpace(msg.EmailCliente))
        {
            _logger.LogInformation($"Enviando email para {msg.EmailCliente}...");
            await emailService.EnviarFaturaEmailAsync(
                msg.EmailCliente, 
                fatura.Cliente?.Nome ?? "Cliente", 
                pdfBytes, 
                fatura.Numero_Fatura
            );
            _logger.LogInformation("Email enviado com sucesso!");
        }
    }
}

    private async Task EnviarEmailComPdf(FaturaMessage msg)
    {
        // SIMULAÇÃO DO ENVIO (Para a tua PAP, podes usar a biblioteca MailKit depois)
        _logger.LogInformation($"[SMTP] Gerando PDF para {msg.NomeCliente}...");
        await Task.Delay(1500); // Simula tempo de processamento
        _logger.LogInformation($"[SMTP] Email enviado com sucesso para {msg.EmailCliente}!");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync();
        if (_connection != null) await _connection.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}