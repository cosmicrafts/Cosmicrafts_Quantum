using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SoundComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler 
{
    public AudioClip hoverSound; // Sonido al pasar el mouse por encima del botón
    public AudioClip clickSound; // Sonido al hacer clic en el botón
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        AudioMixer audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        if (audioMixer != null)
        {
            AudioMixerGroup soundsMixerGroup = audioMixer.FindMatchingGroups("Sounds")[0];
            if (soundsMixerGroup != null)
            {
                audioSource.outputAudioMixerGroup = soundsMixerGroup;
            }
            else
            {
                Debug.LogWarning("No se encontró el grupo de sonidos en el AudioMixer.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el AudioMixer.");
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }    
    
}
