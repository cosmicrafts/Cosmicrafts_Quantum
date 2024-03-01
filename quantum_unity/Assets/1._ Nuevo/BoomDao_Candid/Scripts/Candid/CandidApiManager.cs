namespace Candid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candid.Extv2Standard;
    using Candid.Extv2Boom;
    using Candid.IcpLedger;
    using Candid.IcpLedger.Models;
    using Candid.World;
    using Candid.WorldHub;
    using Cysharp.Threading.Tasks;
    using EdjCase.ICP.Agent.Agents;
    using EdjCase.ICP.Agent.Identities;
    using EdjCase.ICP.Candid.Models;
    using Boom.Patterns.Broadcasts;
    using Boom.Utility;
    using Boom.Values;
    using UnityEngine;
    using Candid.IcrcLedger;
    using Unity.VisualScripting;
    using Boom;
    using EdjCase.ICP.BLS;
    using Newtonsoft.Json;
    using UnityEngine.Events;
    using CanisterPK.CanisterLogin;
    using CanisterPK.CanisterMatchMaking;
    using CanisterPK.CanisterStats;
    using CanisterPK.testnft;
    using CanisterPK.testicrc1;
    using CanisterPK.validator;
    using UnityEngine.SceneManagement;
    //using WebSocketSharp;
    

    public class CandidApiManager : MonoBehaviour
    {

        public bool autoLogin = true;
        public static CandidApiManager Instance { get; private set; }
        public UnityEvent onLoginCompleted;
        
        // Canister APIs
        public CanisterLoginApiClient CanisterLogin { get; private set; }
        public CanisterMatchMakingApiClient CanisterMatchMaking { get; private set; }
        public CanisterStatsApiClient CanisterStats { get; private set; }
        public TestnftApiClient testnft { get; private set; }
        public Testicrc1ApiClient testicrc1{ get; private set; }
        public ValidatorApiClient Validator { get; private set; }
            
            
        // Login Data
        public enum DataState { None, Loading, Ready }
        public struct LoginData 
        {
            public IAgent agent;
            public string principal;
            public string accountIdentifier;
            public bool asAnon;
            public DataState state ;
            
            public LoginData(IAgent agent, string principal, string accountIdentifier, bool asAnon, DataState state)
            {
                this.agent = agent;
                this.principal = principal;
                this.accountIdentifier = accountIdentifier;
                this.asAnon = asAnon;
                this.state = state;
            }
        }
        public LoginData loginData = new LoginData(null, null, null, false, DataState.None);
        
        

        private void Awake()
        {
            if (onLoginCompleted == null) {
                onLoginCompleted = new UnityEvent();
            }
            Debug.Log("[CandidApiManager] Awake called. Initializing instance.");
            if (Instance != null)
            {
                Debug.LogWarning("[CandidApiManager] Instance already exists. Destroying the new one.");
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[CandidApiManager] Instance set and marked as DontDestroyOnLoad.");
        }
        
        private void Start()
        {
            Debug.Log("[CandidApiManager] Start called.");
            if (PlayerPrefs.HasKey("authTokenId") && autoLogin)
            {
                Debug.Log("[CandidApiManager] Saved login found. Registering Candid APIs.");
                LoadingPanel.Instance.ActiveLoadingPanel();
                OnLoginCompleted(PlayerPrefs.GetString("authTokenId"));
            }
            else
            {
                Debug.Log("[CandidApiManager] No saved login found.");
            }
        }


        public void StartLogin()
        {
            Debug.Log("[CandidApiManager] StartLogin called.");
            if (loginData.state == DataState.Ready) 
            {
                Debug.Log("[CandidApiManager] Login data is already ready. Aborting StartLogin.");
                return;
            }
            
            #if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("[CandidApiManager] Starting WebGL login flow.");
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            #else
            Debug.Log("[CandidApiManager] Starting login flow.");
            LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
            #endif
        }
        
        
        public void OnLoginCompleted(string json)
        {
            Debug.Log("[CandidApiManager] OnLoginCompleted called. Login completed. Creating agent...");
            CreateAgentUsingIdentityJson(json, false).Forget();
            onLoginCompleted.Invoke();
        }

        public async UniTaskVoid CreateAgentUsingIdentityJson(string json, bool useLocalHost = false)
        {
            Debug.Log($"[CandidApiManager] Attempting to create agent with JSON. LocalHost: {useLocalHost}");
            await UniTask.SwitchToMainThread();
            try
            {
                var identity = Identity.DeserializeJsonToIdentity(json);
                Debug.Log("[CandidApiManager] Identity deserialized successfully.");

                var httpClient = new UnityHttpClient();

                if (useLocalHost) 
                {
                    Debug.Log("[CandidApiManager] Initializing Candid APIs using localhost.");
                    await InitializeCandidApis(new HttpAgent(identity, new Uri("http://localhost:4943")));
                }
                else 
                {
                    Debug.Log("[CandidApiManager] Initializing Candid APIs.");
                    await InitializeCandidApis(new HttpAgent(httpClient, identity));
                }

                Debug.Log("[CandidApiManager] Agent creation finished. Logged in successfully.");
                PlayerPrefs.SetString("authTokenId", json);

                if (Login.Instance != null)
                {
                    Login.Instance.UpdateWindow(loginData);
                }
                Debug.Log("[CandidApiManager] Exiting CreateAgentUsingIdentityJson.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[CandidApiManager] Agent creation error: {e.Message}");
            }
        }

        public void OnLoginRandomAgent()
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            CreateAgentRandom().Forget();
        }
        public async UniTaskVoid CreateAgentRandom()
        {
            Debug.Log("[CandidApiManager] Starting creation of a random agent. Entering CreateAgentRandom method.");
            await UniTask.SwitchToMainThread();
            
            try
            {
                // Define a local function to encapsulate the agent creation logic.
                IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
                {
                    Debug.Log($"[CandidApiManager] Attempting to create a random agent. LocalHost: {useLocalHost}");
                    IAgent randomAgent = null;
                    var httpClient = new UnityHttpClient();

                    try
                    {
                        string uri = useLocalHost ? "http://localhost:4943" : "Using default agent URI";
                        Debug.Log($"[CandidApiManager] Creating HttpAgent with URI: {uri}");

                        if (useLocalHost)
                            randomAgent = new HttpAgent(Ed25519Identity.Generate(), new Uri("http://localhost:4943"));
                        else
                            randomAgent = new HttpAgent(httpClient, Ed25519Identity.Generate());

                        Debug.Log("[CandidApiManager] Random agent created successfully.");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[CandidApiManager] Failed to create random agent. Error: {e.Message}");
                    }

                    return randomAgent;
                }

                // Initiate the creation of a random agent and its initialization within Candid APIs.
                var createdAgent = CreateAgentWithRandomIdentity(useLocalHost: false); // Adjust useLocalHost as needed.
                if (createdAgent != null)
                {
                    Debug.Log("[CandidApiManager] Random agent creation succeeded. Proceeding to initialize Candid APIs.");
                    await InitializeCandidApis(createdAgent, asAnon: true); // Assuming random agents are always treated as anonymous.
                    Debug.Log("[CandidApiManager] Candid APIs initialized for random agent.");
                }
                else
                {
                    Debug.LogError("[CandidApiManager] Random agent creation failed. Unable to proceed with Candid API initialization.");
                }

                if (Login.Instance != null)
                {
                    Login.Instance.UpdateWindow(loginData);
                    Debug.Log("[CandidApiManager] Login window updated with new random agent data.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[CandidApiManager] Exception caught in CreateAgentRandom. Error: {e.Message}");
            }
            Debug.Log("[CandidApiManager] Exiting CreateAgentRandom method.");
        }
        
        public void LogOut( )
        {
            Debug.Log("[CandidApiManager] Initiating logout process.");
            PlayerPrefs.DeleteKey("authTokenId");
            DesInitializeCandidApis();
            SceneManager.LoadScene(0);
            Debug.Log("[CandidApiManager] Logout process completed. Scene reloaded.");
            //NowCanLogin
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actiontType">If the type is "Update" then must use the return value once at a time to record the update call</param>
        /// <returns></returns>
        private async UniTask InitializeCandidApis(IAgent agent, bool asAnon = false)
        {
            Debug.Log($"[CandidApiManager] Initializing Candid APIs. Anonymous: {asAnon}");
            var userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();

            //Check if anon setup is required
            if (asAnon)
                
            {
                //Build Interfaces
                CanisterLogin =  new CanisterLoginApiClient(agent, Principal.FromText("woimf-oyaaa-aaaan-qegia-cai"));
                CanisterMatchMaking =  new CanisterMatchMakingApiClient(agent, Principal.FromText("vqzll-jiaaa-aaaan-qegba-cai"));
                CanisterStats =  new CanisterStatsApiClient(agent, Principal.FromText("jybso-3iaaa-aaaan-qeima-cai"));
                testnft = new TestnftApiClient(agent, Principal.FromText("phgme-naaaa-aaaap-abwda-cai"));                
                testicrc1 = new Testicrc1ApiClient(agent, Principal.FromText("svcoe-6iaaa-aaaam-ab4rq-cai"));
                Validator = new ValidatorApiClient(agent, Principal.FromText("2dzox-tqaaa-aaaan-qlphq-cai"));
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);
            }
            else
            {
                //Build Interfaces
                CanisterLogin =  new CanisterLoginApiClient(agent, Principal.FromText("woimf-oyaaa-aaaan-qegia-cai"));
                CanisterMatchMaking =  new CanisterMatchMakingApiClient(agent, Principal.FromText("vqzll-jiaaa-aaaan-qegba-cai"));
                CanisterStats =  new CanisterStatsApiClient(agent, Principal.FromText("jybso-3iaaa-aaaan-qeima-cai"));
                testnft = new TestnftApiClient(agent, Principal.FromText("phgme-naaaa-aaaap-abwda-cai"));                
                testicrc1 = new Testicrc1ApiClient(agent, Principal.FromText("svcoe-6iaaa-aaaam-ab4rq-cai"));
                Validator = new ValidatorApiClient(agent, Principal.FromText("2dzox-tqaaa-aaaan-qlphq-cai"));                
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);
            }
            Debug.Log("[CandidApiManager] Candid APIs initialized successfully.");
        }
        
        private void DesInitializeCandidApis()
        {
            Debug.Log("[CandidApiManager] Deinitializing Candid APIs and resetting login data.");
            CanisterLogin = null;
            CanisterMatchMaking = null;
            CanisterStats = null;
            testnft = null;              
            testicrc1 = null;
            Validator = null;                
            
            //Set Login Data
            loginData = new LoginData(null, null, null, false, DataState.None);
            Debug.Log("[CandidApiManager] Candid APIs deinitialized.");
        }
    }
}