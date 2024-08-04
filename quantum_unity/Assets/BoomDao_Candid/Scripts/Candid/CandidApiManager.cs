using TowerRush;

namespace Candid
{
    using System;
    using System.Text;
    using Cysharp.Threading.Tasks;
    using EdjCase.ICP.Candid.Models;
    //using WebSocketSharp;
    using CanisterPK.CanisterLogin;
    using CanisterPK.testnft;
    using CanisterPK.testicrc1;
    using CanisterPK.flux;
    using CanisterPK.chests;
    using CanisterPK.tournaments;
    using Candid.IcpLedger;
    using CanisterPK.BoomToken;

    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Parameters;

    using EdjCase.ICP.Agent.Agents;
    using EdjCase.ICP.Agent.Identities;

    using Newtonsoft.Json;
    using UnityEngine;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class IdentityMessage
    {
        public string Type { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    public class CandidApiManager : MonoBehaviour
    {
        public bool autoLogin = true;
        public static CandidApiManager Instance { get; private set; }

        // Canister APIs
        public CanisterLoginApiClient CanisterLogin { get; private set; }
        public TestnftApiClient testnft { get; private set; }
        public Testicrc1ApiClient testicrc1 { get; private set; }
        public FluxApiClient flux { get; private set; }
        public ChestsApiClient chests { get; private set; }
        public IcpLedgerApiClient icptoken { get; private set; }
        public BoomTokenApiClient boomToken { get; private set; }
        public TournamentsApiClient tournaments { get; private set; }

        // Login Data
        public enum DataState { None, Loading, Ready }
        public struct LoginData
        {
            public IAgent agent;
            public string principal;
            public string accountIdentifier;
            public bool asAnon;
            public DataState state;

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
        private static readonly ConcurrentQueue<Action> _executeOnMainThread = new ConcurrentQueue<Action>();

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

        private async void Start()
        {
            Debug.Log("[CandidApiManager] Start called.");
            // Check for saved web login
            string authTokenId = await AsyncLocalStorage.LoadDataAsync("authTokenId");
            Debug.Log($"[CandidApiManager] authTokenId loaded: {authTokenId}");
            if (authTokenId != null && autoLogin)
            {
                LoadingPanel.Instance.ActiveLoadingPanel();
                Debug.Log("[CandidApiManager] Saved web login found.");
                CreateAgentUsingIdentityJson(authTokenId, false).Forget();
            }
            else
            {
                var identity = await LoadIdentityFromAsyncStorage();
                if (identity != null && autoLogin)
                {
                    LoadingPanel.Instance.ActiveLoadingPanel();
                    Debug.Log("[CandidApiManager] Saved random login found. Using existing identity.");
                    CreateAgentUsingIdentity(identity, false).Forget();
                }
                else
                {
                    Debug.Log("[CandidApiManager] No saved login found.");
                }
            }
        }

        public void StartLogin(string loginMessage = null)
        {
            Debug.Log("[CandidApiManager] StartLogin called.");
            if (loginData.state == DataState.Ready)
            {
                Debug.Log("[CandidApiManager] Login data is already ready. Aborting StartLogin.");
                return;
            }

            if (!string.IsNullOrEmpty(loginMessage))
            {
                ProcessLoginMessage(loginMessage);
                return; // Return early since we're handling a specific login message.
            }

            #if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("[CandidApiManager] Starting WebGL login flow.");
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            #else
            Debug.Log("[CandidApiManager] Starting standard login flow.");
            LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
            #endif
        }

        public void ProcessLoginMessage(string loginMessage)
        {
            Debug.Log("[CandidApiManager] Received login message for processing.");

            try
            {
                // Deserialize the message to extract the identity details
                var identityMessage = JsonConvert.DeserializeObject<IdentityMessage>(loginMessage);
                if (identityMessage?.Type == "Ed25519Identity")
                {
                    Debug.Log("[CandidApiManager] Processing Ed25519Identity message.");
                    // Convert the message to an Ed25519Identity and proceed with creating an agent
                    var identity = ConvertMessageToIdentity(identityMessage);
                    CreateAgentUsingIdentity(identity, false).Forget();

                    MainThreadDispatcher.Enqueue(() => {
                        AsyncLocalStorage.SaveDataAsync("userPrivateKey", identityMessage.PrivateKey).AsUniTask().Forget();
                        AsyncLocalStorage.SaveDataAsync("userPublicKey", identityMessage.PublicKey).AsUniTask().Forget();
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CandidApiManager] Error processing login message: {ex.Message}");
            }
        }

        private Ed25519Identity ConvertMessageToIdentity(IdentityMessage message)
        {
            byte[] publicKey = Convert.FromBase64String(message.PublicKey);
            byte[] privateKey = Convert.FromBase64String(message.PrivateKey);
            return new Ed25519Identity(publicKey, privateKey);
        }

        private async Task SaveIdentityToAsyncStorage(string publicKeyBase64, string privateKeyBase64)
        {
            await AsyncLocalStorage.SaveDataAsync("userPrivateKey", privateKeyBase64);
            await AsyncLocalStorage.SaveDataAsync("userPublicKey", publicKeyBase64);
            Debug.Log("[CandidApiManager] Identity saved to AsyncLocalStorage.");
        }

        private async Task<Ed25519Identity> LoadIdentityFromAsyncStorage()
        {
            string privateKeyBase64 = await AsyncLocalStorage.LoadDataAsync("userPrivateKey");
            string publicKeyBase64 = await AsyncLocalStorage.LoadDataAsync("userPublicKey");
            Debug.Log($"[CandidApiManager] Attempting to load identity from AsyncLocalStorage: PrivateKey={privateKeyBase64}, PublicKey={publicKeyBase64}");

            if (!string.IsNullOrEmpty(privateKeyBase64) && !string.IsNullOrEmpty(publicKeyBase64))
            {
                byte[] privateKey = Convert.FromBase64String(privateKeyBase64);
                byte[] publicKey = Convert.FromBase64String(publicKeyBase64);
                Ed25519Identity loadedIdentity = new Ed25519Identity(publicKey, privateKey);
                Debug.Log("[CandidApiManager] Successfully loaded identity from AsyncLocalStorage.");
                return loadedIdentity;
            }
            else
            {
                Debug.LogWarning("[CandidApiManager] Failed to load identity from AsyncLocalStorage.");
                return null;
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

        public void OnLoginRandomAgent()
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            CreateAgentRandom().Forget();
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

        public void SetTestSeedPhrase(string seedPhrase)
        {
            testSeedPhrase = seedPhrase;
            Debug.Log($"Set testSeedPhrase to: {testSeedPhrase}");
        }

        public static string testSeedPhrase = "wrong swear claim hold sunny prepare dove swamp clip home extend exercise";

        private static Ed25519Identity GenerateEd25519Identity()
        {
            byte[] seedBytes = Encoding.UTF8.GetBytes(testSeedPhrase);

            // Deterministic derivation using SHA-256
            var sha256 = new Sha256Digest();
            byte[] hashOutput = new byte[sha256.GetDigestSize()];
            sha256.BlockUpdate(seedBytes, 0, seedBytes.Length);
            sha256.DoFinal(hashOutput, 0);

            // Use the first 32 bytes of the hash as the private key
            var privateKey = new Ed25519PrivateKeyParameters(hashOutput, 0);

            // Derive the public key
            var publicKey = privateKey.GeneratePublicKey();

            // Directly save the private and public keys in Base64 format into AsyncLocalStorage.
            string privateKeyBase64 = Convert.ToBase64String(privateKey.GetEncoded());
            string publicKeyBase64 = Convert.ToBase64String(publicKey.GetEncoded());

            AsyncLocalStorage.SaveDataAsync("userPrivateKey", privateKeyBase64).AsUniTask().Forget();
            AsyncLocalStorage.SaveDataAsync("userPublicKey", publicKeyBase64).AsUniTask().Forget();

            Debug.Log($"[CandidApiManager] Identity saved to AsyncLocalStorage. PrivateKeyBase64: {privateKeyBase64}, PublicKeyBase64: {publicKeyBase64}");

            // Now create and return the Ed25519Identity using the raw byte arrays.
            return new Ed25519Identity(publicKey.GetEncoded(), privateKey.GetEncoded());
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

        // Corrected call to SaveDataAsync
        await AsyncLocalStorage.SaveDataAsync("authTokenId", json);

        if (Login.Instance != null)
        {
            Login.Instance.UpdateWindow(loginData);
        }
        Debug.Log("[CandidApiManager] Exiting CreateAgentUsingIdentityJson.");
    }
    catch (Exception e)
    {
        Debug.LogError($"[CandidApiManager] Agent creation error: {e.Message}");

        // Fallback to ensure no further login attempts
        Debug.Log("[CandidApiManager] Aborting login process due to error.");
        AbortLogin();
    }
}

private void AbortLogin()
{
    Debug.Log("[CandidApiManager] Aborting login. Cleaning up any initialized components.");

    // Clean up any initialized API clients or reset states
    DesInitializeCandidApis();

    // Optionally, notify the user or reset the UI state
    if (LoadingPanel.Instance != null)
    {
        LoadingPanel.Instance.DesactiveLoadingPanel();
    }

    Debug.Log("[CandidApiManager] Login process aborted and components cleaned up.");
}


        public void LogOut()
        {
            Debug.Log("[CandidApiManager] Initiating logout process.");
            AsyncLocalStorage.DeleteData("authTokenId");
            AsyncLocalStorage.DeleteData("userPrivateKey");
            AsyncLocalStorage.DeleteData("userPublicKey");

            DesInitializeCandidApis();
            Game.Instance.Restart();
            Debug.Log("[CandidApiManager] Logout process completed. Scene reloaded.");
            //NowCanLogin
        }

        private UniTask InitializeCandidApis(IAgent agent, bool asAnon = false)
        {
            Debug.Log($"[CandidApiManager] Initializing Candid APIs. Anonymous: {asAnon}");
            var userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();
            string userAccountIdentity;
            //Check if anon setup is required
            if (asAnon)
            {
                //Build Interfaces
                CanisterLogin = new CanisterLoginApiClient(agent, Principal.FromText("fdaor-cqaaa-aaaao-ai7nq-cai"));
                testnft = new TestnftApiClient(agent, Principal.FromText("etqmj-zyaaa-aaaap-aakaq-cai"));
                testicrc1 = new Testicrc1ApiClient(agent, Principal.FromText("svcoe-6iaaa-aaaam-ab4rq-cai"));
                flux = new FluxApiClient(agent, Principal.FromText("plahz-wyaaa-aaaam-accta-cai"));
                chests = new ChestsApiClient(agent, Principal.FromText("opcce-byaaa-aaaak-qcgda-cai"));
                tournaments = new TournamentsApiClient(agent, Principal.FromText("lqaq5-paaaa-aaaap-qhndq-cai"));
                icptoken = new IcpLedgerApiClient(agent, Principal.FromText("ryjl3-tyaaa-aaaaa-aaaba-cai"));
                boomToken = new BoomTokenApiClient(agent, Principal.FromText("vtrom-gqaaa-aaaaq-aabia-cai"));
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);

            }
            else
            {
                //Build Interfaces
                CanisterLogin = new CanisterLoginApiClient(agent, Principal.FromText("fdaor-cqaaa-aaaao-ai7nq-cai"));
                testnft = new TestnftApiClient(agent, Principal.FromText("etqmj-zyaaa-aaaap-aakaq-cai"));
                testicrc1 = new Testicrc1ApiClient(agent, Principal.FromText("svcoe-6iaaa-aaaam-ab4rq-cai"));
                flux = new FluxApiClient(agent, Principal.FromText("plahz-wyaaa-aaaam-accta-cai"));
                chests = new ChestsApiClient(agent, Principal.FromText("opcce-byaaa-aaaak-qcgda-cai"));
                tournaments = new TournamentsApiClient(agent, Principal.FromText("lqaq5-paaaa-aaaap-qhndq-cai"));
                icptoken = new IcpLedgerApiClient(agent, Principal.FromText("ryjl3-tyaaa-aaaaa-aaaba-cai"));
                boomToken = new BoomTokenApiClient(agent, Principal.FromText("vtrom-gqaaa-aaaaq-aabia-cai"));
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);

            }
            Debug.Log("[CandidApiManager] Candid APIs initialized successfully.");
        }

        private void DesInitializeCandidApis()
        {
            Debug.Log("[CandidApiManager] Deinitializing Candid APIs and resetting login data.");

            CanisterLogin = null;
            testnft = null;
            testicrc1 = null;
            flux = null;
            chests = null;
            icptoken = null;
            boomToken = null;
            tournaments = null;

            //Set Login Data
            loginData = new LoginData(null, null, null, false, DataState.None);
            Debug.Log("[CandidApiManager] Candid APIs deinitialized.");
        }
    }
}
