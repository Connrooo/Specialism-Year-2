using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    PlayerStateMachine context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }
    public PlayerBaseState Interact()
    {
        return new PlayerInteractState(context, this);
    }
}
