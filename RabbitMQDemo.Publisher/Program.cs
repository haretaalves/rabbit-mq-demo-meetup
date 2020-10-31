using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQDemo.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                while (true)
                {
                    Console.WriteLine("Please, type your message: ");

                    var messageTyped = Console.ReadLine();

                    channel.QueueDeclare(
                        queue: "queue-messages",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message =
                        $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - " +
                        $"Message content: {messageTyped}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "queue-messages",
                                         basicProperties: null,
                                         body: body);
                }
            }
        }
    }
}
