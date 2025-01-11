using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    public Camera targetCamera; // Zielkamera, deren Startposition, Rotation und Size �bernommen werden
    public float moveDuration = 2f; // Dauer der Bewegung in Sekunden
    public bool moveOnStart = true; // Bewegung automatisch bei Spielstart
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;

    private bool isMoving = false;
    private float elapsedTime = 0f; // Verstrichene Zeit
    private Vector3 originalPosition; // Urspr�ngliche Position der Main Camera
    private Quaternion originalRotation; // Urspr�ngliche Rotation der Main Camera
    private float originalSize; // Urspr�ngliche Gr��e der Main Camera
    private float targetSize; // Zielgr��e (Size der Target Camera)
    private InputAction cameraMoveAktion;

    void Awake()
    {
        //InputAktion zuweisen
        cameraMoveAktion = actions.FindActionMap("Menu").FindAction("CameraMoveAktion");
        cameraMoveAktion.performed += startMoveCamera;

        // Speichere die urspr�nglichen Werte der Main Camera
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalSize = GetComponent<Camera>().orthographicSize;

        // Setze die Main Camera auf die Werte der Target Camera
        if (targetCamera != null)
        {
            transform.position = targetCamera.transform.position;
            transform.rotation = targetCamera.transform.rotation;
            GetComponent<Camera>().orthographicSize = targetCamera.orthographicSize;
            targetSize = originalSize; // Zielgr��e ist die urspr�ngliche Gr��e

            if (moveOnStart)
            {
                StartMove();
            }
        }
        else
        {
            Debug.LogWarning("Target Camera is not assigned!");
        }
    }

    private void startMoveCamera(InputAction.CallbackContext context)
    {
        isMoving = true;
        elapsedTime = 0f;
        cameraMoveAktion.performed -= startMoveCamera;
    }

    void Update()
    {
        // Bewegung durch Tastendruck starten (optional)
        //if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        //{
        //    StartMove();
        //}

        // Bewegung durchf�hren
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;

            // Berechne den Fortschritt (0 bis 1) mit Ease-In/Out
            float t = elapsedTime / moveDuration;
            t = Mathf.Clamp01(t); // Begrenze den Wert auf [0, 1]
            t = t * t * (3f - 2f * t); // Ease-In/Out (Smoothing)

            // Interpolation der Position, Rotation und Gr��e
            transform.position = Vector3.Lerp(targetCamera.transform.position, originalPosition, t);
            transform.rotation = Quaternion.Lerp(targetCamera.transform.rotation, originalRotation, t);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, targetSize, t);

            // Bewegung beenden, wenn Ziel erreicht
            if (t >= 1f)
            {
                isMoving = false;
            }
        }
    }

    // Bewegung starten
    private void StartMove()
    {
        isMoving = true;
        elapsedTime = 0f;
    }

    private void OnEnable()
    {
        //Aktiviere InputActions
        cameraMoveAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        cameraMoveAktion.Disable();
    }
    private void OnDestroy()
    {
        //L�se Verkn�pfungen
        cameraMoveAktion.performed -= startMoveCamera;
    }
}
