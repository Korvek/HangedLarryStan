using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetLevelMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevelMusik(float sliderValue)
    {
        mixer.SetFloat("MusikVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevelEffekte(float sliderValue)
    {
        mixer.SetFloat("EffekteVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevelSprache(float sliderValue)
    {
        mixer.SetFloat("SpracheVolume", Mathf.Log10(sliderValue) * 20);
    }
}