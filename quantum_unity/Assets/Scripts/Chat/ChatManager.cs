using UnityEngine;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private GameObject messageContainer;
    [SerializeField] private TMP_Text messageText; 
    [SerializeField] private ScrollRect scrollRect;

    private WebSocket webSocket;
    public string serverUrl = "ws://74.208.246.177:8080";
    private Queue<Action> mainThreadActions = new Queue<Action>();

    private void Start()
    {
        ConnectToServer();
        messageInputField.Select();
    }

    private void Update()
    {
        while (mainThreadActions.Count > 0)
        {
            Action action = null;
            lock (mainThreadActions)
            {
                if (mainThreadActions.Count > 0)
                {
                    action = mainThreadActions.Dequeue();
                }
            }
            action?.Invoke();
        }

        if (Input.anyKeyDown)
        {
            messageInputField.Select();
        }
    }

    private void ConnectToServer()
    {
        webSocket = new WebSocket(serverUrl);
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessage;
        webSocket.OnError += OnError;
        webSocket.OnClose += OnClose;
        webSocket.ConnectAsync();
    }

    private void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connection opened.");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            string decodedMessage = System.Text.Encoding.UTF8.GetString(e.RawData);
            lock (mainThreadActions)
            {
                mainThreadActions.Enqueue(() => DisplayMessage(decodedMessage));
            }
        }
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed. Code: " + e.Code + ", Reason: " + e.Reason);
    }

    public void SendMessage(string message)
    {
        if (webSocket != null && webSocket.ReadyState == WebSocketState.Open)
        {
            webSocket.Send(message);
        }
    }

    public void OnMessageSubmit()
{
    string message = messageInputField.text;
    if (!string.IsNullOrEmpty(message))
    {
        SendMessage(message);
        messageInputField.text = ""; 
        messageInputField.ActivateInputField();
    }
}


    private void DisplayMessage(string message)
    {
        if (messageContainer != null && scrollRect.content != null)
        {
            GameObject messageInstance = Instantiate(messageContainer, scrollRect.content.transform);
            messageInstance.SetActive(true);

            TMP_Text tmpTextComponent = messageInstance.GetComponentInChildren<TMP_Text>();
            if (tmpTextComponent != null)
            {
                tmpTextComponent.text = message;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            StartCoroutine(ScrollToBottom());
        }
    }

    private IEnumerator<object> ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.CloseAsync();
        }
    }
}
