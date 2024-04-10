using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var receiverFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin",
        };
        using (var receiverConnection = receiverFactory.CreateConnection())
        using (var receiverChannel = receiverConnection.CreateModel())
        {
            receiverChannel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            var consumer = new EventingBasicConsumer(receiverChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            receiverChannel.BasicConsume(queue: "hello",
                                         autoAck: true,
                                         consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
