using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    public Animator detectiveAnimator;
    Animator cutsceneAnimator;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        cutsceneAnimator= GetComponent<Animator>();
    }

    private void animationOver()
    {
        gameManager.stopAnimation = true;
    }

    private void playAnimation()
    {
        detectiveAnimator.SetBool("playing", true);
        detectiveAnimator.SetBool("second", false);
    }
    private void secondCutscene()
    {
        detectiveAnimator.SetBool("second", true);
    }
}
