using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreen : MonoBehaviour
{

    private Animator anim;

   

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CloseLogin()
    {
        if (anim != null) {
            anim.Play("Close_PanelLogin");
        }
    }
    public void ClosePanelLoading()
    {
        if (anim != null) {
            anim.Play("Close_PanelLoading");
        }
    }

}
