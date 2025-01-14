using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Steuerung überdenken ( QE rechts/links abbiegen; Auslöser bei Pfeil (kein innerer Collider);
//Q/E biegt immer ab, Sammeln automatisch
//Q Abbiegetaste: Biegt auf Pfeil ab
//E Aktionstaste: Sammelt Buchstabe / Stellt Weiche

/* TODO Liste
 * 1. Steuerung überdenken
 * 2. Zweites Zielwort
 * 3. Kollision mit Wegrand/Verlassen der Route
 * 3. Beulen/Leben + Soft Reset
 * 4. Sprungfedern drehen den Spieler
 * 4.1 Visuelle Bewegung des Spielers
 * 4.1.1 Animation
 * 5. Aufräumen und Kommentare
 * 6. Volume Slider
 * 7. Sounds verteilen
 * 7.1 Sounds abspielen wenn nötig
 * 8. Credits
 * 9. Video Levelwechsel
 */

public class Stanley : MonoBehaviour
{
    /// <summary>
    /// Richtungsvariable. Gibt die Bewegungsrichtung an. 
    /// </summary>
    [SerializeField] Richtung richtung; //[SerializeField] ermöglicht Zugriff über Unity Editor
    /// <summary>
    /// Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    /// </summary>
    [Range(0f,15f)] public float geschwindigkeit=1f;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;

    /// <summary>
    /// Drehung mithilfe von Pfeilen oder fixe 90°
    /// </summary>
    //public bool pfeilsteuerungON = true;

    /// <summary>
    /// Event, das ausgelöst wird wenn ein Buchstabe gesammelt wird
    /// </summary>
    public GameEventChar buchstabeGesammelt;
    public GameEvent resetGame;
    

    //Körperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das berührt wird
    private GameObject sammelObjekt;
    //Richtungspfeil, der berührt wird
    private GameObject pfeilObjekt;
    //Richtung in die sich bewegt werden soll
    private Vector2 zielrichtung;
    //Animationskomponente
    private Animator anim;
    //Aktionen die auf Inputs reagieren
    private InputAction drehenAktion;
    private InputAction sammelAktion;
    private InputAction startAktion;
    private InputAction stopAktion;

    //TEST Aktionen die Abbiegen
    private InputAction linksAktion;
    private InputAction rechtsAktion;

    private void Awake()
    {
        //Weise Aktionen den Tasten zu
        drehenAktion = actions.FindActionMap("Player").FindAction("DrehenAktion");
        sammelAktion = actions.FindActionMap("Player").FindAction("SammelAktion");
        stopAktion = actions.FindActionMap("Player").FindAction("StillstandAktion");
        startAktion = actions.FindActionMap("Menu").FindAction("StartGameAktion");

        //TEST
        linksAktion = actions.FindActionMap("Player").FindAction("LinksAktion");
        rechtsAktion = actions.FindActionMap("Player").FindAction("RechtsAktion");

        //Verknüpfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;
        //Verknüpfe startAktion mit der Bewegung starten Methode
        startAktion.performed += StarteBewegung;
        //Verknüpfe stopAktion mit der Bewegung stoppen Methode
        stopAktion.performed += StoppeBewegung;
        stopAktion.canceled += StarteBewegung;
        //Verknüpfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;

        //TEST
        linksAktion.performed += linksAbbiegen;
        rechtsAktion.performed += rechtsAbbiegen;
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Hole Animator Komponente des Objekts
        anim= GetComponent<Animator>();
        //Zielrichtung zum Start ist vorwärts
        zielrichtung = transform.up;
    }

    private void rechtsAbbiegen(InputAction.CallbackContext context)
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
        anim.SetTrigger("TriggerRechtskurve"); //Rechtskurve
    }

    private void linksAbbiegen(InputAction.CallbackContext context)
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
        anim.SetTrigger("TriggerLinkskurve"); //Linkskurve
    }

    private void StoppeBewegung(InputAction.CallbackContext context)
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
        if (context.canceled)
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.None;
        }
    }

    void FixedUpdate()
    {        
        // Vorwärtsbewegung basierend auf der aktuellen Richtung
        rigidbody2d.velocity = transform.up * geschwindigkeit;
    }
    /// <summary>
    /// Setzt die Richtung in die Abgebogen werden soll
    /// </summary>
    public void RichtungWechsel(InputAction.CallbackContext context)
    {
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
        Abbiegen();
    }
    /// <summary>
    /// Das Objekt biegt in Zielrichtung ab
    /// </summary>
    private void Abbiegen() 
    {
        //Bestimme neue Zielrichtung
        switch (richtung)
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
        //Wenn abgebogen werden soll
        if (Vector2.SignedAngle(transform.up, zielrichtung) != 0f)
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
            if (Vector2.Angle(transform.up, zielrichtung) == 180) 
            {
                anim.SetTrigger("TriggerWenden"); //Wenden
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) <= 0f)
            {
                anim.SetTrigger("TriggerRechtskurve"); //Rechtskurve
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) >= 0f)
            {
                anim.SetTrigger("TriggerLinkskurve"); //Linkskurve
            }
        }
    }
    /// <summary>
    /// Sammelt das momentan berührte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
        if (sammelObjekt != null)
        {
            //Wenn der Objektname nur ein Zeichen lang ist und im Lösungswort enthalten ist
            if (sammelObjekt.name.Length == 1)
            {
                //Löse ein Buchstabe gesammelt Event aus
                buchstabeGesammelt.TriggerEvent(sammelObjekt.name[0]);
                //Deaktiviere den Buchstaben
                sammelObjekt.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Startet die Bewegung bei Tastendruck
    /// </summary>
    private void StarteBewegung(InputAction.CallbackContext context)
    {
        Debug.Log("GO");
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        startAktion.performed -= StarteBewegung;
    }

    public void Sprung(Vector3 newPos)
    {
        rigidbody2d.MovePosition(newPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Bei Betreten eines sammelbaren Objekts
        if (collision.CompareTag("Sammelbar"))
        {
            //Speichere das Objekt
            sammelObjekt = collision.gameObject;
        }
        if (collision.CompareTag("PfeilAbbiegen"))
        {
            Abbiegen();
        }
        //Bei Kontakt mit einem Pfeil
        else if(collision.CompareTag("PfeilRechts"))
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
        //Wenn es ein drehender Pfeil ist
        else if (collision.CompareTag("PfeilDrehend"))
        {
            richtung = collision.transform.parent.gameObject.GetComponent<Weiche>().richtung;
            Abbiegen();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Bei Zusammenstoß mit einer Wand
        if (collision.gameObject.CompareTag("Wand"))
        {
            //Starte das Level neu
            resetGame.TriggerEvent();
        }
    }
    private void OnEnable()
    {
        //Aktiviere InputActions
        drehenAktion.Enable();
        sammelAktion.Enable();
        stopAktion.Enable();
        startAktion.Enable();

        //TEST
        linksAktion.Enable();
        rechtsAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        drehenAktion.Disable();
        sammelAktion.Disable();
        stopAktion.Disable();
        startAktion.Disable();

        //TEST
        linksAktion.Disable();
        rechtsAktion.Disable();
    }    private void OnDestroy()
    {
        //Löse Verknüpfungen
        drehenAktion.performed -= RichtungWechsel;
        startAktion.performed -= StarteBewegung;
        stopAktion.performed -= StoppeBewegung;
        sammelAktion.performed -= Sammeln;

        //TEST
        linksAktion.performed -= linksAbbiegen;
        rechtsAktion.performed -= rechtsAbbiegen;
    }
}
