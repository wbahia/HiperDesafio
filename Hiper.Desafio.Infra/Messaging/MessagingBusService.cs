using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Hiper.Desafio.Infra.Messaging;

public class MessageBusService
{
    private readonly IConfiguration _configuration;
    private readonly string _queueName = "pedidos-queue";

    public MessageBusService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void PublicarPedido(object message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            RequestedConnectionTimeout = TimeSpan.FromMilliseconds(2000)
        };

        try
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: true, //persiste a fila se o rabbit reiniciar
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(">>> SUCESSO: Pedido enviado ao RabbitMQ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>> ERRO RABBITMQ: {ex.Message}");
        }
    }
}