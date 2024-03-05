using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerDeliberateState : GameManagerBaseState
{
    public GameManagerDeliberateState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    : base(currentContext, gameManagerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        Ctx.inDeliberation= true;
        foreach (CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.gameplayCamera.Priority = 11;
        Ctx.canMove = true;
        OrganiseEvidence();
        DisplayEvidence();
        Ctx.Player.transform.position = new Vector3(0f, 1f, 0f);
        Ctx.currentRoomSummoned = Object.Instantiate(Ctx.summonPointPrefab);
        Ctx.instantiatedRoom = Object.Instantiate(Ctx.deliberationPrefab, Ctx.currentRoomSummoned.transform);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inDeliberation = false;
        Ctx.day++;
        foreach (GameObject itemsOfEvidenceInstance in Ctx.evidenceInstances)
        {
            Object.Destroy(itemsOfEvidenceInstance);
        }
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.finishedInvestigating)
        {
            SwitchState(Factory.Cutscene());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void OrganiseEvidence()
    {
        foreach(CluePickup clue in Ctx.pickedUpObjects)
        {
            switch(clue.suspectRelated)
            {
                case 0:
                    Ctx.evidence0.Add(clue);
                    break;
                case 1:
                    Ctx.evidence1.Add(clue);
                    break;
                case 2:
                    Ctx.evidence2.Add(clue);
                    break;
            }
        }
    }
    
    private void DisplayEvidence()
    {
        int index = 0;
        foreach(GameObject caseFile in Ctx.caseFileImages)
        {
            string ItemsOfEvidence = "ItemsOfEvidence"; 
            GameObject itemsOfEvidence = FindChildWithTag(caseFile, ItemsOfEvidence);
            switch (index)
            {
                case 0:
                    foreach(CluePickup evidence in Ctx.evidence0)
                    {
                        ChangeEvidenceText(itemsOfEvidence, evidence);
                    }
                    break;
                case 1:
                    foreach (CluePickup evidence in Ctx.evidence1)
                    {
                        ChangeEvidenceText(itemsOfEvidence, evidence);
                    }
                    break;
                case 2:
                    foreach (CluePickup evidence in Ctx.evidence2)
                    {
                        ChangeEvidenceText(itemsOfEvidence, evidence);
                    }
                    break;
            }
            index++;
        }
    }

    private void ChangeEvidenceText(GameObject itemsOfEvidence, CluePickup evidence)
    {
        var evidenceUI = Object.Instantiate(Ctx.evidenceUIPrefab, itemsOfEvidence.transform);
        Ctx.evidenceInstances.Add(evidenceUI);
        string textTag = "EvidenceName";
        TMP_Text evidenceUIName = FindChildWithTag(evidenceUI, textTag).GetComponent<TMP_Text>();
        textTag = "EvidenceDescription";
        TMP_Text evidenceUIDescription = FindChildWithTag(evidenceUI, textTag).GetComponent<TMP_Text>();
        evidenceUIName.text = evidence.itemName;
        evidenceUIDescription.text = evidence.itemDescription;
    }

    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;
        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
        return child;
    }
}
