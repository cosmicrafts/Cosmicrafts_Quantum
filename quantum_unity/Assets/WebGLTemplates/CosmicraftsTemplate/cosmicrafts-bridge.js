/**
 * Cosmicrafts Unity-JavaScript Bridge
 * Handles communication between the Unity WebGL game and the Vue.js frontend
 */

// Global reference to the Unity instance
var unityInstance = null;

// Flag to track if Unity is ready
var isUnityReady = false;

// Queue of callbacks waiting for Unity to be ready
var onUnityReadyCallbacks = [];

/**
 * Called by Unity when the game is initialized and ready
 */
function unityGameReady() {
    console.log('[CosmicraftsBridge] Unity game is ready');
    isUnityReady = true;
    
    // Dispatch event for Vue to listen to
    window.dispatchEvent(new CustomEvent('unity-game-ready'));
    
    // Process any queued callbacks
    while (onUnityReadyCallbacks.length > 0) {
        const callback = onUnityReadyCallbacks.shift();
        callback();
    }
}

/**
 * Sets the Unity instance reference
 * @param {Object} instance - The Unity instance
 */
function setUnityInstance(instance) {
    console.log('[CosmicraftsBridge] Unity instance set');
    unityInstance = instance;
}

/**
 * Register a callback to be executed when Unity is ready
 * @param {Function} callback - The callback function
 */
function onUnityReady(callback) {
    if (isUnityReady && unityInstance) {
        callback();
    } else {
        onUnityReadyCallbacks.push(callback);
    }
}

/**
 * Called by Unity to request login
 */
function RequestLogin() {
    console.log('[CosmicraftsBridge] Unity requested login');
    
    // Dispatch event for Vue to handle
    window.dispatchEvent(new CustomEvent('unity-request-login'));
}

/**
 * Called by Unity when the game is ready to receive messages
 */
function NotifyGameReady() {
    console.log('[CosmicraftsBridge] Unity notified game ready');
    unityGameReady();
}

/**
 * Called by Unity to notify sign out
 */
function SignOut() {
    console.log('[CosmicraftsBridge] Unity requested sign out');
    
    // Dispatch event for Vue to handle
    window.dispatchEvent(new CustomEvent('unity-sign-out'));
}

/**
 * Sends identity to Unity
 * @param {string} identityJson - The identity JSON
 */
function sendIdentityToUnity(identityJson) {
    console.log('[CosmicraftsBridge] Sending identity to Unity');
    
    onUnityReady(() => {
        try {
            // Call the ReceiveIdentityFromJavaScript method on the WebGLBridge GameObject
            unityInstance.SendMessage('WebGLBridge', 'ReceiveIdentityFromJavaScript', identityJson);
            console.log('[CosmicraftsBridge] Identity sent to Unity');
        } catch (error) {
            console.error('[CosmicraftsBridge] Error sending identity to Unity:', error);
        }
    });
}

// Expose functions to window for Vue to access
window.cosmicraftsBridge = {
    setUnityInstance,
    sendIdentityToUnity,
    onUnityReady
};

console.log('[CosmicraftsBridge] JavaScript bridge initialized'); 