using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }
    public override void EnterState() { }
    public override void UpdateState() 
    {
        if (Ctx.gameManager.canMove && !Ctx.gameManager.paused)
        {
            InteractRaycast();
        }
        Ctx.InteractPromptText.text = ("Press "+ InputManager.GetBindingName(Ctx.inputActionReference.action.name, 0)+ " or "+ InputManager.GetBindingName(Ctx.inputActionReference.action.name, 1)+" to gather evidence");
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
        if (PlayerStateMachine.controlScheme!=0)
        {
            if (!Ctx.loading)
            {
                Ctx.loading = true;
                Ctx.loadingCurrent = Transform.Instantiate(Ctx.loadBar, new Vector3(Ctx.pointOfInterest.x, Ctx.pointOfInterest.y, Ctx.pointOfInterest.z), Quaternion.identity);
            }
            Ctx.loadingCurrent.transform.position = Vector3.SmoothDamp(Ctx.loadingCurrent.transform.position, Ctx.pointOfInterest, ref Ctx.loadingReference, Ctx.BarTransitionTime * Time.deltaTime);
        }
        else
        {

            
            var objectScript = Ctx.currentObject.GetComponent<Interactable>();
            switch (objectScript.interactType)
            {
                case "Clue":
                    var clueScript = Ctx.currentObject.GetComponent<ClueScript>();
                    Ctx.gameManager.pickedUpObjects.Add(clueScript.pickup);
                    Object.Destroy(Ctx.currentObject);
                    break;
                case "Door":
                    var doorScript = Ctx.currentObject.GetComponent<DoorScript>();
                    Ctx.gameManager.currentRoomNumber = doorScript.roomNumber;
                    //doorScript.doorAnimator.SetTrigger("Interacted");
                    if(Ctx.gameManager.currentRoomNumber != 0)
                    {
                        Ctx.gameManager.hasRoomBeenChosen= true;
                    }
                    break;
                case "Chief":
                    if (Ctx.gameManager.inRoom)
                    {
                        Ctx.gameManager.roomsSearched.Add(Ctx.gameManager.currentRoomNumber);
                        Ctx.gameManager.finishedInvestigating = true;
                    }
                    else if(Ctx.gameManager.inDeliberation)
                    {
                        Ctx.gameManager.finishedInvestigating = false;
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
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    EventSystem.current.SetSelectedGameObject(Ctx.gameManager.caseFileExits[caseNumber]);
                    break;
            }
        }
    }

    private void NotInteractingWithObject()
    {
        Ctx.loading = false;
        Transform.Destroy(Ctx.loadingCurrent);
    }
}
