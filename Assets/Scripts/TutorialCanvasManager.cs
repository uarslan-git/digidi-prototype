using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;

public class TutorialCanvasManager : MonoBehaviour
{
    public Canvas tutorialCanvas;
    private InputDevice rightController;
    private bool isQuest;
    private bool previousButtonState = false;

    void Start()
    {
        isQuest = Application.platform == RuntimePlatform.Android && this.gameObject.name == "Cameraa";
        tutorialCanvas.enabled = isQuest; // Start with it disabled

        // Initialize the right controller
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        // Reacquire the controller if it's not valid
        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            return;
        }

        // Read the primary button (A on Quest)
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            // Detect rising edge: button just pressed this frame
            if (isPressed && !previousButtonState)
            {
                ToggleCanvas();
            }

            previousButtonState = isPressed;
        }
    }

    public void ToggleCanvas()
    {
        tutorialCanvas.enabled = !tutorialCanvas.enabled;
    }
}

