using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlyMode : MonoBehaviour
{
    public float mainSpeed = 10f; // Regular speed
    public float camSens = 0.5f; // Mouse sensitivity
    public bool flyMode = false;
    private Vector3 flyStartPosition;
    private Quaternion flyStartRotation;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 initialCameraPosition; // Начальное положение камеры
    public Vector3 minBounds; // Минимальные границы куба
    public Vector3 maxBounds; // Максимальные границы куба
    //private CameraChanger cameraChanger;
    private GameEntryMenu gameEntryMenu;
    private CameraChanger cameraChanger;
    private Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        cameraChanger = FindObjectOfType<CameraChanger>();
        //cameraChanger = FindObjectOfType<CameraChanger>();
        flyStartPosition = transform.position; // Store fly start position
        flyStartRotation = transform.rotation; // Store fly start rotation

        // Определение начальных координат камеры
        initialCameraPosition = transform.position;

        // Расчет минимальных и максимальных границ куба
        minBounds = initialCameraPosition - new Vector3(25f, 9f, 45f);
        maxBounds = initialCameraPosition + new Vector3(20f, 20f, 20f);
    }

    void Update()
    {
        if (gameEntryMenu.isNewGameClicked)
        {
            FlyModeOn();
        }

    }

    private void FlyModeOn()
    {
        // Fly mode activation
        //if (Input.GetKeyDown(KeyCode.Keypad1) && cameraChanger.currentCameraIndex == 0)
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            // transform.rotation = Quaternion.Euler(new Vector3(30.1f, -179f, 0f));
            flyMode = !flyMode;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBackToInitialPos();
            flyMode = false;
        }
        if (flyMode)
        {
            rotationX += Input.GetAxis("Mouse X") * camSens;
            rotationY += Input.GetAxis("Mouse Y") * camSens;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Clamp rotationY to prevent camera flipping

            transform.rotation = Quaternion.Euler(-rotationY, rotationX, 0);

            // Mouse camera angle done.  

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Keyboard commands
            Vector3 p = GetBaseInput();

            p = p * mainSpeed;

            p = p * Time.deltaTime;

            // Новая позиция, которая будет проверена на выход за пределы области
            Vector3 newPosition = transform.position + transform.TransformDirection(p);

            // Проверка на выход за пределы куба
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            // Если новая позиция в пределах куба, перемещаем камеру
            if (IsWithinBounds(newPosition))
            {
                transform.position = newPosition;
            }
        }
        else // Reset position and rotation when not in fly mode
        {
            GoBackToInitialPos();
            rotationX = 0f;
            rotationY = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void GoBackToInitialPos()
    {
        transform.position = flyStartPosition;
        transform.rotation = flyStartRotation;
    }

    // Проверка на то, находится ли позиция в пределах куба
    private bool IsWithinBounds(Vector3 position)
    {
        return position.x >= minBounds.x && position.x <= maxBounds.x &&
               position.y >= minBounds.y && position.y <= maxBounds.y &&
               position.z >= minBounds.z && position.z <= maxBounds.z;
    }

    private Vector3 GetBaseInput()
    { // Returns the basic values, if it's 0 then it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + (transform.forward * mainSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (-transform.forward * mainSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (-transform.right * mainSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (transform.right * mainSpeed * Time.deltaTime);
        }
        return p_Velocity;
    }
}
