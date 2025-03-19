using UnityEngine;
using UnityEngine.InputSystem;

public class Stick : MonoBehaviour {
    public void MoveStick(InputAction.CallbackContext context) {
        if (context.performed) {
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log("Stick Input: " + input);

            // Move the left stick object based on input
            this.transform.localPosition = new Vector3(input.x * 30, input.y * 30, 0);
        }
    }
    
}
