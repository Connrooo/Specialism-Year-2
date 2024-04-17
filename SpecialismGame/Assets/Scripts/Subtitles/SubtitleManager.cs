using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public Subtitle[] subtitles;
    public static SubtitleManager Instance { get; private set; }
    public TMP_Text subtitleDisplay;
    int amountOfSubsPlaying;
    bool canPlay;
    float lastSubStarted;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlaySubtitle(string name)
    {
        try
        {
            canPlay= true;
            int amountOfSubs=0;
            Subtitle sub = Array.Find(subtitles, subtitleCollection => subtitleCollection.SubtitleName == name);
            amountOfSubsPlaying++;
            foreach (string subtitle in sub.SubtitleText)
            {
                amountOfSubs++;
            }
            lastSubStarted = Time.time;
            StartCoroutine(TimeBetweenSubs(amountOfSubs, 0, sub, Time.time));
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            Debug.Log(name);
            throw;
        }
    }
    public void StopSubtitle()
    {
        canPlay = false;
    }
    IEnumerator TimeBetweenSubs(int amountOfSubs, int index, Subtitle sub, float timeStarted)
    {
        if (allowSubtitle(timeStarted)&& amountOfSubs != index)
        {
            subtitleDisplay.text = sub.SubtitleText[index];
            yield return new WaitForSeconds(sub.timeBetweenSubs[index]);
            if (canPlay)
            {
                index++;
                StartCoroutine(TimeBetweenSubs(amountOfSubs, index, sub, timeStarted));
            }
        }
        else
        {
            if (amountOfSubsPlaying == 1)
            {
                subtitleDisplay.text = "";
            }
            amountOfSubsPlaying--;
        }
    }

    bool allowSubtitle(float timeStarted)
    {
        if (amountOfSubsPlaying>=2)
        {
            if (timeStarted==lastSubStarted)
            {
                return canPlay;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return canPlay;
        }
    }

}
