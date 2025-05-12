using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;
    void Start()
    {
        this.transform.position = targetObject.transform.position;
        this.transform.rotation = targetObject.transform.rotation;
        this.transform.Rotate(0, 180, 0, Space.Self);

    }
}