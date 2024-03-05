using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuseButton : MonoBehaviour
{

    public List<Interactable> caseFiles;

    Interactable interactable;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.interactType = "AccuseButton";
    }

    public void ActivateButton()
    {
        foreach(Interactable caseFile in caseFiles)
        {
            caseFile.interactType = "Accuse";
        }
        //change lights to be down on the papers
    }
}
