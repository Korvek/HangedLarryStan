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
    /// Liste m�glicher Richtungen
    /// </summary>
    [SerializeField] List<Richtung> richtungsListe;
    //InputActions um auf Tastendr�cke zu reagieren
    public InputAction drehenAktion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //Verkn�pfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;

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
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        drehenAktion.Disable();
    }
    private void OnDestroy()
    {
        //L�se Verkn�pfungen
        drehenAktion.performed -= RichtungWechsel;
    }

    private void RichtungWechsel(InputAction.CallbackContext context)
    {
        //Wechsle zur n�chsten Richtung aus der Richtungsliste
        if (richtungsListe.IndexOf(richtung) + 1 != richtungsListe.Count)
        {
            richtung = richtungsListe[richtungsListe.IndexOf(richtung) + 1];
        }
        else //Beginn am Ende der Liste von vorne
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            //Aktiviere InputActions
            drehenAktion.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Aktiviere InputActions
            drehenAktion.Disable();
        }
    }


}
