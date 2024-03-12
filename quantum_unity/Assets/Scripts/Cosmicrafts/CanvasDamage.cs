using System.Collections;
using System.Collections.Generic;
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

    public void SetDamage(float newDamage)
    {
        mainCamera = Camera.main;
        damageText.text = "" + (int)newDamage;
       
        //The UI always look at the camera
        if (mainCamera) { transform.LookAt(mainCamera.transform); }

    }
}
