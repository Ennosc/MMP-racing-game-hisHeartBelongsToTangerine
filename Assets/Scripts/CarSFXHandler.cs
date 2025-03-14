using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{

    [Header("Mixers")]

    public AudioMixer audioMixer;
    [Header("Audio Sources")]
    public AudioSource tiresScreechingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource carHitAudioSource;
    public AudioSource rocketBoostAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    TopDownCarController topDownCarController;

    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("SFXVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        //Handle engine SFX
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();

        //Increase engine vol as car goes brrrr
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        //min lvl so we can still hear it
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);


        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        //to add more variation to the engine, change in pitch
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);

    }

    void UpdateTiresScreechingSFX()
    {
        //Handle tire screeeching SFX
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)//on braking tire screech louder and change in pitch
            {
                tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);

            }
            else//screech sound for drifting
            {
                tiresScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else//fade out tire screech if not screeching
        {
            tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        Debug.Log("Collision detected with: " + collision2D.gameObject.name);

        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;
        if (!carHitAudioSource.isPlaying)
        {
            carHitAudioSource.Play();
        }
    }
    //rocket
    public void PlayRocketBoostSound() 
    {
        if (rocketBoostAudioSource != null)
        {
            rocketBoostAudioSource.Play();
        }
    }
}
