using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public String Zielwort="Larry";

    public List<TextMeshProUGUI> tmp_Zielwort;

    //Richtungsvektor. Gibt die Bewegungsrichtung an
    private Vector2 bewegung;
    //Körperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das berührt wird
    private GameObject sammelObjekt;
    //Richtungspfeil, der berührt wird
    private GameObject pfeilObjekt;
    
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
        //Löse Verknüpfungen
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

        /* Bewegungsvariante 90 Grad Drehung
        Debug.Log("WHAT");
        //Wenn nicht der letzte Eintrag im Richtungsenum erreicht ist, wechsle einen Eintrag weiter
        if ( ((int)richtung) != 3)
        {
            richtung++;
        Debug.Log("WHAT");
        }
        //Sollte der letzte Eintrag erreicht sein, springe zum ersten
        else
        {
            richtung = 0;
        }
        
        }
        Debug.Log("WHAT");
        */
        //Wenn ein Pfeil gespeichert ist
        if (pfeilObjekt != null)
        {
            //Wechsle in die Richtung des Pfeils
            switch (pfeilObjekt.tag)
            {
                case "PfeilRechts":
                    richtung = Richtung.Rechts;
                    break;
                case "PfeilOben":
                    richtung = Richtung.Oben;
                    break;
                case "PfeilLinks":
                    richtung = Richtung.Links;
                    break;
                case "PfeilUnten":
                    richtung = Richtung.Unten;
                    break;
                default:
                    break;
            }
        }
        //Wechselt die Richtung je nach Richtungsvariable
        switch (richtung)
        {
            case Richtung.Oben:
                transform.eulerAngles = new Vector3 (0f, 0f, 0f);
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
    /// Sammelt das Momentan berührte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
       
        if (sammelObjekt != null)
        {
            //TODO Objekt entfernen, Buchstaben prüfen, Wort anzeigen
            //Wenn der Objektname nur ein Zeichen lang ist und im Lösungswort enthalten ist
            if (sammelObjekt.name.Length == 1 && Zielwort.ToUpper().Contains(sammelObjekt.name))
            {               
                //Für jeden Buchstaben des Zielworts
                foreach(TextMeshProUGUI text in tmp_Zielwort)
                {
                    //Aktiviere das passende Textobjekt
                    if (text.name == sammelObjekt.name)
                    {
                        text.gameObject.SetActive(true);
                    }
                }
                //Deaktiviere den Buchstaben
                sammelObjekt.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Bei Betreten eines sammelbaren Objekts
        if (collision.CompareTag("Sammelbar"))
        {
            //Speichere das Objekt
            sammelObjekt = collision.gameObject;
            Debug.Log(sammelObjekt.name);
        }
        //Bei Kontakt mit einem Pfeil
        if(collision.CompareTag("PfeilRechts"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            Debug.Log(pfeilObjekt.name);
        }
        else if (collision.CompareTag("PfeilLinks"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            Debug.Log(pfeilObjekt.name);
        }
        else if (collision.CompareTag("PfeilOben"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            Debug.Log(pfeilObjekt.name);
        }
        else if (collision.CompareTag("PfeilUnten"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            Debug.Log(pfeilObjekt.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Bei Verlassen eines sammelbaren Objekts
        if (collision.CompareTag("Sammelbar"))
        {
            //Vergiss das Objekt
            sammelObjekt = null;
        }
        //Bei Verlassen eines Pfeils
        else if (collision.CompareTag("PfeilRechts"))
        {
            Debug.Log(pfeilObjekt.name + "Weg");
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilLinks"))
        {
            Debug.Log(pfeilObjekt.name + "Weg");
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilOben"))
        {
            Debug.Log(pfeilObjekt.name + "Weg");
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilUnten"))
        {
            Debug.Log(pfeilObjekt.name + "Weg");
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
    }
}
