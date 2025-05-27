using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI; // Make sure to include this for Canvas

public class MultiplayerWidgetUIController : MonoBehaviour
{
    [Tooltip("Assign the Canvas GameObject that contains your Multiplayer Widgets.")]
    public Canvas multiplayerWidgetsCanvas;

    void Start()
    {
        // Subscribe to Netcode events that signify a client or server session has started.
        // When these events fire, the DisableUI method will be called.
        NetworkManager.Singleton.OnClientStarted += DisableUI;
        NetworkManager.Singleton.OnServerStarted += DisableUI;
        Debug.Log("MultiplayerWidgetUIController subscribed to Netcode events.");
    }

    void OnDestroy()
    {
        // It's crucial to unsubscribe from events when the GameObject is destroyed
        // to prevent memory leaks and null reference exceptions if NetworkManager.Singleton
        // is destroyed before this script.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted -= DisableUI;
            NetworkManager.Singleton.OnServerStarted -= DisableUI;
            Debug.Log("MultiplayerWidgetUIController unsubscribed from Netcode events.");
        }
    }

    private void DisableUI()
    {
        // Disable the main multiplayer widgets canvas if it's assigned.
        if (multiplayerWidgetsCanvas != null)
        {
            multiplayerWidgetsCanvas.enabled = false;
            Debug.Log("Multiplayer Widgets Canvas disabled after Netcode session started.");
        }
        else
        {
            Debug.LogWarning("Multiplayer Widgets Canvas is not assigned in the Inspector! UI might not be disabled.");
        }

        // Now, look for and disable the XRI Spatial Keyboard(Clone)
        // We use GameObject.Find because it might be instantiated at runtime.
        GameObject xriSpatialKeyboard = GameObject.Find("XRI Spatial Keyboard(Clone)");

        if (xriSpatialKeyboard != null)
        {
            xriSpatialKeyboard.SetActive(false); // Disable the GameObject
            Debug.Log("XRI Spatial Keyboard(Clone) disabled.");
        }
        else
        {
            Debug.LogWarning("XRI Spatial Keyboard(Clone) not found in the scene. It might not be present or its name has changed.");
        }
    }
}
