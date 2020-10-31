using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQDemo.Consumer
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
                Password = "guest"
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "queue-messages",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, eventArgs) =>
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    Console.WriteLine(Environment.NewLine + "[New message received] " + message);
                };

                channel.BasicConsume(queue: "queue-messages",
                        autoAck: true,
                        consumer: consumer);

                Console.WriteLine("Waiting messages to proccess");
                Console.WriteLine("Press some key to exit...");
                Console.ReadKey();
            }

        }
    }
}
