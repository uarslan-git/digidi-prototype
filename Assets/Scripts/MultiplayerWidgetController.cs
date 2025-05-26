using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI; // Make sure to include this for Canvas

public class MultiplayerWidgetUIController : MonoBehaviour
{
    [Tooltip("Assign the Canvas GameObject that contains your Multiplayer Widgets.")]
    public Canvas multiplayerWidgetsCanvas;

    void Start()
    {
        NetworkManager.Singleton.OnClientStarted += DisableUI;
        NetworkManager.Singleton.OnServerStarted += DisableUI;
        Debug.Log("MultiplayerWidgetUIController subscribed to Netcode events.");
    }

    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted -= DisableUI;
            NetworkManager.Singleton.OnServerStarted -= DisableUI;
            Debug.Log("MultiplayerWidgetUIController unsubscribed from Netcode events.");
        }
    }

    private void DisableUI()
    {
        if (multiplayerWidgetsCanvas != null)
        {
            multiplayerWidgetsCanvas.enabled = false;
            Debug.Log("Multiplayer Widgets Canvas disabled after Netcode session started.");
        }
        else
        {
            Debug.LogWarning("Multiplayer Widgets Canvas is not assigned in the Inspector! UI might not be disabled.");
        }
    }
}