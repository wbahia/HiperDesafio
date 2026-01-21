using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Hiper.Desafio.Domain.Entities;

namespace Hiper.Desafio.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ConnectionFactory _factory;
    

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    /// <summary>
    /// async Task ExecuteAsync(CancellationToken stoppingToken)
    /// Aqui imagino parte de um processo pesado que pode envolver, gerar uma NF, envio de email, controle de estoque etc.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string queueName = "pedidos-queue";
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        // garante que a fila exista antes de tentar consumir
        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        // evento disparado quando uma mensagem chega na fila
        consumer.Received += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var pedido = JsonSerializer.Deserialize<Pedido>(message);

                _logger.LogInformation(" [x] Worker: Processando Pedido ID: {Id}", pedido?.Id);
                _logger.LogInformation("     Descrição: {Desc}", pedido?.Descricao);
                _logger.LogInformation("     Valor Calculado pela Strategy: {Valor:C}", pedido?.ValorFinal);

                // Simulação de processamento mais pesado
                Task.Delay(2000).Wait();

                _logger.LogInformation(" [v] Worker: Pedido {Id} finalizado com sucesso!", pedido?.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(" [!] Erro ao processar mensagem: {Msg}", ex.Message);
            }
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true, // remove da fila "automagicamente" após ler
                             consumer: consumer);

        _logger.LogInformation(" [*] Worker aguardando mensagens em: {Queue}", queueName);

        // mantém o serviço rodando
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}