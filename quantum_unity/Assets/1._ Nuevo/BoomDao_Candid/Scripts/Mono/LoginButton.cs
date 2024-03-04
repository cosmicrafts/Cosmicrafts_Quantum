using UnityEngine;
using UnityEngine.UI;
using Boom;

[RequireComponent(typeof(Button))]
public class LoginButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (!button) Debug.LogError("LoginButton script requires a Button component");
        
        button.interactable = true;
        button.onClick.AddListener(OnLoginButtonClick);
    }

    private void OnLoginButtonClick()
    {
        // Call the new public method in BoomManager to perform login
        BoomManager.Instance.PerformLogin();
    }
}
