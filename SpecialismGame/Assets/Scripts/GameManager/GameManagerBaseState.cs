public abstract class GameManagerBaseState
{
    private bool isRootState = false;
    private GameManagerStateMachine ctx;
    private GameManagerStateFactory factory;
    private GameManagerBaseState currentSuperState;
    private GameManagerBaseState currentSubState;

    protected bool IsRootState { set { isRootState = value; } }
    protected GameManagerStateMachine Ctx { get { return ctx; } }
    protected GameManagerStateFactory Factory { get { return factory; } }

    public GameManagerBaseState(GameManagerStateMachine currentContext, GameManagerStateFactory gameManagerStateFactory)
    {
        ctx = currentContext;
        factory = gameManagerStateFactory;
    }


    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }
    protected void SwitchState(GameManagerBaseState newState)
    {
        ExitState(); // Current state exits state
        newState.EnterState(); // New state enters state
        if (isRootState)
        {
            ctx.CurrentState = newState; // Switches current state of context
        }
        else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState); // Sets the current super states sub state to the new state
        }
    }
    protected void SetSuperState(GameManagerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }
    protected void SetSubState(GameManagerBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}