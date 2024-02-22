using UnityEngine;
using System;
using System.Collections.Generic;
using WebSocketSharp;

public class ChatManager : MonoBehaviour
{
    private WebSocket webSocket;
    public string serverUrl = "ws://74.208.246.177:8080";
    public event Action<string> OnMessageReceived;
    private Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        webSocket = new WebSocket(serverUrl);
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessage;
        webSocket.OnError += OnError;
        webSocket.OnClose += OnClose;
        webSocket.Connect();
    }

    public void SendMessage(string message)
    {
        if (webSocket != null && webSocket.ReadyState == WebSocketState.Open)
        {
            webSocket.Send(message);
            Debug.Log("Message sent: " + message);
        }
        else
        {
            Debug.LogWarning("WebSocket is not open. Cannot send message.");
        }
    }

    private void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connection opened.");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        try
        {
            string message = System.Text.Encoding.UTF8.GetString(e.RawData);
            Debug.Log("Received message from server: " + message);
            EnqueueMessage(message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception in OnMessage: " + ex.Message);
        }
    }

    private void EnqueueMessage(string message)
    {
        lock (messageQueue)
        {
            messageQueue.Enqueue(message);
        }
        MainThreadDispatcher.Enqueue(() => OnMessageReceived?.Invoke(message));
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed. Code: " + e.Code + ", Reason: " + e.Reason);
    }

    void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }
}
