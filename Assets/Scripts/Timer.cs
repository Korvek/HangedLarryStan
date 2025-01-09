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
    public floatObject abgelaufeneZeit;

    public InputAction timerAktivieren;

    private void Awake()
    {
        //TODO: Timer initialisieren ohne reset bei Levelwechsel
        abgelaufeneZeit.value = 0f;
        timerAktivieren.performed += TimerAktivieren;
    }

    private void Update()
    {
        if (abgelaufeneZeit.value < maxZeit)
        {
            abgelaufeneZeit.value += Time.deltaTime;
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();

        }
    }

    private void TimerAktivieren(InputAction.CallbackContext context)
    {
        timerAktivieren.Disable();
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        timerAktivieren.Enable();
    }
    private void OnDisable()
    {
        timerAktivieren.Disable();
    }
    private void OnDestroy()
    {
        timerAktivieren.performed -= TimerAktivieren;
    }
}
