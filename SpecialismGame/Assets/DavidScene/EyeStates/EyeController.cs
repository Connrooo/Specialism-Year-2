using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour
{
    public Transform eye;

    public BaseEyeState currentState;
    public readonly CentreCentreState ccs = new CentreCentreState();
    public readonly CentreLeftState cls = new CentreLeftState();
    public readonly CentreRightState crs = new CentreRightState();
    public readonly BottomCentreState bcs= new BottomCentreState();
    public readonly BottomLeftState bls= new BottomLeftState();
    public readonly BottomRightState brs= new BottomRightState();
    public readonly TopCentreState tcs = new TopCentreState();
    public readonly TopLeftState tls = new TopLeftState();
    public readonly TopRightState trs = new TopRightState();
    
    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(ccs);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToState(BaseEyeState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
