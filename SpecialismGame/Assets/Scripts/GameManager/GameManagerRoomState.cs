using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Ctx.inRoom= true;
        Ctx.currentRoomSummoned = Object.Instantiate(Ctx.summonPointPrefab);
        Ctx.instantiatedRoom = Object.Instantiate(Ctx.rooms[Ctx.currentRoomNumber-1],Ctx.currentRoomSummoned.transform);
        Ctx.roomDisplayValue = 1;
        DestroyCollectedEvidence();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inRoom= false;
        Ctx.hasRoomBeenChosen = false;
        Object.Destroy(Ctx.currentRoomSummoned);
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
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
