using UnityEngine;
using UnityEngine.InputSystem;

public class TiltController : MonoBehaviour
{
    [Header("Platform tilt")]
    [SerializeField] private float maxTiltAngle = 25f;
    [SerializeField] private float tiltSpeed = 8f;
    [SerializeField] private float deadZone = 0.1f;

    private Rigidbody rb;
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        Vector2 input = GetTiltInput();


        float targetX = Mathf.Clamp(input.x * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        float targetZ = Mathf.Clamp(input.y * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

        // store target rotation computed from input; apply in FixedUpdate to keep physics stable
        targetRotation = Quaternion.Euler(targetX, 0f, targetZ);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            // perform rotation smoothing in FixedUpdate using fixedDeltaTime
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, tiltSpeed * Time.fixedDeltaTime));
        }
    }

    private Vector2 GetTiltInput()
    {
        Vector2 input = Vector2.zero;

        if (Accelerometer.current != null)
        {
            Vector3 accel = Accelerometer.current.acceleration.ReadValue();
            input.x = accel.x;
            input.y = accel.y;
        }
        else if (Keyboard.current != null)
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                horizontal -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                horizontal += 1f;
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
                vertical -= 1f;
            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
                vertical += 1f;

            input.x = horizontal;
            input.y = vertical;
        }

        if (Mathf.Abs(input.x) < deadZone)
            input.x = 0f;

        if (Mathf.Abs(input.y) < deadZone)
            input.y = 0f;

        return input;
    }
}
