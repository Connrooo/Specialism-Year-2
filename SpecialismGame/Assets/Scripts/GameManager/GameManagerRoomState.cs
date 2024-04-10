using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerRoomState : GameManagerBaseState
{
    public GameManagerRoomState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    : base(currentContext, gameManagerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() 
    {
        foreach (CinemachineVirtualCamera camera in Ctx.Cameras)
        {
            camera.Priority = 10;
        }
        Ctx.gameplayCamera.Priority = 11;
        Ctx.inRoom= true;
        Ctx.currentRoomSummoned = Object.Instantiate(Ctx.summonPointPrefab);
        Ctx.instantiatedRoom = Object.Instantiate(Ctx.rooms[Ctx.currentRoomNumber-1],Ctx.currentRoomSummoned.transform);
        Ctx.roomDisplayValue = 1;
        Ctx.resetRotation = true;
        Ctx.Player.GetComponent<CharacterController>().enabled = false;
        Ctx.Player.transform.position = Ctx.roomPositions[Ctx.currentRoomNumber - 1];
        Ctx.Player.GetComponent<CharacterController>().enabled = true;
        Ctx.loadGame = false;
        DestroyCollectedEvidence();
    }
    public override void UpdateState()
    {
        if (!Ctx.canMove)
        {
            Ctx.canMove = true;
        }
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.hasRoomBeenChosen = false;
        Ctx.inRoom = false;
        Object.Destroy(Ctx.currentRoomSummoned);
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
            Ctx.fadingScript.overrideFade = true;
        }
        else if (Ctx.finishedInvestigating)
        {
            SwitchState(Factory.Cutscene());
        }
    }
    public override void InitializeSubState()
    {
    }

    private void DestroyCollectedEvidence()
    {
        List<GameObject> objectsInWorld = new();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Interact"))
        {
            if ((obj.GetComponent("ClueScript") as ClueScript) != null)
            {
                foreach (CluePickup collectedClue in Ctx.pickedUpObjects)
                {
                    if (obj.GetComponent<ClueScript>().pickup.itemName == collectedClue.itemName)
                    {
                        Object.Destroy(obj);
                    }
                }
            }
        }
    }

}
