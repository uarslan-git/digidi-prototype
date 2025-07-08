
using UnityEngine;

#if UNITY_NETCODE
using Unity.Netcode;
#endif
using System.Collections.Generic;
using UnityEngine.XR;


#if UNITY_NETCODE
public class TutorialCanvasController : NetworkBehaviour
#else
public class TutorialCanvasController : MonoBehaviour
#endif
{

    [Tooltip("Assign the tutorial Canvas GameObject here.")]
    public GameObject tutorialCanvas;

    private bool canvasHiddenByUser = false;


#if UNITY_NETCODE
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(false);
            return;
        }

        if (IsXROriginPrefab())
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(true); // Always enable on spawn for XR Origin owner
            canvasHiddenByUser = false;
        }
        else
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(false);
        }
    }
#else
    void Start()
    {
        if (IsXROriginPrefab())
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(true);
        }
        else
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(false);
        }
    }
#endif

    void Update()
    {
        // Check for Meta Quest 3 "A" button (using InputSystem)
        if (canvasHiddenByUser && XRControllerAButtonPressed())
        {
            if (tutorialCanvas != null)
                tutorialCanvas.SetActive(true);
            canvasHiddenByUser = false;
        }
    }

    public void HideCanvas()
    {
        if (tutorialCanvas != null)
            tutorialCanvas.SetActive(false);
        canvasHiddenByUser = true;
    }

    bool XRControllerAButtonPressed()
    {
        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightHandedControllers);

        foreach (var device in rightHandedControllers)
        {
            bool pressed;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out pressed) && pressed)
                return true;
        }
        return false;
    }

    bool IsXROriginPrefab()
    {
        // Check if this object is the XR Origin prefab by name
        return gameObject.name.Contains("XR Origin");
    }
}
