using UnityEngine;

public class LockChildTransform : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 localPos = transform.localPosition;

        // Lock X and Z, but allow Y (clamped to non-negative if desired)
        localPos.x = 0f;
        localPos.z = 0f;
        localPos.y = 0f;
        //localPos.y = Mathf.Max(0f, localPos.y); // prevent moving below parent

        transform.localPosition = localPos;
        transform.localRotation = Quaternion.identity; // lock rotation
    }
}
