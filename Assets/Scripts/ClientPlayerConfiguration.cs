using UnityEngine;
using Unity.Netcode;

public class ClientPlayerConfiguration : NetworkBehaviour
{
    [SerializeField] private GameObject Configuration;
    [SerializeField] private ActionManager ActionManager;

    private void Awake()
    {
        Configuration.SetActive(false);
        ActionManager.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            Configuration.SetActive(true);
            ActionManager.enabled = true;
        }
    }
}