using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LoadingPanel : MonoBehaviour
{
    private Animator animator;
    private static LoadingPanel _instance;

    public AudioSource audioSource;
    public AudioClip openingSound;
    public AudioClip closingSound;
    
    public static LoadingPanel Instance {
        get 
        {
            if (_instance == null) { _instance = Instantiate( ResourcesServices.LoadLoadingPanel() ).GetComponent<LoadingPanel>(); }
            return _instance;
        }
    }
    private void Awake() {
        animator = this.GetComponent<Animator>();
        if (!_instance) { _instance = this; DontDestroyOnLoad(gameObject); } //DesactiveLoadingPanel();
        else { Destroy(gameObject); }
    }
    
    public void ActiveLoadingPanel()
    {
        animator.Play("Open_Panel");
    }
    public void DesactiveLoadingPanel()
    {
        animator.Play("Close_Panel");
    }

    public void PlayOpeningSound()
    {
        audioSource.Stop(); audioSource.clip = openingSound;
        audioSource.Play();
    }
    
    public void PlayClosingSound()
    {
        audioSource.Stop(); audioSource.clip = closingSound;
        audioSource.Play();
    }



}

