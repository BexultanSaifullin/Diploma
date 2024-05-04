using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public CinemachineVirtualCamera[] VirtualCameras;
    public int currentCameraIndex;
    public bool flyMode = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCameraIndex == 0)
        {
            flyMode = !flyMode;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            flyMode = false;
        }
        if (Input.GetButtonDown("Jump") && !flyMode)
        {
            SwitchCamera();
        }
    }
    private void SwitchCamera()
    {
        VirtualCameras[currentCameraIndex].Priority = 0;
        currentCameraIndex++;
        if (currentCameraIndex >= VirtualCameras.Length)
            currentCameraIndex = 0;
        VirtualCameras[currentCameraIndex].Priority = 1;
    }
}