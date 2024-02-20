using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState() { }
    public override void UpdateState() 
    {
        CheckSwitchStates();
        switch (Ctx.controlScheme)
        {
            case 0:
                DefaultControls();
                break;
            case 1:
                DefaultControls();
                break;
            case 2:
                DefaultControls();
                break;
        }
        Ctx.moveDirection.y = 0;
        Ctx.characterController.Move(Ctx.moveDirection * Ctx.movementSpeed * Time.deltaTime);
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() 
    {
        if (Ctx.horInput==0&&Ctx.vertInput==0)
        {
            SwitchState(Factory.Idle());
        }
        SetSubState(Factory.Interact());
    }
    public override void InitializeSubState() 
    {
        SetSubState(Factory.Interact());
    }

    private void DefaultControls()
    {
        Ctx.moveDirection = Ctx.cameraObject.forward * Ctx.vertInput;
        Ctx.moveDirection = Ctx.moveDirection + Ctx.cameraObject.right * Ctx.horInput;
    }
}
