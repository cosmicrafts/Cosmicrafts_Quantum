using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDoorIntro : MonoBehaviour
{
    [SerializeField]
    GameObject doorIntro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideIntroDoor()
    {
        doorIntro.gameObject.SetActive(false);
    }
}
