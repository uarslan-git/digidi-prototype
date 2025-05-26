using Unity.Netcode;
using UnityEngine;
// No need for System.Collections.Generic for this simplified version

public class PlatformPlayerSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    [Tooltip("The default player prefab for Windows/Linux/Mac (e.g., v1 or a standard PC character).")]
    public GameObject defaultPlayerPrefab;

    [Tooltip("The XR player prefab for Meta Quest 3 (typically XR Origin (XR Rig)).")]
    public GameObject xrPlayerPrefab;

    private NetworkManager networkManager;

    void Awake()
    {
        // Get the NetworkManager component from the same GameObject
        networkManager = GetComponent<NetworkManager>();

        if (networkManager == null)
        {
            Debug.LogError("PlatformPlayerSpawner requires a NetworkManager component on the same GameObject!");
            enabled = false; // Disable this script if no NetworkManager is found
            return;
        }

        SetPlayerPrefab();
    }

    private void SetPlayerPrefab()
    {
        bool isMetaQuestOrAndroid = IsRunningOnMetaQuestOrAndroid();
        GameObject selectedPrefab = null;

        if (isMetaQuestOrAndroid)
        {
            selectedPrefab = xrPlayerPrefab;
            Debug.Log("PlatformPlayerSpawner: Selected XR Origin (XR Rig) for Meta Quest 3 / Android.");
        }
        else
        {
            selectedPrefab = defaultPlayerPrefab;
            Debug.Log("PlatformPlayerSpawner: Selected default player prefab (v1) for PC/Linux/Mac.");
        }

        if (selectedPrefab != null)
        {
            // IMPORTANT: Assign the selected prefab to the NetworkManager's PlayerPrefab property
            networkManager.NetworkConfig.PlayerPrefab = selectedPrefab;
            Debug.Log($"PlatformPlayerSpawner: NetworkManager's PlayerPrefab set to: {networkManager.NetworkConfig.PlayerPrefab.name}");

            // The 'PlayerPrefabs' list on NetworkConfig is obsolete/removed.
            // When you set NetworkConfig.PlayerPrefab, Netcode handles the internal registration
            // for the default player. If you need to register *other* prefabs that aren't the
            // primary player prefab, you'd use the NetworkManager's 'NetworkPrefabsList' in the Inspector,
            // or dynamically register them via networkManager.AddNetworkPrefab().
            // For the default player prefab, setting NetworkConfig.PlayerPrefab is sufficient.

        }
        else
        {
            Debug.LogError("PlatformPlayerSpawner: No player prefab assigned by the script! Please ensure both defaultPlayerPrefab and xrPlayerPrefab are set in the inspector.");
        }
    }

    private bool IsRunningOnMetaQuestOrAndroid()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("PlatformPlayerSpawner: Running on an Android device (potentially Meta Quest).");
            return true;
        }
        else if (Application.isEditor)
        {
            Debug.Log("PlatformPlayerSpawner: Running in Unity Editor. Simulating PC/Linux.");
            return false;
        }
        else
        {
            Debug.Log("PlatformPlayerSpawner: Running on PC/Linux/Mac standalone build.");
            return false;
        }
    }
}