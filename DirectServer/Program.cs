namespace DirectServer
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using MassTransit.Metadata;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;


    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(HostMetadataCache.IsRunningInContainer ? "rabbitmq" : "localhost", "/");

                            cfg.ConfigureMessageTopology();

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    services.AddOptions<MassTransitHostOptions>()
                        .Configure(options =>
                        {
                            options.WaitUntilStarted = true;
                            options.StartTimeout = TimeSpan.FromSeconds(30);
                            options.StopTimeout = TimeSpan.FromSeconds(60);
                        });
                    services.AddOptions<HostOptions>()
                        .Configure(options =>
                        {
                            options.StartupTimeout = TimeSpan.FromSeconds(60);
                            options.ShutdownTimeout = TimeSpan.FromSeconds(60);
                        });
                });
        }
    }
}