using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera targetCamera; // Zielkamera, deren Startposition, Rotation und Size übernommen werden
    public float moveDuration = 2f; // Dauer der Bewegung in Sekunden
    public bool moveOnStart = true; // Bewegung automatisch bei Spielstart

    private bool isMoving = false;
    private float elapsedTime = 0f; // Verstrichene Zeit
    private Vector3 originalPosition; // Ursprüngliche Position der Main Camera
    private Quaternion originalRotation; // Ursprüngliche Rotation der Main Camera
    private float originalSize; // Ursprüngliche Größe der Main Camera
    private float targetSize; // Zielgröße (Size der Target Camera)

    void Start()
    {
        // Speichere die ursprünglichen Werte der Main Camera
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalSize = GetComponent<Camera>().orthographicSize;

        // Setze die Main Camera auf die Werte der Target Camera
        if (targetCamera != null)
        {
            transform.position = targetCamera.transform.position;
            transform.rotation = targetCamera.transform.rotation;
            GetComponent<Camera>().orthographicSize = targetCamera.orthographicSize;
            targetSize = originalSize; // Zielgröße ist die ursprüngliche Größe

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

    void Update()
    {
        // Bewegung durch Tastendruck starten (optional)
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            StartMove();
        }

        // Bewegung durchführen
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;

            // Berechne den Fortschritt (0 bis 1) mit Ease-In/Out
            float t = elapsedTime / moveDuration;
            t = Mathf.Clamp01(t); // Begrenze den Wert auf [0, 1]
            t = t * t * (3f - 2f * t); // Ease-In/Out (Smoothing)

            // Interpolation der Position, Rotation und Größe
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
}
