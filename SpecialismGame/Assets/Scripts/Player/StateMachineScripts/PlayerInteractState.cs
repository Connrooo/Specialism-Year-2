using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    GameObject objectHighlighted;

    public override void EnterState() { }
    public override void UpdateState() 
    {
        if (Ctx.gameManager.canMove && !Ctx.gameManager.paused)
        {
            InteractRaycast();
        }
        DisplayInteractionText();
        Ctx.InteractPromptText.text = ("Press "+ InputManager.GetBindingName(Ctx.inputActionReference.action.name, 0)+ " or "+ InputManager.GetBindingName(Ctx.inputActionReference.action.name, 1)+ Ctx.inputActionDisplayed);
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
    }
    public override void InitializeSubState() { }

    private void InteractRaycast()
    {
        RaycastHit hit;
        Vector3 front = Ctx.cameraObject.TransformDirection(Vector3.forward);
        var mask = Ctx.layerMaskInteract.value;
        Debug.DrawRay(Ctx.cameraObject.position, front, Color.green);
        if (Physics.Raycast(Ctx.cameraObject.position, front, out hit, Ctx.rayLength, mask))
        {
            if (hit.collider.tag == "Interact")
            {
                objectHighlighted = hit.transform.gameObject;
                Ctx.InteractPromptTextAnim.SetBool("OnObject", true);
                if (Ctx.CanInteract())
                {
                    Ctx.pointOfInterest = hit.point;
                    Ctx.currentObject = hit.transform.gameObject;
                    PlayerInteractingWithObject();
                }
                else
                {
                    NotInteractingWithObject();
                }
            }
            else
            {
                Ctx.InteractPromptTextAnim.SetBool("OnObject", false);
                NotInteractingWithObject();
            }
        }
        else
        {
            Ctx.InteractPromptTextAnim.SetBool("OnObject", false);
            NotInteractingWithObject();
        }
    }

    private void PlayerInteractingWithObject()
    {
        if (!Ctx.gameManager.stopInteract)
        {
            if (PlayerStateMachine.controlScheme != 0 && !Ctx.gameManager.paused)
            {
                if (Ctx.interactedCS1)
                {
                    FinishInteraction();
                }
                if (!Ctx.loading&&!Ctx.interactedCS1)
                {
                    Ctx.loading = true;
                    Ctx.loadingCurrent = Transform.Instantiate(Ctx.loadBar, new Vector3(Ctx.pointOfInterest.x, Ctx.pointOfInterest.y, Ctx.pointOfInterest.z), Quaternion.identity);
                }
                if(Ctx.loadingCurrent!=null)
                {
                    Ctx.loadingCurrent.transform.position = Vector3.SmoothDamp(Ctx.loadingCurrent.transform.position, Ctx.pointOfInterest, ref Ctx.loadingReference, Ctx.BarTransitionTime * Time.deltaTime);
                }
                Ctx.interactedCS1 = false;
            }
            else
            {
                FinishInteraction();
            }
        }
    }

    public void FinishInteraction()
    {
        var objectScript = Ctx.currentObject.GetComponent<Interactable>();
        switch (objectScript.interactType)
        {
            case "Clue":
                var clueScript = Ctx.currentObject.GetComponent<ClueScript>();
                Ctx.gameManager.pickedUpObjects.Add(clueScript.pickup);
                AudioManager.Instance.Play("Camera Flash");
                Object.Destroy(Ctx.currentObject);
                break;
            case "Door":
                var doorScript = Ctx.currentObject.GetComponent<DoorScript>();
                Ctx.gameManager.currentRoomNumber = doorScript.roomNumber;
                doorScript.doorAnimator.SetTrigger("open");
                if (Ctx.gameManager.currentRoomNumber != 0)
                {
                    Ctx.gameManager.hasRoomBeenChosen = true;
                }
                break;
            case "Chief":
                if (Ctx.gameManager.inRoom)
                {
                    Ctx.gameManager.roomsSearched.Add(Ctx.gameManager.currentRoomNumber);
                    Ctx.gameManager.finishedInvestigating = true;
                }
                else if (Ctx.gameManager.inDeliberation)
                {
                    Ctx.gameManager.finishedInvestigating = false;
                    Ctx.gameManager.stopInteract = true;
                }
                else
                {
                    Debug.Log("Not in room, haven't written this yet bozo");
                }
                break;
            case "CaseFile":
                var caseNumber = Ctx.currentObject.GetComponent<CaseFile>().suspectRelated;
                Ctx.gameManager.caseFileImages[caseNumber].SetActive(true);
                Ctx.gameManager.canMove = false;
                Ctx.gameManager.stopInteract = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                EventSystem.current.SetSelectedGameObject(Ctx.gameManager.caseFileExits[caseNumber]);
                break;
            case "AccuseButton":
                Ctx.currentObject.GetComponent<AccuseButton>().ActivateButton();
                break;
            case "Accuse":
                Ctx.currentObject.GetComponent<AccuseSuspectScript>().AccuseSuspect();
                break;
        }
    }
    public void DisplayInteractionText()
    {
        if (objectHighlighted!= null)
        {
            var objectScript = objectHighlighted.GetComponent<Interactable>();
            switch (objectScript.interactType)
            {
                case "Clue":
                    Ctx.inputActionDisplayed = " to gather evidence";
                    if (Ctx.pointerCurrent==null)
                    {
                        Transform[] children = objectHighlighted.GetComponentsInChildren<Transform>();
                        foreach(Transform child in children)
                        {
                            if (child.CompareTag("PointerPos"))
                            {
                                Ctx.pointerCurrent = Object.Instantiate(Ctx.pointer, child.transform.position, Ctx.pointer.transform.rotation);
                            }
                        }
                        
                    }
                    break;
                case "Door":
                    Ctx.inputActionDisplayed = " to open the door";
                    break;
                case "Chief":
                    if (Ctx.gameManager.inRoom)
                    {
                        Ctx.inputActionDisplayed = " to go to deliberation";
                    }
                    else if (Ctx.gameManager.inDeliberation)
                    {
                        if (Ctx.gameManager.day == 2)
                        {
                            Ctx.inputActionDisplayed = " to go to the final day";
                        }
                        else
                        {
                            Ctx.inputActionDisplayed = " to go to the next day";
                        }
                    }
                    break;
                case "CaseFile":
                    Ctx.inputActionDisplayed = " to open the case file";
                    break;
                case "AccuseButton":
                    Ctx.inputActionDisplayed = " to start accusing (THIS ACTION CANNOT BE STOPPED)";
                    break;
                case "Accuse":
                    var suspectNumber = objectHighlighted.GetComponent<AccuseSuspectScript>().caseFile.suspectRelated;
                    string suspectName;
                    switch (suspectNumber)
                    {
                        case 0:
                            suspectName = "Chef, Paddington Jenkins";
                            Ctx.inputActionDisplayed = " to accuse the " + suspectName;
                            break;
                        case 1:
                            suspectName = "Wife, Dianne Monclair";
                            Ctx.inputActionDisplayed = " to accuse the " + suspectName;
                            break;
                        case 2:
                            suspectName = "Butler, Jamie Doe";
                            Ctx.inputActionDisplayed = " to accuse the " + suspectName;
                            break;
                    }
                    break;
            }
        }
        else
        {
            if (Ctx.pointerCurrent!=null)
            {
                Debug.Log("Destroyed");
                Object.Destroy(Ctx.pointerCurrent);
            }
        }
    }



    

    private void NotInteractingWithObject()
    {
        Ctx.loading = false;
        Transform.Destroy(Ctx.loadingCurrent);
    }
}
