using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeValueChange : MonoBehaviour
{
    //Reference to Audio Source component
    private AudioSource audioSrc;

    //Music volume variable that will be modified by dragging slider
    private float musicVolume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        //Assign Audio Source component to control it
        audioSrc = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Setting ovlume option of Audio Source to be equal to musicVolume
        audioSrc.volume = musicVolume;
    }

    //method that is called by slider game obect
    //takes vol value passed by slider
    public void SetVolume(float vol)
    {
        musicVolume = vol;
        
    }
}

