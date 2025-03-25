using UnityEngine;
using Unity.Netcode;

public class ClientPlayerConfiguration : NetworkBehaviour
{
    [SerializeField] private GameObject Configuration;

    private void Awake()
    {
        Configuration.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            Configuration.SetActive(true);
        }
    }
}