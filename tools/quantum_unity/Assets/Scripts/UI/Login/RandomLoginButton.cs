using UnityEngine;
using Boom; // Ensure this namespace correctly references where BoomManager is located

public class RandomLoginButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        Debug.Log("Button Clicked");
        BoomManager.Instance.OnLoginRandomAgent();
    }

}
