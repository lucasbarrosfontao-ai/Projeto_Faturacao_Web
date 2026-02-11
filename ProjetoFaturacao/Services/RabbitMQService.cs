using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ProjetoFaturacao.Models;
public class RabbitMQService
{
    private readonly string _hostname;
    private readonly string _user;
    private readonly string _password;

    public RabbitMQService(IConfiguration configuration)
    {
        _hostname = configuration["RABBITMQ_HOST"] ?? "localhost";
        _user = configuration["RABBITMQ_USER"] ?? "guest";
        _password = configuration["RABBITMQ_PASSWORD"] ?? "guest";
    }

    public async Task EnviarFaturaParaFila(FaturaMessage fatura)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = _hostname,
            UserName = _user,
            Password = _password
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "faturas_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        var json = JsonSerializer.Serialize(fatura);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true 
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "faturas_queue",
            mandatory: false,
            basicProperties: properties,
            body: body);
    }
    public async Task EnviarCodigoParaSenhaDeRecuperacao(RecuperacaoMessage recuperacao)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = _hostname,
            UserName = _user,
            Password = _password
        };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "recuperacao_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        var json = JsonSerializer.Serialize(recuperacao);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true 
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "recuperacao_queue",
            mandatory: false,
            basicProperties: properties,
            body: body);
    }
}