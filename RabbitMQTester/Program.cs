using System.Text.Json;
using Algoritma.DataExpress.Core.Models;
using MassTransit;

namespace RabbitMQTester
{
    internal static class Program
    {
        private static async Task Main()
        {
            const string jsonFile = "CustomerData.json"; // JSON dosyasının adı
            var content = await File.ReadAllTextAsync($"../../../{jsonFile}");
            var message = new ProducerMessage
            {
                Data = JsonSerializer.Deserialize<List<CustomerData>>(content)
            };

            await Send(message);
        }

        private static async Task Send(object? message)
        {
            const string rabbitMqUri = "rabbitmq://localhost:5672/CustomerData";
            const string queue = "FullData";
            const string userName = "guest";
            const string password = "guest";

            var bus = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(rabbitMqUri, configurator =>
                {
                    configurator.Username(userName);
                    configurator.Password(password);
                });
            });

            var sendToUri = new Uri($"{rabbitMqUri}/{queue}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            await bus.StartAsync();
            await endPoint.Send(message!);
            Console.WriteLine("Mesaj başarıyla gönderildi. Çıkış yapmak için bir tuşa basın.");
            Console.ReadKey();
        }
    }
}
