using UnityEngine;
using Unity.Netcode;

public class PlayerSpecificSpawn : NetworkBehaviour
{
    // Private fields to hold the found spawn point Transforms
    private Transform questRigSpawnPoint;
    private Transform playerSpawnPoint;

    /// <summary>
    /// Called when the NetworkObject is spawned on the network.
    /// This is where we handle player positioning.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        // Important: Only the client who owns this player object (or the server)
        // should execute the positioning logic.
        if (!IsOwner)
        {
            return;
        }


        GameObject questRigSpawnObj = GameObject.Find("QuestRigSpawnPoint");
        if (questRigSpawnObj != null)
        {
            questRigSpawnPoint = questRigSpawnObj.transform;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: 'QuestRigSpawnPoint' GameObject not found in the scene. Please create an empty GameObject named 'QuestRigSpawnPoint' and position it.");
        }

        GameObject playerSpawnObj = GameObject.Find("PlayerSpawnPoint");
        if (playerSpawnObj != null)
        {
            playerSpawnPoint = playerSpawnObj.transform;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: 'PlayerSpawnPoint' GameObject not found in the scene. Please create an empty GameObject named 'PlayerSpawnPoint' and position it.");
        }

        if (gameObject.name.Contains("XR Origin (XR Rig)")) 
        {
            if (questRigSpawnPoint != null)
            {
                MoveToSpawnPoint(questRigSpawnPoint);
            }
            else
            {
                Debug.LogError($"{gameObject.name}: Quest Rig spawn point ('QuestRigSpawnPoint') was not found, cannot move. Check previous warnings.");
            }
        }
        else if (gameObject.name.Contains("V1")) 
        {
            if (playerSpawnPoint != null)
            {
                MoveToSpawnPoint(playerSpawnPoint);
            }
            else
            {
                Debug.LogError($"{gameObject.name}: Player spawn point ('PlayerSpawnPoint') was not found, cannot move. Check previous warnings.");
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Unknown prefab type. This player object will not move to a specific spawn point.");
        }
    }

    private void MoveToSpawnPoint(Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning($"{gameObject.name}: Attempted to move to a null spawn point. This should have been caught by earlier error logs.");
            return;
        }

        // Directly setting the transform position and rotation here.
        // For networked objects, ensure your NetworkTransform component (if you're using one)
        // allows client-side authority if this code runs only on the owner.
        // If the server is ultimately responsible for positioning, you might use an RPC
        // to tell the server to move you. However, for initial spawn positioning,
        // this direct approach is often effective when combined with IsOwner.
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        transform.Rotate(0, 180, 0, Space.Self); // Optional: Rotate 180 degrees on Y-axis
    }

    // No OnDestroy override is typically needed here as this script does not subscribe
    // to global NetworkManager events that need explicit unsubscription.
}
