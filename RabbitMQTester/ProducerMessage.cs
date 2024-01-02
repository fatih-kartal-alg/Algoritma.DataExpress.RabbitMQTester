using RabbitMQTester;

namespace Algoritma.DataExpress.Core.Models;

public class ProducerMessage
{
    public object? Partition { get; set; }
    public object? ProducerQuery { get; set; }
    public object? OffsetDate { get; set; }
    public List<CustomerData>? Data { get; init; }
}
