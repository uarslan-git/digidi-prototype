using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR; // Required for OpenXR

public class TutorialCanvasManager : MonoBehaviour
{
    public Canvas tutorialCanvas;
    private InputDevice rightController;
    private bool isQuest;

    void Start()
    {
        isQuest = Application.platform == RuntimePlatform.Android;
        tutorialCanvas.enabled = isQuest;

        // Initialize the right controller
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        // Check if the right controller is valid
        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            return;
        }

        // Check for A button press (PrimaryButton in OpenXR)
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            ToggleCanvas();
        }
    }

    public void ToggleCanvas()
    {
        tutorialCanvas.enabled = !tutorialCanvas.enabled;
    }
}
