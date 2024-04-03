using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuseButton : MonoBehaviour
{

    public List<Interactable> caseFiles;
    GameManagerStateMachine gameManager;
    Interactable interactable;
    Animator animator;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactable = GetComponent<Interactable>();
        animator = GetComponent<Animator>();
        interactable.interactType = "AccuseButton";
        if (gameManager.accusingSuspect)
        {
            ActivateButton();
        }
    }

    public void ActivateButton()
    {
        foreach(Interactable caseFile in caseFiles)
        {
            caseFile.interactType = "Accuse";
            gameManager.accusingSuspect = true;
        }
        animator.SetTrigger("Accused");
        //change lights to be down on the papers
    }
}
