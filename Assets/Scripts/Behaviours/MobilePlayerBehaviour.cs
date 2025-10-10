using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobilePlayerBehaviour : PlayerBehaviour
{
    public JoyStickController joyStickController;
    public EventTrigger shootBtn;

    public override void Init(Camera camera, Ship ship)
    {
        if (shootBtn != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { m_shipBehaviour.TryShoot(); });
            shootBtn.triggers.Add(entry);
        }

        base.Init(camera, ship);
    }

    protected override Vector2 GetMovementInput()
    {
        Vector2 movement = joyStickController.GetInputDirection();

        return movement.normalized;
    }


}
