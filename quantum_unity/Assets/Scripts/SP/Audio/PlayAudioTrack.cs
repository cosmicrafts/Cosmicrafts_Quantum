using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTrack : MonoBehaviour
{
    private AudioManager audioManager;
    
     int newTrackID;

    public bool playOnStart;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (playOnStart)
        {
           // audioManager.PlayNewTrack(newTrackID);
        }
    }



    
}