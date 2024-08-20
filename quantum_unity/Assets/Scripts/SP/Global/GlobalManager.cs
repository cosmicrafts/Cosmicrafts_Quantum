namespace CosmicraftsSP {
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GameData GMD;

    private void Awake()
    {
        //Init Data
        GMD = new GameData();
        //Set persistent
        DontDestroyOnLoad(gameObject);
        //Check the build type
        GMD.DebugMode = false;
#if UNITY_EDITOR
        GMD.DebugMode = true;
#endif
        //Check the current plataform
#if UNITY_WEBGL
        GMD.CurrentPlataform = Plataform.Web;
#endif
    }

    private void OnDestroy()
    {
        GMD = null;
    }
}
}