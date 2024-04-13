using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Quantum;
using UnityEngine;
using TMPro;
public class CanvasDamage : MonoBehaviour
{
    [SerializeField]
    TMP_Text damageText;
    
    Camera mainCamera;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2.5f);
    }

    public void SetDamage(float newDamage, Quantum.EAttackMode attackMode)
    {
        mainCamera = Camera.main;
        damageText.text = "" + (int)newDamage;

        switch (attackMode)
        {
            case EAttackMode.None:
                damageText.color = Color.gray;
                break;
            
            case EAttackMode.Critic:
                damageText.color = Color.yellow;
                break;
            
            case EAttackMode.Evasion:
                damageText.color = Color.magenta;
                break;
           
            default:
                damageText.color = Color.gray;
                break;
        }
       
        //The UI always look at the camera
        if (mainCamera) { transform.LookAt(mainCamera.transform); }

    }
}
