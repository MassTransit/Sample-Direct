using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;

namespace Contracts
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureMessageTopology(this IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Message<ContentReceived>(c => c.SetEntityName("content.received"));
//            configurator.Send<ContentReceived>(c =>
//            {
//                c.UseCorrelationId(m => m.Id);
//                c.UseRoutingKeyFormatter(f => f.Message.AgentId);
//            });


            configurator.Publish<ContentReceived>(c => { c.ExchangeType = ExchangeType.Direct; });
        }
    }
}