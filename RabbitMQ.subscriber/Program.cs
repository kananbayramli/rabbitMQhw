﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
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


            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;

            //bashlangici ve sonu ne olur olsun, orta Error olan mesajlari getir
            var routeKey = "*.Error.*";
            channel.QueueBind(queueName, "logs-topic", routeKey);

            channel.BasicConsume(queueName, false, consumer);

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
