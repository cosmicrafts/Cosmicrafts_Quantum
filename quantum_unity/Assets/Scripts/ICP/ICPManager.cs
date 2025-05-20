using System;
using System.Threading.Tasks;
using UnityEngine;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Candid.Models;
using Cysharp.Threading.Tasks;
using System.Text;
using Cosmicrafts.MainCanister;
using EdjCase.ICP.Agent;
using Newtonsoft.Json;

namespace Cosmicrafts.ICP
{
    public class ICPManager : MonoBehaviour
    {
        public static ICPManager Instance { get; private set; }
        
        // Candid API Client for your main canister
        public MainCanisterApiClient MainCanister { get; private set; }
        
        // Your canister ID - replace with your actual canister ID
        private readonly string MAIN_CANISTER_ID = "opcce-byaaa-aaaak-qcgda-cai";
        
        // Login state tracking
        public bool IsLoggedIn { get; private set; }
        public string PrincipalId { get; private set; }
        
        // Events
        public event Action OnICPInitialized;
        public event Action<string> OnLoginSuccessful;
        public event Action OnLoginFailed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[ICPManager] Instance already exists. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[ICPManager] Instance initialized and set to DontDestroyOnLoad.");
        }

        /// <summary>
        /// Creates an agent using a JSON identity received from the web frontend
        /// </summary>
        public async UniTask CreateAgentFromWeb(string identityJson)
        {
            Debug.Log("[ICPManager] Attempting to create agent from web identity JSON");
            
            try
            {
                // Deserialize the identity JSON to our custom format
                var identityData = JsonConvert.DeserializeObject<IdentityData>(identityJson);
                if (identityData == null)
                {
                    throw new Exception("Failed to deserialize identity JSON");
                }

                // Convert the private key from hex string to bytes
                byte[] privateKeyBytes = StringToByteArray(identityData.PrivateKey);
                byte[] publicKeyBytes = StringToByteArray(identityData.PublicKey);
                
                // Create the Ed25519 identity
                var identity = new Ed25519Identity(privateKeyBytes, publicKeyBytes);
                
                // Create an HTTP agent with the identity
                var httpClient = new UnityHttpClient();
                var agent = new HttpAgent(httpClient, identity);
                
                // Initialize the canister client
                await InitializeCanisterClient(agent);
                
                // Login successful
                PrincipalId = agent.Identity.GetPublicKey().ToPrincipal().ToText();
                IsLoggedIn = true;
                
                Debug.Log($"[ICPManager] Login successful. Principal ID: {PrincipalId}");
                OnLoginSuccessful?.Invoke(PrincipalId);
                
                // Cache identity for session persistence if needed
                await AsyncLocalStorage.SaveDataAsync("authIdentity", identityJson);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ICPManager] Error creating agent: {ex.Message}");
                IsLoggedIn = false;
                OnLoginFailed?.Invoke();
            }
        }
        
        /// <summary>
        /// Generates a random identity for testing purposes
        /// </summary>
        public async UniTask CreateRandomAgentForTesting()
        {
            Debug.Log("[ICPManager] Creating random agent for testing");
            
            try
            {
                // Generate a random Ed25519 identity
                var identity = GenerateRandomIdentity();
                
                // Create an HTTP agent with the identity
                var httpClient = new UnityHttpClient();
                var agent = new HttpAgent(httpClient, identity);
                
                // Initialize the canister client
                await InitializeCanisterClient(agent);
                
                // Login successful
                PrincipalId = agent.Identity.GetPublicKey().ToPrincipal().ToText();
                IsLoggedIn = true;
                
                Debug.Log($"[ICPManager] Random login successful. Principal ID: {PrincipalId}");
                OnLoginSuccessful?.Invoke(PrincipalId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ICPManager] Error creating random agent: {ex.Message}");
                IsLoggedIn = false;
                OnLoginFailed?.Invoke();
            }
        }
        
        /// <summary>
        /// Tries to load a saved identity from local storage and use it
        /// </summary>
        public async UniTask TryAutoLogin()
        {
            Debug.Log("[ICPManager] Attempting auto-login from cached identity");
            string savedIdentity = await AsyncLocalStorage.LoadDataAsync("authIdentity");
            
            if (!string.IsNullOrEmpty(savedIdentity))
            {
                Debug.Log("[ICPManager] Found cached identity, attempting to use it");
                await CreateAgentFromWeb(savedIdentity);
            }
            else
            {
                Debug.Log("[ICPManager] No cached identity found");
            }
        }
        
        /// <summary>
        /// Initializes the canister client with the provided agent
        /// </summary>
        private async UniTask InitializeCanisterClient(IAgent agent)
        {
            // Create the main canister client
            MainCanister = new MainCanisterApiClient(agent, Principal.FromText(MAIN_CANISTER_ID));
            
            // Wait for one frame to ensure everything is initialized
            await UniTask.Yield();
            
            OnICPInitialized?.Invoke();
            Debug.Log("[ICPManager] Canister client initialized");
        }
        
        /// <summary>
        /// Logs out the user and clears cached identity
        /// </summary>
        public void Logout()
        {
            Debug.Log("[ICPManager] Logging out");
            
            // Clear saved identity
            AsyncLocalStorage.DeleteData("authIdentity");
            
            // Reset state
            PrincipalId = null;
            IsLoggedIn = false;
            MainCanister = null;
            
            Debug.Log("[ICPManager] Logout complete");
        }
        
        /// <summary>
        /// Generates a random Ed25519 identity for testing
        /// </summary>
        private Ed25519Identity GenerateRandomIdentity()
        {
            // Generate a random seed phrase for deterministic identity generation
            string seedPhrase = Guid.NewGuid().ToString();
            byte[] seedBytes = Encoding.UTF8.GetBytes(seedPhrase);
            
            // Create a hash of the seed phrase to use as the private key
            var sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] privateKeyBytes = sha256.ComputeHash(seedBytes);
            
            // For testing purposes, we'll use the same bytes for both private and public key
            // In a real implementation, you would derive the public key from the private key
            // using proper Ed25519 key derivation
            byte[] publicKeyBytes = new byte[privateKeyBytes.Length];
            Array.Copy(privateKeyBytes, publicKeyBytes, privateKeyBytes.Length);
            
            // Create the Ed25519 identity with both keys
            return new Ed25519Identity(privateKeyBytes, publicKeyBytes);
        }

        private class IdentityData
        {
            public string Type { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        private static byte[] StringToByteArray(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentException("Hex string cannot be null or empty");

            hex = hex.Replace("0x", ""); // Remove 0x prefix if present
            
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string must have an even number of characters");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
} 