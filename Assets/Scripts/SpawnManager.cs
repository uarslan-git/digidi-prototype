//using UnityEngine;

//public class PlayerSpawnPoint : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject targetObject;
//    void Start()
//    {
//        this.transform.position = targetObject.transform.position;
//        this.transform.rotation = targetObject.transform.rotation;
//        this.transform.Rotate(0, 180, 0, Space.Self);

//    }
//}

using UnityEngine;
using Unity.Netcode;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform questRigSpawn;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(ulong clientId)
    {
        GameObject playerObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId).gameObject;

        // Check if it's a Quest Rig (by tag)
        if (playerObj.CompareTag("QuestRig"))
        {
            MoveToSpawnPoint(playerObj, questRigSpawn);
        }
        // Default to Player Spawn
        else
        {
            MoveToSpawnPoint(playerObj, playerSpawn);
        }
    }

    private void MoveToSpawnPoint(GameObject obj, Transform spawnPoint)
    {
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = spawnPoint.rotation;
        obj.transform.Rotate(0, 180, 0, Space.Self); // Optional rotation adjustment
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerSpawned;
    }
}