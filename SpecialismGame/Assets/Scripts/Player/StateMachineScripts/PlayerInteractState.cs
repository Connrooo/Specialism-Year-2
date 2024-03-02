using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    break;
                case "Door":
                    var doorScript = Ctx.currentObject.GetComponent<DoorScript>();
                    Ctx.gameManager.currentRoom = doorScript.roomNumber;
                    doorScript.doorAnimator.SetTrigger("Interacted");
                    if(Ctx.gameManager.currentRoom != 0)
                    {
                        Ctx.gameManager.hasRoomBeenChosen= true;
                    }
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
