using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreLeftState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(30, 0, 0);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ec.TransitionToState(ec.tls);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ec.TransitionToState(ec.bls);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ec.TransitionToState(ec.ccs);
        }
    }
}
