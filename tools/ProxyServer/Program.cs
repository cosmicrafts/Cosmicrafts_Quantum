using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ProxyServer
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Starting server...");
      IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
          config.AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
          services.Configure<WebSocketServerSettings>(
          hostContext.Configuration.GetSection("WebSocketServer"));
          services.AddSingleton<WebSocketServer>();
          services.AddHostedService<WebSocketServer>();
        }).Build();
      await host.StartAsync();
      Console.WriteLine("Proxy started.");
      await host.WaitForShutdownAsync();
    }
  }
}