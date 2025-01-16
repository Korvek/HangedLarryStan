
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weiche : MonoBehaviour
{
    /// <summary>
    /// Aktive Richtung
    /// </summary>
    public Richtung richtung;
    /// <summary>
    /// Liste möglicher Richtungen
    /// </summary>
    [SerializeField] List<Richtung> richtungsListe;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    //Input der die Weiche dreht
    private InputAction drehenAktion;
    private void Awake()
    {
        //Weise Aktion den Tasten zu
        drehenAktion = actions.FindActionMap("Weiche").FindAction("DrehenAktion");
        //Wechselt die Richtung je nach Richtungsvariable
        drehenAktion.Enable();
        //Aktiviere Inputs
        Drehen(richtung);
    }
    /// <summary>
    /// Wenn die Weiche die Richtung wechselt
    /// </summary>
    private void RichtungWechsel(InputAction.CallbackContext context)
    {
        //Wechsle zur nächsten Richtung aus der Richtungsliste
        if (richtungsListe.IndexOf(richtung) + 1 != richtungsListe.Count)
        {
            richtung = richtungsListe[richtungsListe.IndexOf(richtung) + 1];
        }
        else //Beginn am Ende der Liste von vorne
        {
            richtung = richtungsListe[0];
        }
        //Wechselt die Richtung je nach Richtungsvariable
        Drehen(richtung);

    }
    /// <summary>
    /// Dreht den Pfeil in die gewählte Richtung
    /// </summary>
    /// <param name="richtung">Gewählte Richtung</param>
    private void Drehen(Richtung richtung)
    {
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
    /// <summary>
    /// Aktiviert das Drehen der Weiche
    /// </summary>
    public void WeicheAktivieren(GameObject gO)
    {
        Debug.Log(gO.Equals(transform.parent.gameObject));
        if (gO.Equals(transform.parent.gameObject))
        {
            Debug.Log("GUT");
            drehenAktion.performed += RichtungWechsel;
        }
    }
    /// <summary>
    /// Deaktiviert das Drehen der Weiche
    /// </summary>
    public void WeicheBlockieren(GameObject gO)
    {
        Debug.Log("Exit Weiche: " + transform.parent.gameObject);
        if (gO.Equals(transform.parent.gameObject))
        {
            drehenAktion.performed -= RichtungWechsel;
        }
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
}