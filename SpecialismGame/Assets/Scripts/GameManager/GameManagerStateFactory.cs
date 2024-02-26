using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStateFactory
{
    GameManagerStateMachine context;

    public GameManagerStateFactory(GameManagerStateMachine currentContext)
    {
        context = currentContext;
    }

    public GameManagerBaseState Menu()
    {
        return new GameManagerMenuState(context, this);
    }
    public GameManagerBaseState Cutscene()
    {
        return new GameManagerCutsceneState(context, this);
    }
    public GameManagerBaseState Hallway()
    {
        return new GameManagerHallwayState(context, this);
    }
    public GameManagerBaseState Room()
    {
        return new GameManagerRoomState(context, this);
    }
    public GameManagerBaseState Deliberate()
    {
        return new GameManagerDeliberateState(context, this);
    }
}
