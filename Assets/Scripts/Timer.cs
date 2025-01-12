using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// Zeit, die der Spieler maximal haben soll in Sekunden
    /// </summary>
    public int maxZeit;
    /// <summary>
    /// Abgelaufene Zeit des Spielers in Sekunden
    /// </summary>
    public float abgelaufeneZeit;

    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    public InputAction startAktion;

    private void Awake()
    {
        abgelaufeneZeit = 0f;
        startAktion = actions.FindActionMap("Menu").FindAction("StartGameAktion");
        startAktion.performed += TimerAktivieren;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (abgelaufeneZeit < maxZeit)
        {
            abgelaufeneZeit += Time.deltaTime;
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();

        }
    }
    private void TimerAktivieren(InputAction.CallbackContext context)
    {
        Time.timeScale = 1;
        startAktion.Disable();
    }

    private void OnEnable()
    {
        startAktion.Enable();
    }
    private void OnDisable()
    {
        startAktion.Disable();
    }
    private void OnDestroy()
    {
        startAktion.performed -= TimerAktivieren;
    }
}
