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
        Ctx.InteractPromptText.text = (Ctx.interactTextState+ Ctx.inputActionDisplayed);
    }
    public override void ExitState() 
    {
    }
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
                AudioManager.Instance.PlayDialogueAudio(clueScript.pickup.itemName);
                Ctx.gameManager.evidenceInRoomCollected++;
                if (Ctx.gameManager.evidenceInRoomCollected == 3)
                {
                    AudioManager.Instance.PlayDialogueAudio("Final Evidence #" + Ctx.gameManager.day);
                }
                Object.Destroy(Ctx.currentObject);
                break;
            case "Door":
                var doorScript = Ctx.currentObject.GetComponent<DoorScript>();
                doorScript.doorAnimator.SetTrigger("open");
                    break;
            case "Chief":
                var chiefTemp = GameObject.FindGameObjectWithTag("ChiefAlt").GetComponent<DoorScript>();
                chiefTemp.doorAnimator.SetTrigger("open");
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
                string audio = "Day " + Ctx.gameManager.day + " Accusation";
                AudioManager.Instance.PlayDialogueAudio(audio);
                break;
            case "Accuse":
                Ctx.currentObject.GetComponent<AccuseSuspectScript>().AccuseSuspect();
                string suspect = "";
                switch(Ctx.gameManager.suspectAccused)
                {
                    case 0:
                        suspect = "Chef";
                        break;
                    case 1:
                        suspect = "Wife";
                        break;
                    case 2:
                        suspect = "Butler";
                        break;
                }
                audio = suspect + " Arrested";
                AudioManager.Instance.PlayDialogueAudio(audio);
                break;
            case "MagGlass":
                Ctx.gameManager.magGlassActive= true;
                GameObject[] evidence = GameObject.FindGameObjectsWithTag("PointerPos");
                GameObject magGlassTimer =  Object.Instantiate(Ctx.gameManager.magGlassTimer,Ctx.gameManager.currentRoomSummoned.transform);
                for (int i = 0; i < evidence.Length; i++)
                {
                    GameObject pointerObject = Object.Instantiate(Ctx.pointer, evidence[i].transform.position, Ctx.pointer.transform.rotation);
                    magGlassTimer.GetComponent<MagGlassScript>().pointers.Add(pointerObject);
                }
                AudioManager.Instance.PlayDialogueAudio("Magnifying Glass");
                Object.Destroy(Ctx.currentObject);
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
                    Ctx.inputActionDisplayed = " to gather the " + objectHighlighted.GetComponent<ClueScript>().pickup.itemName + " as evidence";
                    if (Ctx.pointerCurrent==null)
                    {
                        Transform[] children = objectHighlighted.GetComponentsInChildren<Transform>();
                        foreach(Transform child in children)
                        {
                            if (child.CompareTag("PointerPos"))
                            {
                                if(!Ctx.gameManager.magGlassActive)
                                {
                                    Ctx.pointerCurrent = Object.Instantiate(Ctx.pointer, child.transform.position, Ctx.pointer.transform.rotation);
                                }
                                Ctx.wiggleAnimator = child.GetComponentInParent<Animator>();
                                Ctx.wiggleAnimator.SetBool("wiggle", true);
                            }
                        }
                    }
                    break;
                case "Door":

                    switch (objectHighlighted.GetComponent<DoorScript>().roomNumber)
                    {
                        case 0:
                            Ctx.inputActionDisplayed = " to open the door";
                            break;
                        case 1:
                            Ctx.inputActionDisplayed = " to open the door to the living room";
                            break;
                        case 2:
                            Ctx.inputActionDisplayed = " to open the door to the bathroom";
                            break;
                        case 3:
                            Ctx.inputActionDisplayed = " to open the door to the kitchen";
                            break;
                        case 4:
                            Ctx.inputActionDisplayed = " to open the door to the bedroom";
                            break;
                        case 5:
                            Ctx.inputActionDisplayed = " to open the door to the study room";
                            break;
                        case 6:
                            Ctx.inputActionDisplayed = " to open the door to the dining room";
                            break;
                    }
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
                case "MagGlass":
                    Ctx.inputActionDisplayed = " to use magnifying glass";
                    break;
            }
        }
        else
        {
            if (Ctx.pointerCurrent!=null)
            {
                Object.Destroy(Ctx.pointerCurrent);
            }
            if (Ctx.wiggleAnimator != null)
            {
                Ctx.wiggleAnimator.SetBool("wiggle", false);
                Ctx.wiggleAnimator = null;
            }
        }
    }



    

    private void NotInteractingWithObject()
    {
        Ctx.loading = false;
        Transform.Destroy(Ctx.loadingCurrent);
    }
}
