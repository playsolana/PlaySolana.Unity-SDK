using PlaySolanaSdk;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android.LowLevel;

public class PSG1InputMirror : MonoBehaviour
{
    private PSG1 virtualDevice;
    private Gamepad source;

    void Start()
    {
        virtualDevice = PSG1.current;
        source = Gamepad.current;

        if (virtualDevice == null)
            Debug.LogWarning("PSG1 virtual device not found.");

        if (source == null)
            Debug.LogWarning("No physical gamepad found.");

        if (source != null && virtualDevice != null) 
            Debug.Log("PSG1 paired with: " + source.displayName);
    }

    void Update()
    {
        if (virtualDevice == null || source == null)
            return;

        PSG1StateController state = default;

        state = state
            .WithButton(AndroidKeyCode.DpadUp, source.dpad.up.isPressed)
            .WithButton(AndroidKeyCode.DpadDown, source.dpad.down.isPressed)
            .WithButton(AndroidKeyCode.DpadLeft, source.dpad.left.isPressed)
            .WithButton(AndroidKeyCode.DpadRight, source.dpad.right.isPressed)
            .WithButton(AndroidKeyCode.ButtonA, source.buttonSouth.isPressed)
            .WithButton(AndroidKeyCode.ButtonB, source.buttonEast.isPressed)
            .WithButton(AndroidKeyCode.ButtonX, source.buttonNorth.isPressed)
            .WithButton(AndroidKeyCode.ButtonY, source.buttonWest.isPressed)
            .WithButton(AndroidKeyCode.ButtonL1, source.leftShoulder.isPressed)
            .WithButton(AndroidKeyCode.ButtonR1, source.rightShoulder.isPressed)
            .WithButton(AndroidKeyCode.ButtonThumbl, source.leftStickButton.isPressed)
            .WithButton(AndroidKeyCode.ButtonThumbr, source.rightStickButton.isPressed)
            .WithButton(AndroidKeyCode.ButtonStart, source.startButton.isPressed)
            .WithButton(AndroidKeyCode.ButtonSelect, source.selectButton.isPressed);
        
        Vector2 ls = source.leftStick.ReadValue();
        Vector2 rs = source.rightStick.ReadValue();
        Vector2 dpad = source.dpad.ReadValue();

        state = state
            .WithAxis(AndroidAxis.X, ls.x)
            .WithAxis(AndroidAxis.Y, -ls.y)
            .WithAxis(AndroidAxis.Z, rs.x)
            .WithAxis(AndroidAxis.Rz, -rs.y)
            .WithAxis(AndroidAxis.HatX, dpad.x)
            .WithAxis(AndroidAxis.HatY, dpad.y)
            .WithAxis(AndroidAxis.Brake, source.leftTrigger.ReadValue())
            .WithAxis(AndroidAxis.Gas, source.rightTrigger.ReadValue());

        InputSystem.QueueStateEvent(virtualDevice, state);
    }
}
