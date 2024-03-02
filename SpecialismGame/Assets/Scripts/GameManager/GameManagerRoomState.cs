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

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.hasRoomBeenChosen = false;
    }
    public override void CheckSwitchStates()
    {
        if (!Ctx.playingGame)
        {
            Ctx.canMove = false;
            SwitchState(Factory.Menu());
        }
    }
    public override void InitializeSubState()
    {
    }
}
