namespace DirectServer
{
    using System.Threading.Tasks;
    using Contracts;
    using DirectClient;
    using MassTransit;
    using MassTransit.ConsumeConfigurators;
    using MassTransit.Definition;
    using MassTransit.RabbitMqTransport;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;


    public class ContentReceivedConsumer :
        IConsumer<ContentReceived>
    {
        readonly ILogger<ContentReceivedConsumer> _logger;

        public ContentReceivedConsumer(ILogger<ContentReceivedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ContentReceived> context)
        {
            _logger.LogInformation("Content Received: {Id}", context.Message.Id);

            return Task.CompletedTask;
        }
    }


    public class ContentReceivedConsumerDefinition :
        ConsumerDefinition<ContentReceivedConsumer>
    {
        readonly string _nodeId;

        public ContentReceivedConsumerDefinition(IOptions<NodeOptions> options)
        {
            _nodeId = options.Value.NodeId;
            EndpointName = $"direct.client.{_nodeId}";
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ContentReceivedConsumer> consumerConfigurator)
        {
            endpointConfigurator.ConfigureConsumeTopology = false;

            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.Bind<ContentReceived>(x =>
                {
                    x.RoutingKey = _nodeId;
                    x.ExchangeType = ExchangeType.Direct;
                });
            }
        }
    }
}