using UnityEngine;
using Unity.Netcode;
using TMPro; // Required for TextMeshProUGUI
using System.Linq; // For .FirstOrDefault() if using complex LINQ queries

public class CharacterCanvasUpdater : NetworkBehaviour
{
    // This is the TextMeshProUGUI component that this script is attached to (on the prefab)
    private TextMeshProUGUI thisCharacterTextMeshPro;

    [Header("Scene Text Identification")]
    [Tooltip("The name of the GameObject that has the TextMeshProUGUI component in your scene whose text you want to copy. " +
             "Ensure this name is unique in your scene, or be more specific with paths if needed.")]
    private string sceneTextObjectName = "Session Code Text"; // Default to the name you provided

    void Awake()
    {
        // Get the reference to the TextMeshProUGUI component on this GameObject
        thisCharacterTextMeshPro = GetComponent<TextMeshProUGUI>();
        if (thisCharacterTextMeshPro == null)
        {
            Debug.LogError("CharacterCanvasUpdater requires a TextMeshProUGUI component on the same GameObject!", this);
            enabled = false; // Disable the script if it's attached to the wrong place
        }
    }

    public override void OnNetworkSpawn()
    {
        // This code runs on all clients and the server when the character spawns.
        // We only want the owner of this character to initiate the text transfer
        // and then send it to everyone via an RPC.
        if (IsOwner)
        {
            string textToCopy = GetTextFromSceneCanvasDynamically();

            if (!string.IsNullOrEmpty(textToCopy))
            {
                UpdateCharacterCanvasTextClientRpc(textToCopy);
            }
            else
            {
                Debug.LogWarning($"Could not find or retrieve text from scene object '{sceneTextObjectName}'.", this);
            }
        }
    }

    // This method dynamically finds the TextMeshProUGUI in the scene
    private string GetTextFromSceneCanvasDynamically()
    {
        // Option 1: Find by GameObject Name (simplest, but needs unique name)
        // This finds active, top-level GameObjects by name. If it's deep inside a Canvas,
        // it won't find it unless the Canvas itself is top-level and this object is a child.
        // If "Session Code Text" is a child of a Canvas named "MainCanvas", you might need more.
        GameObject sceneTextGameObject = GameObject.Find(sceneTextObjectName);

        if (sceneTextGameObject != null)
        {
            TextMeshProUGUI sourceText = sceneTextGameObject.GetComponent<TextMeshProUGUI>();
            if (sourceText == null)
            {
                // If the TextMeshProUGUI component is on a child of the found GameObject,
                // you might need GetComponentInChildren<TextMeshProUGUI>()
                sourceText = sceneTextGameObject.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (sourceText != null)
            {
                return sourceText.text;
            }
            else
            {
                Debug.LogError($"GameObject '{sceneTextObjectName}' found, but no TextMeshProUGUI component (or in children) on it.", this);
            }
        }
        else
        {
            // Option 2: Find ALL TextMeshProUGUI components and filter (more robust if names aren't unique)
            // This can be slower if there are many TMP components.
            /*
            TextMeshProUGUI[] allTextsInScene = FindObjectsOfType<TextMeshProUGUI>();
            TextMeshProUGUI sourceText = allTextsInScene.FirstOrDefault(t => t.gameObject.name == sceneTextObjectName);

            if (sourceText != null)
            {
                return sourceText.text;
            }
            */

            Debug.LogError($"GameObject with name '{sceneTextObjectName}' not found in the scene (or not active/top-level).", this);
        }
        return string.Empty; // Return empty string if not found or text component missing
    }

    [ClientRpc]
    private void UpdateCharacterCanvasTextClientRpc(string newText)
    {
        // This RPC is called on all clients.
        // It updates the TextMeshProUGUI component on each character's canvas.
        if (thisCharacterTextMeshPro != null)
        {
            thisCharacterTextMeshPro.text = newText;
            Debug.Log($"Character's Session Code Text updated to: '{newText}'");
        }
    }
}