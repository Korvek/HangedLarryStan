using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;



public class Larry : MonoBehaviour
{  
    //Richtungsvariable. Gibt die Bewegungsrichtung an. [SerializeField] erm�glicht Zugriff �ber Unity Editor
    [SerializeField] Richtung richtung;
    //Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    [Range(0.1f,5f)] public float geschwindigkeit=1f;
    //InputActions um auf Tastendr�cke zu reagieren
    public InputAction drehenAktion;
    public InputAction sammelAktion;

    public String Zielwort="Larry";
    /// <summary>
    /// Zeit die eine Kurve dauert (Testzweck)
    /// </summary>
    [SerializeField]float drehZeit = 100f;

    //Buchstabenliste f�r Zielwort
    public List<TextMeshProUGUI> tmp_Zielwort;

    //Richtungsvektor. Gibt die Bewegungsrichtung an
    //public Vector2 bewegung;
    //K�rperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das ber�hrt wird
    private GameObject sammelObjekt;
    //Richtungspfeil, der ber�hrt wird
    private GameObject pfeilObjekt;
    //Richtung in die sich bewegt werden soll
    Vector2 zielrichtung;


    private void Awake()
    {
        //Verkn�pfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;
        //Verkn�pfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        zielrichtung = transform.up;
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
        //L�se Verkn�pfungen
        drehenAktion.performed -= RichtungWechsel;
        sammelAktion.performed -= Sammeln;
    }

    void FixedUpdate()
    {
        //Geschwindigkeit mit der die Kurve passiert wird
        float drehGeschwindigkeit = 0f;
        
        //Bestimme neue Zielrichtung
        switch(richtung)
        {
            case Richtung.Oben:
                zielrichtung = Vector2.up;
                break;
            case Richtung.Unten:
                zielrichtung = Vector2.down;
                break;
            case Richtung.Rechts:
                zielrichtung = Vector2.right;                
                break;
            case Richtung.Links:
                zielrichtung = Vector2.left;
                break;
        }
        
        if(Vector2.SignedAngle(transform.up,zielrichtung) !=0f)
        {
            if (Vector2.SignedAngle(transform.up, zielrichtung) >= 0f)
            {
                drehGeschwindigkeit = drehZeit * Time.fixedDeltaTime; //Linkskurve
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) <= 0f)
            {
                drehGeschwindigkeit = -drehZeit * Time.fixedDeltaTime; //Rechtskurve
            }
        }
        else
        {
            drehGeschwindigkeit = 0;
        }
        // Rotation
        transform.Rotate(0, 0, drehGeschwindigkeit);
        // Vorw�rtsbewegung basierend auf der aktuellen Richtung
        rigidbody2d.velocity = transform.up * geschwindigkeit;
    }

    /// <summary>
    /// Wechselt die aktuelle Bewegungsrichtung im Uhrzeigersinn
    /// </summary>
    void RichtungWechsel(InputAction.CallbackContext context)
    {

        /* //Bewegungsvariante 90 Grad Drehung
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
        
        }
        */

        //Drehung durch Pfeile
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
            //StartCoroutine(nameof(Kurvefahren));
        }

        //Wechsle Bewegungsrichtung
        //switch (richtung)
        //{
        //    case Richtung.Oben:
        //        bewegung = Vector2.up; //Vektor aufw�rts
        //        break;
        //    case Richtung.Unten:
        //        bewegung = Vector2.down; //Vektor abw�rts
        //        break;
        //    case Richtung.Rechts:
        //        bewegung = Vector2.right; //Vektor rechts
        //        break;
        //    case Richtung.Links:
        //        bewegung = Vector2.left; //Vektor links
        //        break;
        //    default:
        //        break;
        //}
    }
    /// <summary>
    /// Sammelt das Momentan ber�hrte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
       
        if (sammelObjekt != null)
        {
            //TODO Objekt entfernen, Buchstaben pr�fen, Wort anzeigen
            //Wenn der Objektname nur ein Zeichen lang ist und im L�sungswort enthalten ist
            if (sammelObjekt.name.Length == 1 && Zielwort.ToUpper().Contains(sammelObjekt.name))
            {               
                //F�r jeden Buchstaben des Zielworts
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
        }
        //Bei Kontakt mit einem Pfeil
        if(collision.CompareTag("PfeilRechts"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
        }
        else if (collision.CompareTag("PfeilLinks"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
        }
        else if (collision.CompareTag("PfeilOben"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
        }
        else if (collision.CompareTag("PfeilUnten"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
        }
        else if (collision.CompareTag("PfeilDrehend"))
        {
            richtung = collision.GetComponent<DrehenderPfeil>().richtung;
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
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilLinks"))
        {
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilOben"))
        {
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
        else if (collision.CompareTag("PfeilUnten"))
        {
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
    }
}
