namespace Candid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Cysharp.Threading.Tasks;
    
   
    using EdjCase.ICP.Candid.Models;
    //using WebSocketSharp;
    using CanisterPK.CanisterLogin;
    using CanisterPK.CanisterMatchMaking;
    using CanisterPK.CanisterStats;
    using CanisterPK.testnft;
    using CanisterPK.testicrc1;
    using CanisterPK.validator;
    using Boom;


    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;

    using EdjCase.ICP.Agent;
    using EdjCase.ICP.Agent.Agents;
    using EdjCase.ICP.Agent.Identities;
    using EdjCase.ICP.Agent.Models;

    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Signers;
    using Org.BouncyCastle.Security;


    using Newtonsoft.Json;
    using System.IO;
    using UnityEngine.SceneManagement;
    using Unity.VisualScripting;
    using UnityEngine;

    public class CandidApiManager : MonoBehaviour
    {

        public bool autoLogin = true;
        public static CandidApiManager Instance { get; private set; }
        
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
    string authTokenId = PlayerPrefs.GetString("authTokenId", "");

    if (!string.IsNullOrEmpty(authTokenId))
    {
        HandleWebLogin(authTokenId); // For web or other types of login that use an authTokenId.
    }
    else
    {
        var identity = LoadIdentityFromPlayerPrefs();
        if (identity != null)
        {
            Debug.Log("[CandidApiManager] Identity found in PlayerPrefs. Using existing identity.");
            CreateAgentUsingIdentity(identity, false).Forget();
        }
        else
        {
            Debug.Log("[CandidApiManager] No identity found in PlayerPrefs. Creating new random agent.");
            CreateAgentRandom().Forget();
        }
    }
}

private async UniTaskVoid CreateAgentUsingIdentity(Ed25519Identity identity, bool useLocalHost = false)
{
    Debug.Log("[CandidApiManager] Creating agent using existing identity.");
    await UniTask.SwitchToMainThread();
    var httpClient = new UnityHttpClient();


    var agent = new HttpAgent(new UnityHttpClient(), identity);
    await InitializeCandidApis(agent, asAnon: true);

    Debug.Log("[CandidApiManager] Random agent created and initialized.");
    if (Login.Instance != null)
    {
        Login.Instance.UpdateWindow(loginData);
        Debug.Log("[CandidApiManager] Login window updated with random agent data.");
    }
}


private Ed25519Identity LoadIdentityFromPlayerPrefs()
{
    string privateKeyBase64 = PlayerPrefs.GetString("userPrivateKey", "");
    string publicKeyBase64 = PlayerPrefs.GetString("userPublicKey", "");
    Debug.Log($"[CandidApiManager] Attempting to load identity from PlayerPrefs: PrivateKey={privateKeyBase64}, PublicKey={publicKeyBase64}");

    if (!string.IsNullOrEmpty(privateKeyBase64) && !string.IsNullOrEmpty(publicKeyBase64))
    {
        byte[] privateKey = Convert.FromBase64String(privateKeyBase64);
        byte[] publicKey = Convert.FromBase64String(publicKeyBase64);
        Ed25519Identity loadedIdentity = new Ed25519Identity(publicKey, privateKey);
        Debug.Log("[CandidApiManager] Successfully loaded identity from PlayerPrefs.");
        return loadedIdentity;
    }
    else
    {
        Debug.LogWarning("[CandidApiManager] Failed to load identity from PlayerPrefs.");
        return null;
    }
}

private void HandleWebLogin(string authTokenId)
{
    // Implement your logic for handling web login here
    Debug.Log("[CandidApiManager] Handling web login with authTokenId.");
}

public async UniTaskVoid CreateAgentRandom()
{
    Debug.Log("[CandidApiManager] Starting creation of a random agent.");
    await UniTask.SwitchToMainThread();

    var identity = GenerateEd25519Identity();

    var agent = new HttpAgent(new UnityHttpClient(), identity);
    await InitializeCandidApis(agent, asAnon: true);

    Debug.Log("[CandidApiManager] Random agent created and initialized.");
    if (Login.Instance != null)
    {
        Login.Instance.UpdateWindow(loginData);
        Debug.Log("[CandidApiManager] Login window updated with random agent data.");
    }
}

private static Ed25519Identity GenerateEd25519Identity()
{
    var secureRandom = new SecureRandom();
    var privateKeyParams = new Ed25519PrivateKeyParameters(secureRandom);
    byte[] privateKey = privateKeyParams.GetEncoded();
    byte[] publicKey = privateKeyParams.GeneratePublicKey().GetEncoded();

    // Directly save the private and public keys in Base64 format into PlayerPrefs.
    string privateKeyBase64 = Convert.ToBase64String(privateKey);
    string publicKeyBase64 = Convert.ToBase64String(publicKey);
    PlayerPrefs.SetString("userPrivateKey", privateKeyBase64);
    PlayerPrefs.SetString("userPublicKey", publicKeyBase64);
    PlayerPrefs.Save();
    Debug.Log($"[CandidApiManager] Identity saved to PlayerPrefs. PrivateKeyBase64: {privateKeyBase64}, PublicKeyBase64: {publicKeyBase64}");

    // Now create and return the Ed25519Identity using the raw byte arrays.
    return new Ed25519Identity(publicKey, privateKey);
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
            string userAccountIdentity;
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