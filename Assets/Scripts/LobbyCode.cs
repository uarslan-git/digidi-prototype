using UnityEngine;
using Unity.Netcode;
using TMPro; // Required for TextMeshProUGUI
using System.Linq; // Required for LINQ methods like .FirstOrDefault() if used, though not strictly needed for GameObject.Find

public class LobbyCode : NetworkBehaviour
{
    // This variable will hold the TextMeshProUGUI component attached to this specific GameObject (the prefab instance).
    private TMP_Text textMesh;

    /// <summary>
    /// Called when the network object spawns, both on the server and clients.
    /// This is a suitable place to initialize UI elements that depend on scene objects.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); // Call the base class's OnNetworkSpawn method.

        // Get the TextMeshProUGUI component that is directly attached to this GameObject.
        // This assumes your prefab has a TextMeshProUGUI component on its root or a child.
        // If it's on a child, you might need GetComponentInChildren<TextMeshProUGUI>().
        textMesh = GetComponent<TextMeshProUGUI>();

        // Check if the TextMeshProUGUI component was found on this GameObject.
        if (textMesh == null)
        {
            Debug.LogWarning("LobbyCode: TMP_Text component not found on this GameObject (the prefab instance). " +
                             "Please ensure your prefab has a TextMeshProUGUI component attached.");
            return; // Exit the method if the component is missing to prevent NullReferenceExceptions.
        }

        // --- Find the "Session Code Text" in the scene and update this prefab's text ---

        TMP_Text sessionCodeTextMesh = null; // Initialize to null.

        // Attempt to find the GameObject named "Session Code Text" anywhere in the active scene hierarchy.
        GameObject sessionCodeGO = GameObject.Find("Session Code Text");

        // Check if the GameObject was found.
        if (sessionCodeGO != null)
        {
            // If the GameObject is found, try to get its TextMeshProUGUI component.
            sessionCodeTextMesh = sessionCodeGO.GetComponent<TextMeshProUGUI>();
        }

        // Check if the "Session Code Text" component was successfully found.
        if (sessionCodeTextMesh != null)
        {
            // If found, set the text of this prefab's TextMeshProUGUI to match the session code text.
            textMesh.text = sessionCodeTextMesh.text;
            Debug.Log($"LobbyCode: Prefab's text updated to: '{textMesh.text}' from 'Session Code Text'.");
        }
        else
        {
            // If the "Session Code Text" or its component was not found, log a warning.
            Debug.LogWarning("LobbyCode: GameObject named 'Session Code Text' or its TMP_Text component not found in the scene. " +
                             "Please ensure it exists and is active.");
            // Optionally, set a default or error message to indicate the issue to the user.
            textMesh.text = "Session Code Not Available!";
        }
    }
}
