using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationarySpinnerBehavior : SpinnerBehavior
{
    public override void MoveCharacter(Vector3 moveVelocity, bool jumpEnabled = true)
    {
        HandleInput();
    }
}
