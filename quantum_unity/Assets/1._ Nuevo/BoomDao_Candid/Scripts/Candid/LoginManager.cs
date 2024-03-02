using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp.Server;
using WebSocketSharp;
using Boom.Utility;
using Boom;

namespace Candid
{
    public class LoginManager : MonoBehaviour
    {
        public static LoginManager  Instance;
        private Action<string> createIdentityCallback = null;

        [SerializeField]
        string url = "https://7p3gx-jaaaa-aaaal-acbda-cai.raw.ic0.app/";

        void Awake()
        {
            Instance = this;
            Debug.Log("[LoginManager] Awake - Instance initialized.");
        }

        public void StartLoginRandom()
        {
            Debug.Log("[LoginManager] Starting Random Login Flow.");
            BrowserUtils.ToggleLoginIframe(true);
            BoomManager.Instance.OnLoginRandomAgent();
            CandidApiManager.Instance.OnLoginRandomAgent();
        }

        /// <summary>
        /// This is the login flow using localstorage for WebGL
        /// </summary>
        public void StartLoginFlowWebGl(Action<string> _createIdentityCallback = null)
        {
            Debug.Log("Starting WebGL Login Flow");
            createIdentityCallback = _createIdentityCallback;
            BrowserUtils.ToggleLoginIframe(true);
        }

        public void CreateIdentityWithJson(string identityJson)
        {
            Debug.Log("[LoginManager] Creating identity with JSON.");
            createIdentityCallback?.Invoke(identityJson);
            createIdentityCallback = null;
            BrowserUtils.ToggleLoginIframe(false);
            
            CloseSocket();
        }

        public void SendCanisterIdsToWebpage(Action<string> send)
        {
            List<string> targetCanisterIds = new List<string>(); // This is where you'd specify the list of World canister ids this game controls
            send(JsonConvert.SerializeObject(new WebsocketMessage(){type = "targetCanisterIds", content = JsonConvert.SerializeObject(targetCanisterIds)}));
        }

        public void CancelLogin()
        {
            Debug.Log("[LoginManager] Cancelling login and closing WebSocket server if active.");
            BrowserUtils.ToggleLoginIframe(false);
            if (wssv != null)
            {
                wssv.Stop();
                wssv = null;
            }
            else
            {
                Debug.Log("[LoginManager] No WebSocket server to stop.");
            }
        }

        /// <summary>
        /// This is the login flow using websockets for PC, Mac, iOS, and Android
        /// </summary>
        public void StartLoginFlow(Action<string> _createIdentityCallback = null)
        {
            Debug.Log("[LoginManager] Starting Login Flow (WebSocket-based)");
            createIdentityCallback = _createIdentityCallback;
            StartSocket();
            Debug.Log("[LoginManager] Opening login URL in default browser: " + url);
            Application.OpenURL(url);
        }

        WebSocketServer wssv;

        private void StartSocket()
        {
            wssv = new WebSocketServer("ws://127.0.0.1:8080");
            wssv.AddWebSocketService<Data>("/Data");
            wssv.Start();
        }

        public void CloseSocket()
        {
            Debug.Log("[LoginManager] CloseSocket called. Stopping WebSocket server.");
            "CloseWebSocket".Log();

            wssv.Stop();
            wssv = null;
            Debug.Log("[LoginManager] WebSocket server stopped successfully.");
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
            ("Websocket Message Received: " + e.Data).Log();
            


            WebsocketMessage message = JsonConvert.DeserializeObject<WebsocketMessage>(e.Data);
            
            if (message == null)
            {
                Debug.LogError("Error: Unable to parse websocket message, does it follow the correct WebsocketMessage structure?");
                return;
            }
            
            switch (message.type)
            {
                case "fetchCanisterIds":
                    LoginManager.Instance.SendCanisterIdsToWebpage(Send);
                    break;
                case "identityJson":
                    LoginManager.Instance.CreateIdentityWithJson(message.content);
                    break; 
                default:
                    Debug.LogError("No corresponding websocket message type found for=" + message.type);
                    break;
            }
        }
    }

}