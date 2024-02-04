using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxTiltAngle = 80.0f;
    public float rotationSpeed = 2.0f;
    public float MaxY = 50.0f;
    public float MinY = 0.0f;
    public CinemachineVirtualCamera VirtualCameras;
    private float targetFieldOfView = 60;
    private float targetFieldOfViewMin = 10;
    private float targetFieldOfViewMax = 60;

    //float zSpeed = 5.0f;
    //public float maxZ = 20.0f;
    //public float minZ = 0.0f;

    void Update()
    {
        // Change height based on WASD input
        if (VirtualCameras.Priority == 1)
        {
            float vertical = Input.GetAxis("Vertical");
            float newY = Mathf.Clamp(transform.position.y + vertical * moveSpeed * Time.deltaTime, MinY, MaxY);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            HandleCameraZoom();

            //float mousescrollwheel = input.getaxis("mouse scrollwheel");
            //float newzcoordinate = transform.position.z + mousescrollwheel * zspeed;
            //newzcoordinate = mathf.clamp(newzcoordinate, minz, maxz);
            //transform.position = new vector3(transform.position.x, transform.position.y, newzcoordinate);
        }

        // Tilt camera downwards based on height
        float tiltAngle = Mathf.Clamp(transform.position.y* rotationSpeed, 0, maxTiltAngle);
        transform.localEulerAngles = new Vector3(tiltAngle, 0, 0);

    }
    private void HandleCameraZoom()
    {
        float fieldOfViewIncreaseAmount = 5f;
        if(Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= fieldOfViewIncreaseAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += fieldOfViewIncreaseAmount;
        }
        targetFieldOfView = Mathf.Clamp(targetFieldOfView, targetFieldOfViewMin, targetFieldOfViewMax);


        float zoomSpeed = 5f;
        Mathf.Lerp(VirtualCameras.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        VirtualCameras.m_Lens.FieldOfView = targetFieldOfView;
    }
}