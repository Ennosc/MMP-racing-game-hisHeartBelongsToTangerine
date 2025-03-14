using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundVolumeSlider;

    public void QuitButton()
    {
        Debug.Log("game closed");
        Application.Quit();
    }

    /*public void SoundVolume()
    {
        float soundVolume = soundVolumeSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(soundVolume)*15);
    }*/

}
