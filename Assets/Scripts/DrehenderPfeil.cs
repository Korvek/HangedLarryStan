using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrehenderPfeil : MonoBehaviour
{
    /// <summary>
    /// Aktive Richtung
    /// </summary>
    public Richtung richtung;
    /// <summary>
    /// Liste möglicher Richtungen
    /// </summary>
    [SerializeField] List<Richtung> richtungsListe;
    //InputActions um auf Tastendrücke zu reagieren
    public InputAction drehenAktion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //Verknüpfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;
    }
    private void OnEnable()
    {
        //Aktiviere InputActions
        drehenAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        drehenAktion.Disable();
    }
    private void OnDestroy()
    {
        //Löse Verknüpfungen
        drehenAktion.performed -= RichtungWechsel;
    }

    private void RichtungWechsel(InputAction.CallbackContext context)
    {
        if (richtungsListe.IndexOf(richtung) + 1 != richtungsListe.Count)
        {
            richtung = richtungsListe[richtungsListe.IndexOf(richtung) + 1];
        }
        else 
        { 
            richtung = richtungsListe [0];
        }
        //Wechselt die Richtung je nach Richtungsvariable
        switch (richtung)
        {
            case Richtung.Oben:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case Richtung.Unten:
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                break;
            case Richtung.Rechts:
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                break;
            case Richtung.Links:
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            default:
                break;
        }
    }
}
