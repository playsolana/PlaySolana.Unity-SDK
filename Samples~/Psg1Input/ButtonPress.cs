using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour {
    private Image buttonImage;

    void Start() {
        buttonImage = GetComponent<Image>();
        buttonImage.color = Color.white;
        buttonImage.pixelsPerUnitMultiplier = 0.13f;
    }

    public void PressButton(InputAction.CallbackContext context) {
        if (context.performed) {
            buttonImage.color = Color.red;
            buttonImage.pixelsPerUnitMultiplier = 0.10f;
        } else if (context.canceled) {
            buttonImage.color = Color.white;
            buttonImage.pixelsPerUnitMultiplier = 0.13f;
        }
    }
}
