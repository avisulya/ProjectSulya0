using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 15f, -8f);
    [Tooltip("Smaller = snappier")] [Range(0.01f, 2f)] public float positionSmoothTime = 0.15f;
    [Tooltip("Higher = snappier")] [Range(0.1f, 20f)] public float rotationSmoothSpeed = 8f;
    public bool lookAtTarget = false;

    Vector3 positionVelocity;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref positionVelocity, positionSmoothTime);

        if (lookAtTarget)
        {
            Vector3 dir = target.position - transform.position;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion desiredRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                float t = 1f - Mathf.Exp(-rotationSmoothSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, t);
            }
        }
    }
}
