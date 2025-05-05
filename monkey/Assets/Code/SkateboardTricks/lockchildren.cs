using UnityEngine;

public class LockChildTransform : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
