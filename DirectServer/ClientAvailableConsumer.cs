namespace DirectServer
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using MassTransit.Definition;


    public class ClientAvailableConsumer :
        IConsumer<ClientAvailable>
    {
        public async Task Consume(ConsumeContext<ClientAvailable> context)
        {
            await context.Publish<ContentReceived>(new
            {
                InVar.Id,
                context.Message.NodeId
            });
        }
    }


    public class ClientAvailableConsumerDefinition :
        ConsumerDefinition<ClientAvailableConsumer>
    {
        public ClientAvailableConsumerDefinition()
        {
            EndpointName = "direct.server";
        }
    }
}