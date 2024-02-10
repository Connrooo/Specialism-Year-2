using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLeftState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(30, 0, -30);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ec.TransitionToState(ec.tcs);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ec.TransitionToState(ec.cls);
        }
    }
}
