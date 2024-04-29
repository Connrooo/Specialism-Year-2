using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayWalkControls : MonoBehaviour
{
    TMP_Text walkText;
    PlayerStateMachine playerManager;
    GameManagerStateMachine gameManager;
    public Animator animator;

    private void Start()
    {
        walkText = GetComponent<TMP_Text>();
        playerManager = FindObjectOfType<PlayerStateMachine>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        TextControl();
        AnimControl();
    }

    private void TextControl()
    {
        if (gameManager.justStarted)
        {
            if (gameManager.inHallway)
            {
                string text = playerManager.WalkText();
                walkText.text = text;
            }
            else
            {
                walkText.text = "";
            }
        }
    }

    private void AnimControl()
    {
        animator.SetBool("onScreen", gameManager.justStarted);
    }
}
