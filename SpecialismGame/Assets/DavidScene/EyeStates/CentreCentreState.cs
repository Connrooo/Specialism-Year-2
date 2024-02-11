using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentreCentreState : BaseEyeState
{
    public override void EnterState(EyeController ec)
    {
        ec.eye.eulerAngles = new Vector3(0, 0, 0);
    }

    public override void UpdateState(EyeController ec)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ec.TransitionToState(ec.tcs);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ec.TransitionToState(ec.bcs);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ec.TransitionToState(ec.cls);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ec.TransitionToState(ec.crs);
        }
    }
}
