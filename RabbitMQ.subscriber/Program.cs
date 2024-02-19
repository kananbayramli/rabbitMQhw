using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://flzpmnzi:0shfjNKkz5zt_a39kqc4pdbJd0tzqUPL@moose.rmq.cloudamqp.com/flzpmnzi");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //Eger eyni queue publisherde yaradilibsa subscriberda yeniden yaratmaga ehtiyac yoxdur
            //channel.QueueDeclare("hello-queue", true, false, false);


            var randomQueueName = "log-database-save-queue"; //channel.QueueDeclare().QueueName;
            channel.QueueDeclare(randomQueueName, true, false, false);
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Logs are reading...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);

                Console.WriteLine("Gelen mesaj: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
