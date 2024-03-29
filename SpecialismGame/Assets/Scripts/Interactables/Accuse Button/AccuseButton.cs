using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuseButton : MonoBehaviour
{

    public List<Interactable> caseFiles;
    GameManagerStateMachine gameManager;
    Interactable interactable;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactable = GetComponent<Interactable>();
        interactable.interactType = "AccuseButton";
    }

    public void ActivateButton()
    {
        foreach(Interactable caseFile in caseFiles)
        {
            caseFile.interactType = "Accuse";
            gameManager.accusingSuspect = true;
        }
        //change lights to be down on the papers
    }
}
