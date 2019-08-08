using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using RabbitMQ.Client;

namespace DirectClient
{
    class Program
    {
        static async Task Main()
        {
            var nodeId = Process.GetCurrentProcess().Id.ToString();

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("localhost", "/");

                cfg.ConfigureMessageTopology();

                cfg.ReceiveEndpoint(host, $"direct.client.{nodeId}", endpoint =>
                {
                    endpoint.BindMessageExchanges = false;

                    endpoint.Bind<ContentReceived>(x =>
                    {
                        x.RoutingKey = nodeId;
                        x.ExchangeType = ExchangeType.Direct;
                    });

                    endpoint.Handler<ContentReceived>(async context => { Console.WriteLine("Content Received: {0}", context.Message.Id); });
                });
            });

            await bus.StartAsync();
            try
            {
                await bus.Publish<ClientAvailable>(new {NodeId = nodeId});

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