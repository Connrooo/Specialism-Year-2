using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void EnterState(PlayerController ctrl);
    public abstract void ExitState(PlayerController ctrl);
}
