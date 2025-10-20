using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovementBehaviour : MovementBehaviour
{
    public override Vector2 GetMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        return inputDirection;
    }

    public override bool IsShooting()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
