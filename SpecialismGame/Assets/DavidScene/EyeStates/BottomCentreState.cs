using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCentreState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(0, 0, 30);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ec.TransitionToState(ec.ccs);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ec.TransitionToState(ec.bls);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ec.TransitionToState(ec.brs);
        }
    }
}
