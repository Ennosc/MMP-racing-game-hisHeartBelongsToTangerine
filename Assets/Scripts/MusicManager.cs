using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlayMusic(AudioClip musicClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(musicSource, spawnTransform.position, Quaternion.identity);
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
