using UnityEngine;
using System;
using WebSocketSharp.Server;
using WebSocketSharp;
using Newtonsoft.Json;

namespace Candid
{
    public class LoginManager : MonoBehaviour
    {
        public static LoginManager  Instance;
        private Action<string> callback = null;
        

        [SerializeField]
        string url = "https://7p3gx-jaaaa-aaaal-acbda-cai.raw.ic0.app/";

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("[LoginManager] Instance already exists. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Debug.Log("[LoginManager] Awake - Instance initialized.");
        }

        public void StartLoginRandom()
        {
            Debug.Log("[LoginManager] Starting Random Login Flow.");
            BrowserUtils.ToggleLoginIframe(true);
            CandidApiManager.Instance.OnLoginRandomAgent();
        }
        /// <summary>
        /// This is the login flow using localstorage for WebGL
        /// </summary>
        public void StartLoginFlowWebGl(Action<string> _callback = null)
        {
            Debug.Log("[LoginManager] Starting WebGL Login Flow. Callback set: " + (_callback != null));
            callback = _callback;
            BrowserUtils.ToggleLoginIframe(true);
        }
        public void ExecuteCallbackWithJson(string identityJson)
        {
            if (callback != null)
            {
                Debug.Log("[LoginManager] Executing callback with JSON.");
                callback(identityJson);
                callback = null; // Ensure callback is only called once
            }
            else
            {
                Debug.LogWarning("[LoginManager] Callback is null, cannot execute.");
            }
            BrowserUtils.ToggleLoginIframe(false);
        }
         public void CancelLogin()
        {
            Debug.Log("[LoginManager] Cancelling login and closing WebSocket server if active.");
            BrowserUtils.ToggleLoginIframe(false);
            if (wssv != null)
            {
                wssv.Stop();
                wssv = null;
                Debug.Log("[LoginManager] WebSocket server stopped.");
            }
            else
            {
                Debug.Log("[LoginManager] No WebSocket server to stop.");
            }
        }

        /// <summary>
        /// This is the login flow using websockets for PC, Mac, iOS, and Android
        /// </summary>
        public void StartLoginFlow(Action<string> _callback = null)
        {
            Debug.Log("[LoginManager] Starting Login Flow (WebSocket-based). Callback set: " + (_callback != null));
            callback = _callback;
            StartSocket();
            Debug.Log("[LoginManager] Opening login URL in default browser: " + url);
            Application.OpenURL(url);
        }

        WebSocketServer wssv;

        private void StartSocket()
        {
            Debug.Log("[LoginManager] Initializing WebSocket server on ws://127.0.0.1:8080.");
            wssv = new WebSocketServer("ws://127.0.0.1:8080");

            Debug.Log("[LoginManager] Adding service endpoint '/Data' (handled by Data class)");
            wssv.AddWebSocketService<Data>("/Data");

            Debug.Log("Starting WebSocket server");
            wssv.Start();
            Debug.Log("[LoginManager] WebSocket server is now active and listening for connections");
        }

        public void CloseSocket(string identity)
        {
            Debug.Log("[LoginManager] CloseSocket called. Stopping WebSocket server.");
            if (wssv != null)
            {
                wssv.Stop();
                wssv = null;
                Debug.Log("[LoginManager] WebSocket server stopped successfully.");
            }
            else
            {
                Debug.LogWarning("[LoginManager] CloseSocket called but WebSocket server is already null.");
            }

            Debug.Log("[LoginManager] Received identity for callback: " + identity);
            ExecuteCallbackWithJson(identity);
        }
    }

    public class WebsocketMessage
    {
        public string type;
        public string content;
    }


    public class Data : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log("Received message: " + e.Data);
            WebsocketMessage message = JsonConvert.DeserializeObject<WebsocketMessage>(e.Data);
            if (message == null)
            {
                Debug.LogError("Error: Unable to parse websocket message.");
                return;
            }

            LoginManager.Instance.CloseSocket(e.Data);
        }

        protected override void OnOpen()
        {
            Debug.Log("[Data] WebSocket OnOpen: Connection opened.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log($"[Data] WebSocket OnClose: Connection closed. Code: {e.Code}, Reason: {e.Reason}");
        }
    }
}
