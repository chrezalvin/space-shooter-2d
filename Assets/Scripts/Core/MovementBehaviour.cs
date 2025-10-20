using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBehaviour : MonoBehaviour
{
    /// <summary>
    /// Get movement direction
    /// </summary>
    /// <returns>
    /// normalized version of Vector2
    /// </returns>
    public abstract Vector2 GetMovement();

    /// <summary>
    /// Check if the player is shooting
    /// </summary>
    /// <returns>
    /// true if shooting, false otherwise
    /// </returns>
    public abstract bool IsShooting();
}
