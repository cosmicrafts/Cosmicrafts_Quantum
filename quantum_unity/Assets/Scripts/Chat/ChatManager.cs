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
    [SerializeField] private TMP_Text usernameText;

    private WebSocket webSocket;
    public string serverUrl = "ws://74.208.246.177:8080";
    private Queue<Action> mainThreadActions = new Queue<Action>();
    public string username;

    private void Start()
    {
        ConnectToServer();
        messageInputField.Select();
        SetUsername(GlobalGameData.Instance.GetUserData().NikeName);
    }

    public void SetUsername(string username)
    {
        this.username = username;
        if (usernameText != null)
        {
            usernameText.text = username;
        }
    }
/*
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
    */

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
        
        string messageToSend = $"{username}: {message}";
        webSocket.Send(messageToSend);
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

        // Split the received message into username and message parts
        string[] messageParts = message.Split(new string[] { ": " }, 2, StringSplitOptions.None);
        string senderUsername = messageParts[0];
        string actualMessage = messageParts.Length > 1 ? messageParts[1] : "";

        // Find the TMP_Text components for username and message within the instantiated message prefab
        TMP_Text[] tmpTextComponents = messageInstance.GetComponentsInChildren<TMP_Text>();
        if (tmpTextComponents.Length >= 2)
        {
            tmpTextComponents[0].text = senderUsername; // Assuming the first TMP_Text is for the username
            tmpTextComponents[1].text = actualMessage; // Assuming the second TMP_Text is for the message
        }
        else if (tmpTextComponents.Length == 1)
        {
            // Fallback in case there's only one TMP_Text component, display the whole message
            tmpTextComponents[0].text = message;
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
        Debug.Log("ChatManager OnDestroy called.");
        if (webSocket != null)
        {
            webSocket.CloseAsync();
        }
    }

}
