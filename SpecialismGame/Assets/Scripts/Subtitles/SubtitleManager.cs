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
    bool canPlay;

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
            int index = 0;
            float waittime = 1;
            float totalWaittime = 0;
            Subtitle sub = Array.Find(subtitles, subtitleCollection => subtitleCollection.SubtitleName == name);
            foreach(string subtitle in sub.SubtitleText)
            {
                waittime = sub.timeBetweenSubs[index];
                StartCoroutine(TimeBetweenSubs(totalWaittime, waittime, index, subtitle));
                index++;
                totalWaittime += waittime;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void StopSubtitle()
    {
        canPlay = false;
    }

    IEnumerator TimeBetweenSubs(float totalWaittime,float waittime, int subNumber, string subtitle)
    {
        if (canPlay)
        {
            yield return new WaitForSeconds(totalWaittime);
            if (canPlay)
            {
                subtitleDisplay.text = subtitle;
                yield return new WaitForSeconds(waittime);
                if (subtitleDisplay.text == subtitle)
                {
                    subtitleDisplay.text = "";
                }
            }
        }
    }
}
