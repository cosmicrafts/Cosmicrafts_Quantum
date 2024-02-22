using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private ChatManager chatManager;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private GameObject messageContainer;
    [SerializeField] private GameObject messagePrefab;

    private Queue<string> messageQueue = new Queue<string>(); // Message queue for main thread

    void Start()
    {
        messageInputField.onEndEdit.AddListener(OnEndEdit);
        chatManager.OnMessageReceived += AddMessageToQueue;
    }

    void OnDestroy()
    {
        messageInputField.onEndEdit.RemoveListener(OnEndEdit);
        chatManager.OnMessageReceived -= AddMessageToQueue;
    }

    private void OnEndEdit(string text)
    {
        // ... (Your existing send message logic) ...
    }

    private void AddMessageToQueue(string message)
    {
        lock (messageQueue) // Ensure thread safety when adding to queue
        {
            messageQueue.Enqueue(message); 
        }
    }

    private void Update()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                ProcessMessage(message);
            }
        }
    }

    public void SendMessage()
    {
        string message = messageInputField.text;
        chatManager.SendMessage(message);
        messageInputField.text = "";
    }

    private void ProcessMessage(string message)
    {
        // Create message object and add it to the display
        GameObject messageObject = Instantiate(messagePrefab, messageContainer.transform);
        messageObject.GetComponent<TMP_Text>().text = message;
    }

        private void AddMessageToHistory(string message)
    {
        GameObject messageObject = Instantiate(messagePrefab, messageContainer.transform);
        messageObject.GetComponent<TMP_Text>().text = message;
    }
}
