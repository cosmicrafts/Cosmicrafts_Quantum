using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;
using Cosmicrafts.Data;
using Cosmicrafts.Managers;

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
        if (GameDataManager.Instance != null)
        {
            var userData = GameDataManager.Instance.playerData;
            if (userData != null)
            {
                SetUsername(userData.Username);
            }
        }
        else
        {
            Debug.LogError("[ChatManager] GameDataManager instance is null.");
        }

        ConnectToServer();
        messageInputField.Select();
    }

    public void SetUsername(string username)
    {
        this.username = username;
        if (usernameText != null)
        {
            usernameText.text = username;
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

            string[] messageParts = message.Split(new string[] { ": " }, 2, StringSplitOptions.None);
            string senderUsername = messageParts[0];
            string actualMessage = messageParts.Length > 1 ? messageParts[1] : "";

            TMP_Text[] tmpTextComponents = messageInstance.GetComponentsInChildren<TMP_Text>();
            if (tmpTextComponents.Length >= 2)
            {
                tmpTextComponents[0].text = senderUsername;
                tmpTextComponents[1].text = actualMessage;
            }
            else if (tmpTextComponents.Length == 1)
            {
                tmpTextComponents[0].text = message;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            StartCoroutine(ScrollToBottom());
        }
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    private void Update()
    {
        // Execute actions queued on the main thread
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue().Invoke();
            }
        }
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
