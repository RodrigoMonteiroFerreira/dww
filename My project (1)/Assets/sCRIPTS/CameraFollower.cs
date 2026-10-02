using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -8f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float lookAtSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        //Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookAtSpeed * Time.deltaTime);
    }
}
