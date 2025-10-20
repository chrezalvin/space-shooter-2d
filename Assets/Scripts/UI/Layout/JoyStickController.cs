using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // For detecting touch events

public class JoyStickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickControl;

    private float m_movementRange = 50f;
    private Vector2 joystickCenter;           // The center point of the joystick (used to calculate movement)
    public Vector2 inputDirectionNormal { get; private set; }

    private void Start()
    {
        joystickCenter = joystickBackground.position; // Store the joystick center position

        // since the joystick background is circle, movement range can be clamped
        float controllerSize = joystickControl.sizeDelta.x;
        float bgSize = joystickBackground.sizeDelta.x;
        m_movementRange = (bgSize - controllerSize) / 2;
    }

    // Called when the user clicks or touches the joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 touchPosition = eventData.position;
        
        // Calculate the difference between the touch position and the joystick center
        Vector2 direction = touchPosition - joystickCenter;

        // Clamp the direction to the joystick's maximum range (to prevent the control from moving outside the background)
        float distance = Mathf.Clamp(direction.magnitude, 0f, m_movementRange);

        // Set the position of the joystick control within the allowed range
        joystickControl.position = joystickCenter + direction.normalized * distance;

        // Calculate the movement direction (normalized)
        inputDirectionNormal = direction.normalized;
    }

    // Called when the user drags the joystick control
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPosition = eventData.position;
        
        // Calculate the difference between the touch position and the joystick center
        Vector2 direction = touchPosition - joystickCenter;

        // Clamp the direction to the joystick's maximum range (to prevent the control from moving outside the background)
        float distance = Mathf.Clamp(direction.magnitude, 0f, m_movementRange);

        // Set the position of the joystick control within the allowed range
        joystickControl.position = joystickCenter + direction.normalized * distance;

        // Calculate the movement direction (normalized)
        inputDirectionNormal = direction.normalized;
    }

    // Called when the user releases the joystick
    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the joystick control to the center when the user stops touching
        joystickControl.position = joystickCenter;

        // Reset the input direction (so the player stops moving)
        inputDirectionNormal = Vector2.zero;
    }
}
