namespace CosmicraftsSP {
    using UnityEngine;
using UnityEngine.UI;
/*
 * This code show the game´s version on a text component
 */
[RequireComponent(typeof(Text))]
public class Version : MonoBehaviour
{
    //Show the current game version (requires a TEXT component)
    void Start()
    {
        GetComponent<Text>().text = GlobalManager.GMD.GetVersion();
    }
}
}