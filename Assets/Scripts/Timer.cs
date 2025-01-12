using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public GameEvent endGameEvent;

    private InputAction startAktion;

    private TextMeshProUGUI timerT;

    private void Awake()
    {
        timerT = GetComponent<TextMeshProUGUI>();
        abgelaufeneZeit = 0f;
        startAktion = actions.FindActionMap("Menu").FindAction("StartGameAktion");
        startAktion.performed += TimerAktivieren;
        //Time.timeScale = 0f;
    }

    private void Update()
    {
        timerT.text=abgelaufeneZeit.ToString();
        if (abgelaufeneZeit < maxZeit)
        {
            abgelaufeneZeit += Time.deltaTime;
        }
        else
        {
            endGameEvent.TriggerEvent();
        }
    }
    private void TimerAktivieren(InputAction.CallbackContext context)
    {
        abgelaufeneZeit = 0f;
        //Time.timeScale = 1;
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
