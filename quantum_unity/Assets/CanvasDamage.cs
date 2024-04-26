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

    [Header("Color Settings")]
    public Color noneColor = Color.gray;
    public Color criticColor = Color.yellow;
    public Color evasionColor = Color.magenta;
    
    Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        transform.rotation = Quaternion.Euler(90f, 90f, transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 1.5f);
    }

    public void SetDamage(float newDamage, Quantum.EAttackMode attackMode)
    {
        mainCamera = Camera.main;

        if (newDamage == 0 && attackMode == EAttackMode.Evasion)
        {
            damageText.text = "Miss";
            damageText.color = evasionColor;
        }
        else
        {
            // Set text color based on attack mode
            switch (attackMode)
            {
                case EAttackMode.None:
                    damageText.color = noneColor;
                    break;

                case EAttackMode.Critic:
                    damageText.color = criticColor;
                    break;

                case EAttackMode.Evasion:
                    damageText.color = evasionColor;
                    break;

                default:
                    damageText.color = noneColor;
                    break;
            }

            damageText.text = "" + (int)newDamage;
        }
       
        //The UI always look at the camera
        if (mainCamera)
        {
            transform.LookAt(mainCamera.transform);
        }

    }
}
