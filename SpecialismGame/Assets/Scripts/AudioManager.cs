using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sound[] sounds;
    public Sound[] walkSounds;
    public Sound[] dialogue;
    public float idleCountdown;
    public static AudioManager Instance { get; private set; }
    public AudioSource DialogueSource;
    GameManagerStateMachine gameManager;
    float deltaPitch;

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
        idleCountdown = Random.Range(60, 120);
    }

    private void Update()
    {
        if (!gameManager.inMenu && !gameManager.inCutscene)
        {
            idleCountdown -= Time.deltaTime;
            if (idleCountdown<=0)
            {
                idleCountdown = Random.Range(60, 120);
                string idle = "Idle #" + Random.Range(1, 4);
                PlayDialogueAudio(idle);
                SubtitleManager.Instance.PlaySubtitle(idle);
            }
        }
        DialogueManagement();
    }

    private void DialogueManagement()
    {
        if (Time.timeScale == 0)
        {
            DialogueSource.pitch = 0;
        }
        else
        {
            DialogueSource.pitch = 1;
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

    public void RunCutscene(bool play)
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
        if(play)
        {
            PlayDialogueAudio(audio);
        }
        else
        {
            StopDialogueAudio(audio);
        }
    }

    public void PlayDialogueAudio(string name)
    {
        try
        {
            Sound s = Array.Find(dialogue, sound => sound.name == name);
            if (DialogueSource.isPlaying)
            {
                StartCoroutine(RetryDialogue(name));
            }
            else
            {
                DialogueSource.clip = s.clip;
                DialogueSource.Play();
                SubtitleManager.Instance.PlaySubtitle(name);
            }
            
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            Debug.Log(name);
            throw;
        }
    }

    public void StopDialogueAudio(string name)
    {
        if (DialogueSource.isPlaying)
        {
            DialogueSource.Stop();
        }
    }

    IEnumerator RetryDialogue(string audio)
    {
        yield return new WaitForSeconds(1);
        PlayDialogueAudio(audio);
    }
}
