namespace DirectClient
{
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;


    public class StartupService :
        BackgroundService
    {
        readonly IBus _bus;
        readonly string _nodeId;

        public StartupService(IBus bus, IOptions<NodeOptions> options)
        {
            _bus = bus;
            _nodeId = options.Value.NodeId;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.Publish<ClientAvailable>(new {NodeId = _nodeId}, stoppingToken);
        }
    }
}