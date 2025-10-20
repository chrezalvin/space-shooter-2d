using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidMovementBehaviour : MovementBehaviour
{
    public JoyStickController joyStickController;

    public override Vector2 GetMovement()
    {
        Vector2 dir = joyStickController.inputDirectionNormal;

        return dir;
    }

    public override bool IsShooting()
    {
        return false; // TODO
    }
}
