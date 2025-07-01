using UnityEngine;

public class WatchXROrigin : MonoBehaviour
{
    public string xrOriginName = "XR Origin"; // Change if your XR Origin GameObject has a different name
    private Transform xrOriginTransform;

    void Update()
    {
        // If we haven't found the XR Origin yet, keep searching
        if (xrOriginTransform == null)
        {
            GameObject xrOriginObj = GameObject.Find(xrOriginName);
            if (xrOriginObj != null)
                xrOriginTransform = xrOriginObj.transform;
        }

        // If found, look at it
        if (xrOriginTransform != null)
        {
            transform.LookAt(xrOriginTransform.position);
            // Optional: lock to Y axis only
            // Vector3 lookPos = xrOriginTransform.position - transform.position;
            // lookPos.y = 0;
            // transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
