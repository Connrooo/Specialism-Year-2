using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sound[] sounds;
    public Sound[] walkSounds;
    public static AudioManager Instance { get; private set; }
    GameManagerStateMachine gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)

        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.panStereo = s.direction;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
            
        }
        foreach (Sound s in walkSounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.panStereo = s.direction;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;

        }
    }
    private void Start()
    {
        foreach (Sound s in sounds)
        {
            if (s.audioSource.playOnAwake)
            {
                s.audioSource.Play();
            }
        }
    }

    public void Play(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }
    public void PlayWalk()
    {
        try
        {
            Sound s = walkSounds[UnityEngine.Random.Range(0, walkSounds.Length)];
            s.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void Stop(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.audioSource.Stop();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void PlayCutsceneAudio()
    {
        string audio = "";
        if(gameManager.day!=4)
        {
            if (gameManager.finishedInvestigating)
            {
                audio = "Day " + gameManager.day + " Deliberation";

            }
            else
            {
                audio = "Day " + gameManager.day + " Opening";
            }
        }
        else
        {
            switch(gameManager.suspectAccused)
            {
                case 0:
                    audio = "Chef Arrested";
                    break;
                case 1:
                    audio = "Wife Arrested";
                    break;
                case 2:
                    audio = "Butler Arrested";
                    break;
            }
        }
        SubtitleManager.Instance.PlaySubtitle(audio);
    }
}
