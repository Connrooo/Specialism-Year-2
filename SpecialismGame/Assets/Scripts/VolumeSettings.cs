using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider mainSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider dialogueSlider;
    [SerializeField] Slider SFXSlider;


    const string MIXER_MAIN = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_DIALOGUE = "DialogueVolume";
    const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        dialogueSlider.onValueChanged.AddListener(SetDialogueVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        mainSlider.value = PlayerPrefs.GetFloat("MAINPref", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MUSICPref", 1f);
        dialogueSlider.value = PlayerPrefs.GetFloat("MIXERPref", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXPref", 1f);
    }

    private void SetMainVolume(float value)
    {
        mixer.SetFloat(MIXER_MAIN, Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("MAINPref", value);
    }
    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MUSICPref", value);
    }
    private void SetDialogueVolume(float value)
    {
        mixer.SetFloat(MIXER_DIALOGUE, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MIXERPref", value);
    }
    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXPref", value);
    }
}
