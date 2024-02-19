using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://flzpmnzi:0shfjNKkz5zt_a39kqc4pdbJd0tzqUPL@moose.rmq.cloudamqp.com/flzpmnzi");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-fanout",durable:true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x => {

                string message = $"Log {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-fanout", "", null, messageBody);
                Console.WriteLine($"Mesaj catdirilmishdir : {message}");

            });




            Console.ReadLine();

        }
    }
}
