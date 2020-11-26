using System;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace DirectServer
{
    class Program
    {
        static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/");

                cfg.ConfigureMessageTopology();

                cfg.ReceiveEndpoint("direct.server", endpoint =>
                {
                    endpoint.Handler<ClientAvailable>(async context =>
                    {
                        await context.Publish<ContentReceived>(new
                        {
                            Id = NewId.NextGuid(),
                        }, x => x.SetRoutingKey(context.Message.NodeId));
                    });
                });
            });

            await bus.StartAsync();
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Started, enter to quit");

                    Console.ReadLine();
                });
            }
            finally
            {
                await bus.StopAsync();
            }
        }
    }
}