using CanisterPK.CanisterLogin;
using CanisterPK.CanisterMatchMaking;
using CanisterPK.CanisterStats;
using UnityEngine.SceneManagement;

namespace Candid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Cysharp.Threading.Tasks;
    using EdjCase.ICP.Agent.Agents;
    using EdjCase.ICP.Agent.Identities;
    using EdjCase.ICP.Candid.Models;
   
    using UnityEngine;
    //using WebSocketSharp;
  

    using Unity.VisualScripting;

    public class CandidApiManager : MonoBehaviour
    {

        public bool autoLogin = true;
        public static CandidApiManager Instance { get; private set; }
        
        // Canister APIs
        public CanisterLoginApiClient CanisterLogin { get; private set; }
        public CanisterMatchMakingApiClient CanisterMatchMaking { get; private set; }
        public CanisterStatsApiClient CanisterStats { get; private set; }

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
        private LoginData loginData = new LoginData(null, null, null, false, DataState.None);
        
        

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }

        private void Start()
        {
            
            if (PlayerPrefs.HasKey("authTokenId") && autoLogin)
            {
                Debug.Log("Ya tenía login guardado, estoy registrando ApiCandis");
                LoadingPanel.Instance.ActiveLoadingPanel();
                OnLoginCompleted(PlayerPrefs.GetString("authTokenId"));
            }
            else
            {
                Debug.Log("No tengo login guardado");
            }
            
            
        }


        public void StartLogin()
        {
            if (loginData.state == DataState.Ready) { return; }
            

            #if UNITY_WEBGL && !UNITY_EDITOR
            LoginManager.Instance.StartLoginFlowWebGl(OnLoginCompleted);
            return;
            #endif

            LoginManager.Instance.StartLoginFlow(OnLoginCompleted);
        }
        
        public void OnLoginCompleted(string json)
        {
            CreateAgentUsingIdentityJson(json, false).Forget();
            
        }
        public async UniTaskVoid CreateAgentUsingIdentityJson(string json, bool useLocalHost = false)
        {
            await UniTask.SwitchToMainThread();
            try
            {
                var identity = Identity.DeserializeJsonToIdentity(json);

                var httpClient = new UnityHttpClient();

                if (useLocalHost) await InitializeCandidApis(new HttpAgent(identity, new Uri("http://localhost:4943")));
                else await InitializeCandidApis(new HttpAgent(httpClient, identity));

                Debug.Log("Terminé de crear el agente, ahora estoy logueado");
                PlayerPrefs.SetString("authTokenId", json);

                if (Login.Instance != null)
                {
                    Login.Instance.UpdateWindow(loginData);
                }
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void OnLoginRandomAgent()
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            CreateAgentRandom().Forget();
        }
        public async UniTaskVoid CreateAgentRandom()
        {
            await UniTask.SwitchToMainThread();
            
            try
            {
                IAgent CreateAgentWithRandomIdentity(bool useLocalHost = false)
                {
                    IAgent randomAgent = null;

                    var httpClient = new UnityHttpClient();

                    try
                    {
                        if (useLocalHost)
                            randomAgent = new HttpAgent(Ed25519Identity.Generate(), new Uri("http://localhost:4943"));
                        else
                            randomAgent = new HttpAgent(httpClient, Ed25519Identity.Generate());
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }

                    return randomAgent;
                }
                await InitializeCandidApis(CreateAgentWithRandomIdentity(), true);

                Debug.Log("Terminé de crear el agente random, ahora estoy logueado");

                if (Login.Instance != null)
                {
                    Login.Instance.UpdateWindow(loginData);
                }
                
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        
        public void LogOut( )
        {
            PlayerPrefs.DeleteKey("authTokenId");
            DesInitializeCandidApis();
            SceneManager.LoadScene(0);
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
            var userPrincipal = agent.Identity.GetPublicKey().ToPrincipal().ToText();
            string userAccountIdentity;
            //Check if anon setup is required
            if (asAnon)
                
            {
                CanisterLogin =  new CanisterLoginApiClient(agent, Principal.FromText("woimf-oyaaa-aaaan-qegia-cai"));
                CanisterMatchMaking =  new CanisterMatchMakingApiClient(agent, Principal.FromText("vqzll-jiaaa-aaaan-qegba-cai"));
                CanisterStats =  new CanisterStatsApiClient(agent, Principal.FromText("jybso-3iaaa-aaaan-qeima-cai"));
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);
                
            }
            else
            {
                //Build Interfaces
                CanisterLogin =  new CanisterLoginApiClient(agent, Principal.FromText("woimf-oyaaa-aaaan-qegia-cai"));
                CanisterMatchMaking =  new CanisterMatchMakingApiClient(agent, Principal.FromText("vqzll-jiaaa-aaaan-qegba-cai"));
                CanisterStats =  new CanisterStatsApiClient(agent, Principal.FromText("jybso-3iaaa-aaaan-qeima-cai"));
                //Set Login Data
                loginData = new LoginData(agent, userPrincipal, null, asAnon, DataState.Ready);

                
                
            }
            
        }
        
        private void DesInitializeCandidApis()
        {
            CanisterLogin = null;
            CanisterMatchMaking = null;
            CanisterStats = null;
            //Set Login Data
            loginData = new LoginData(null, null, null, false, DataState.None);
        }

       
        






    }
}