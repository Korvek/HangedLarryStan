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
    [Tooltip("Sekunden, die als Strafe abgezogen werden.")]
    public int strafZeit;
    [Tooltip("Sekunden, die als Bonus addiert werden.")]
    public int bonusZeit;

    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    public GameEvent zeitAbgelaufenEvent;

    private InputAction startAktion;

    private TextMeshProUGUI timerT;
    private AudioSource tickTack;
    private bool timerAktiviert = false;
    private void Awake()
    {
        //Textkomponente finden
        timerT = GetComponent<TextMeshProUGUI>();
        tickTack = GetComponent<AudioSource>();
        //Abgelaufene Zeit 0 setzen
        abgelaufeneZeit = 0f;
        //Weise Aktionen den Tasten zu
        startAktion = actions.FindActionMap("Player").FindAction("StartAktion");
        //Verknüpfe startAktion mit der TimerAktivieren Methode
        startAktion.performed += TimerAktivieren;
        //timerT.enabled = false;
    }

    private void Update()
    {
        if (timerAktiviert)
        {
            //Setze neuen Timer Text 
            timerT.text = (maxZeit - Mathf.Round(abgelaufeneZeit)).ToString();
            //Wenn die Zeit noch nicht abgelaufen ist
            if (abgelaufeneZeit < maxZeit)
            {
                //Bestimme vergangene Zeit
                abgelaufeneZeit += Time.deltaTime;
                if ((maxZeit - abgelaufeneZeit) <= 30f)
                {
                    tickTack.outputAudioMixerGroup.audioMixer.SetFloat("TimerVolume", Mathf.Log10(Mathf.Lerp(0.00001f, 1f, abgelaufeneZeit / maxZeit)) * 20);
                }
                else
                {
                    tickTack.outputAudioMixerGroup.audioMixer.SetFloat("TimerVolume", Mathf.Log10(0.00001f) * 20);
                }
            }
            else
            {
                zeitAbgelaufenEvent.TriggerEvent();
                
                
            }
        }
    }
    /// <summary>
    /// Starte den Timer neu
    /// </summary>
    private void TimerAktivieren(InputAction.CallbackContext context)
    {        
        abgelaufeneZeit = 0f; //Setze abgelaufene Zeit zurück
        startAktion.performed -= TimerAktivieren; //Deaktiviere start des Timers
        timerT.enabled = true; //Aktiviere Timer Text
        tickTack.Play();
        timerAktiviert = true;
    }
    /// <summary>
    /// Funktion für Zeitstrafen
    /// </summary>
    public void Zeitstrafe()
    {
        abgelaufeneZeit += strafZeit;
    }
    /// <summary>
    /// Funktion für Zeitboni
    /// </summary>
    public void Zeitbonus()
    {
        abgelaufeneZeit -= bonusZeit;
    }

    private void OnEnable()
    {
        //startAktion.Enable();
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
