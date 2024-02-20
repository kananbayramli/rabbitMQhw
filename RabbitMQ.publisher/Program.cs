using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{

    public enum LogNames 
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://flzpmnzi:0shfjNKkz5zt_a39kqc4pdbJd0tzqUPL@moose.rmq.cloudamqp.com/flzpmnzi");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-topic",durable:true, type: ExchangeType.Topic);


            Random rmd = new Random();
            Enumerable.Range(1, 50).ToList().ForEach(x => 
            {
                LogNames log1 = (LogNames)rmd.Next(1,5);
                LogNames log2 = (LogNames)rmd.Next(1, 5);
                LogNames log3 = (LogNames)rmd.Next(1, 5);

                string message = $"{log1}.{log2}.{log3}"; 
                var routeKey = $"{log1}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-topic", routeKey , null, messageBody);
                Console.WriteLine($"Log catdirilmishdir : {message}");
            });
           Console.ReadLine();

        }
    }
}
