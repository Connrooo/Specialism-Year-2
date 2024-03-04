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
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.inRoom= false;
        Ctx.hasRoomBeenChosen = false;
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
            Object.Destroy(Ctx.currentRoomSummoned);
            SwitchState(Factory.Cutscene());
        }
    }
    public override void InitializeSubState()
    {
    }
}
