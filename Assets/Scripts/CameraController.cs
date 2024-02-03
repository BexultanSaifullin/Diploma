using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxTiltAngle = 80.0f;
    public float rotationSpeed = 2.0f;
    public float MaxY = 50.0f;
    public float MinY = 0.0f;

    void Update()
    {
        // Change height based on WASD input
        float vertical = Input.GetAxis("Vertical");
        float newY = Mathf.Clamp(transform.position.y + vertical * moveSpeed * Time.deltaTime, MinY, MaxY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Tilt camera downwards based on height
        float tiltAngle = Mathf.Clamp(transform.position.y* rotationSpeed, 0, maxTiltAngle);
        transform.localEulerAngles = new Vector3(tiltAngle, 0, 0);

    }
}