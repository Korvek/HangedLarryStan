using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;



public class Larry : MonoBehaviour
{   
    //Richtungsenum. Ordnet Zahlen Namen zu
    enum Richtung
    {
        Oben,
        Rechts,
        Unten,
        Links
    }
    //Richtungsvariable. Gibt die Bewegungsrichtung an. [SerializeField] ermöglicht Zugriff über Unity Editor
    [SerializeField] Richtung richtung;
    //Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    [Range(0.01f,0.2f)] public float geschwindigkeit=0.1f;

    //InputActions um auf Tastendrücke zu reagieren
    public InputAction drehenAktion;
    public InputAction sammelAktion;

    //Richtungsvektor. Gibt die Bewegungsrichtung an
    private Vector2 bewegung;
    //Körperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das berührt wird
    private GameObject sammelObjekt;
    
    private void Awake()
    {
        //Verknüpfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;
        //Verknüpfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Initialisierung des Richtungsvektors
        bewegung = new Vector2(0f, 0f);
    }

    

    private void OnEnable()
    {
        //Aktiviere InputActions
        drehenAktion.Enable();
        sammelAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        drehenAktion.Disable();
        sammelAktion.Disable();
    }
    private void OnDestroy()
    {
        //Löse Verknüpfung
        drehenAktion.performed -= RichtungWechsel;
        sammelAktion.performed -= Sammeln;
    }

    void FixedUpdate()
    {
        switch (richtung)
        {
            case Richtung.Oben:
                bewegung.y = 1f;    //Vertikal aufwärts
                bewegung.x = 0f;    //Horizontal neutral
                break;
            case Richtung.Unten:
                bewegung.y = -1f;   //Vertikal abwärts
                bewegung.x = 0f;    //Horizontal neutral
                break;
            case Richtung.Rechts:
                bewegung.x = 1f;    //Horizontal aufwärts
                bewegung.y = 0f;    //Vertikal neutral
                break;
            case Richtung.Links:
                bewegung.x = -1f;   //Horizontal abwärts
                bewegung.y = 0f;    //Vertikal neutral
                break;
            default:
                break;
        }
        //Multipliziere Richtung mit Geschwindigkeit um die Bewegungsgeschwindigkeit zu steuern
        bewegung *= geschwindigkeit;

        //Addiere aktuelle Position zur Bewegungsrichtung dazu
        bewegung.x += transform.position.x;
        bewegung.y += transform.position.y;
        //Bewege das Objekt an die Position
        rigidbody2d.MovePosition(bewegung);
    }

    /// <summary>
    /// Wechselt die aktuelle Bewegungsrichtung im Uhrzeigersinn
    /// </summary>
    void RichtungWechsel(InputAction.CallbackContext context)
    {
        //Wenn nicht der letzte Eintrag im Richtungsenum erreicht ist, wechsle einen Eintrag weiter
        if ( ((int)richtung) != 3)
        {
            richtung++;
        }
        //Sollte der letzte Eintrag erreicht sein, springe zum ersten
        else
        {
            richtung = 0;
        }
        Debug.Log(bewegung.ToString());
        //Wechselt die Richtung je nach Richtungsvariable
        switch (richtung)
        {
            case Richtung.Oben:
                transform.Rotate(new Vector3(0f, 0f, -90f));
                break;
            case Richtung.Unten:
                transform.Rotate(new Vector3(0f, 0f, -90f));
                break;
            case Richtung.Rechts:
                transform.Rotate(new Vector3(0f, 0f, -90f));
                break;
            case Richtung.Links:
                transform.Rotate(new Vector3(0f, 0f, -90f));
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Sammelt das Momentan berührte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
        if (sammelObjekt != null)
        {
            //TODO Objekt entfernen, Buchstaben prüfen, Wort anzeigen
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Sammelbar")
        {
            sammelObjekt = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Sammelbar")
        {
            sammelObjekt = null;
        }
    }
}
