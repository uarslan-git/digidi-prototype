using UnityEngine;
using Unity.Netcode;

public class OwnerComponentManager : NetworkBehaviour
{
    [SerializeField] private Camera _camera; // This is your camera, assign it in the prefab

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner) { 
            return; 
        }
        _camera.enabled = false;
    }
}