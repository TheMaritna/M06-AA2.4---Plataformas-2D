using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Controlers : MonoBehaviour
{
    public enum InputDevice { Keyboard, Controller }
    public InputDevice currentDevice = InputDevice.Keyboard;

    private Gamepad activeGamepad;

    private void Update()
    {
        DetectActiveController();
    }

    private void DetectActiveController()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            currentDevice = InputDevice.Controller;
            activeGamepad = Gamepad.current;
        }
        if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
        {
            currentDevice = InputDevice.Keyboard;
        }
    }
    public void Vibrate(float low, float high, float duration)
    {
        if (currentDevice != InputDevice.Controller) return;
        if (activeGamepad == null) return;

        activeGamepad.SetMotorSpeeds(low, high);
        StartCoroutine(StopVibration(duration));
    }

    private IEnumerator StopVibration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (activeGamepad != null)
            activeGamepad.SetMotorSpeeds(0, 0);
    }
}
