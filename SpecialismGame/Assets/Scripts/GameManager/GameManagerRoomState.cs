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
    public override void EnterState() { }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
    }
    public override void InitializeSubState()
    {
    }
}
