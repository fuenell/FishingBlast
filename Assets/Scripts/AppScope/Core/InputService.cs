using UnityEngine;
using UnityEngine.InputSystem;

namespace AppScope.Core
{
    public class InputService
    {
        public bool IsPressScreen(out Vector2 screenPosition)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                return true;
            }
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                screenPosition = Mouse.current.position.ReadValue();
                return true;
            }

            screenPosition = Vector2.zero;
            return false;
        }

        public bool IsPressEscape()
        {
            if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
            {
                return true;
            }
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                return true;
            }

            return false;
        }
    }
}
