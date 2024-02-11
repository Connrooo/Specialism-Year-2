using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLeftState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(30, 0, 30);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ec.TransitionToState(ec.bcs);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ec.TransitionToState(ec.cls);
        }
    }
}
