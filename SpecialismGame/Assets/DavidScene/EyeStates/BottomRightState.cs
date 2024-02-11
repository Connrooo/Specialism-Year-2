using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomRightState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(-30, 0, 30);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ec.TransitionToState(ec.bcs);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ec.TransitionToState(ec.crs);
        }
    }
}
