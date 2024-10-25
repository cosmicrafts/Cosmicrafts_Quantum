using System.Net;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using System.Text;
using EdjCase.ICP.Agent;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.WebSockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ProxyServer.Cosmicrafts;
using ProxyServer.Cosmicrafts.Models;

namespace ProxyServer
{
  public class WebSocketServer : IHostedService
  {
    private readonly ILogger<WebSocketServer> _logger;
    private readonly WebSocketServerSettings _settings;
    private HttpListener? _listener;

    public WebSocketServer(
      ILogger<WebSocketServer> logger,
      IOptions<WebSocketServerSettings> settings)
    {
      _logger = logger;
      _settings = settings.Value;
      if (_settings.Port <= 0 || _settings.Port > 65535)
        throw new ArgumentException("Invalid WebSocket port");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _listener = new HttpListener();
      _listener.Prefixes.Add($"http://localhost:{_settings.Port}/");
      _logger.LogInformation("Starting proxy on port {Port}", _settings.Port);
      _listener.Start();
      Task.Run(async () =>
      {
        while (!cancellationToken.IsCancellationRequested)
        {
          var context = await _listener.GetContextAsync();
          if (context.Request.IsWebSocketRequest)
          {
            var webSocketContext = await
            context.AcceptWebSocketAsync(null);
            _ = HandleWebSocketConnection(
              webSocketContext.WebSocket,
                cancellationToken
                );
          }
          else
          {
            context.Response.StatusCode = 400;
            context.Response.Close();
          }
        }
      }, cancellationToken);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _listener?.Stop();
      return Task.CompletedTask;
    }

    private async Task HandleWebSocketConnection(
      WebSocket webSocket,
      CancellationToken cancellationToken)
    {
      var buffer = new byte[1024 * 4];
      WebSocketReceiveResult result = await
        webSocket.ReceiveAsync(
          new ArraySegment<byte>(buffer),
          cancellationToken
        );

      while (!result.CloseStatus.HasValue)
      {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var canisterResponse = await CallCanister(message);
        var responseBuffer = Encoding.UTF8.GetBytes(
        canisterResponse?.ToString() ?? string.Empty);
        await webSocket.SendAsync(
          new ArraySegment<byte>(responseBuffer),
          WebSocketMessageType.Text, true,
          cancellationToken);
        _logger.LogInformation(
          "Response sent to client"
          );
        result = await webSocket.ReceiveAsync(
          new ArraySegment<byte>(buffer),
          cancellationToken
          );
      }
      await webSocket.CloseAsync(
        result.CloseStatus.Value,
        result.CloseStatusDescription,
        cancellationToken);
    }

    private async Task<object> CallCanister(string unityMsg)
    {
      _logger.LogInformation("Received command: {cmd}", unityMsg);
      var parts = unityMsg.Split(' ');
      if (parts.Length < 2)
      {
        _logger.LogError(
          "Invalid command format. Use: className functionName arg1 arg2 ...");
        return "Invalid command format";
      }
      string className = parts[0];
      string functionName = parts[1];
      var args = parts.Skip(2).ToArray();
      try
      {
        Uri uri = new Uri("wss://localhost:8080");
        Principal id = Principal.FromText("bkyz2-fmaaa-aaaaa-qaaaq-cai");
        var builder = new WebSocketBuilder<AppMessage>(id, uri);
        _logger.LogInformation("WebSocketBuilder created.");
        HttpAgent agent = new(httpBoundryNodeUrl: new Uri("http://localhost:4943"));
        SubjectPublicKeyInfo devRootKey = await agent.GetRootKeyAsync();
        builder = builder.WithRootKey(devRootKey);
        _logger.LogInformation("Root key set successfully.");

        object classInstance;
        if (className == "Cosmicrafts")
        {
          classInstance = new CosmicraftsApiClient(agent,
          Principal.FromText("bkyz2-fmaaa-aaaaa-qaaaq-cai"));
          _logger.LogInformation("CosmicraftsApiClient created.");
          Type classType = classInstance.GetType();
          MethodInfo? method = classType.GetMethod(functionName);
          if (method == null)
          {
            _logger.LogError(
              $"Method '{functionName}' not found in class '{className}'.");
            return $"Method '{functionName}' not found in class '{className}'.";
          }
          ParameterInfo[] parameters = method.GetParameters();
          if (parameters.Length != args.Length)
          {
            _logger.LogError(
              $"Parameter count mismatch. Expected {parameters.Length}, but got {args.Length}.");
            return
            $"Parameter count mismatch. Expected {parameters.Length}, but got {args.Length}.";
          }
          object[] convertedArgs = new object[args.Length];
          for (int i = 0; i < args.Length; i++)
          {
            convertedArgs[i] = ConvertArgument(args[i], parameters[i].ParameterType);
          }
          object? result = method.Invoke(classInstance, convertedArgs);
          if (result is Task task)
          {
            await task.ConfigureAwait(false);
            if (task.GetType().IsGenericType)
            {
              var resultProperty = task.GetType().GetProperty("Result");
              if (resultProperty != null)
              {
                return resultProperty?.GetValue(task) ??
              "Method invocation returned null.";
              }
            }
          }
        }
        else
        {
          _logger.LogError($"Class '{className}' not supported.");
          return $"Class '{className}' not found.";
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to call canister function");
        return "An error occurred while calling the canister function.";
      }
      return "null";
    }

    // Helper method to convert arguments based on their expected types
    private object ConvertArgument(string arg, Type targetType)
    {
      if (targetType == typeof(UnboundedUInt))
      {
        // Convert string to UnboundedUInt (assuming it takes a string or BigInteger)
        return UnboundedUInt.FromBigInteger(BigInteger.Parse(arg));
      }
      // Add other custom types here as necessary
      return Convert.ChangeType(arg, targetType); // Fallback to default conversion
    }
  }

  internal class AppMessage
  {
    public string? Message { get; set; }

  }

  public class WebSocketServerSettings
  {
    public int? Port { get; set; } = 8081; // Default value
  }
}