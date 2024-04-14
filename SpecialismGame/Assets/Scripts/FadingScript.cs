using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class FadingScript : MonoBehaviour
{
    public bool fading;
    public bool overrideFade;
    Animator fadeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        fadeAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fadeAnimator.SetBool("fading", fading);
        fadeAnimator.SetBool("overrideFade",overrideFade);
        if (overrideFade)
        {
            fading = false;
            overrideFade=false;
        }
    }
}
