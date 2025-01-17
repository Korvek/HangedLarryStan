using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/* TODO Liste
 * Visuelle Bewegung des Spielers bei Schleuder
 *  Animation Rotieren w�hrend Bewegung, Landung in richtiger Richtung?
 * Textgr��e anpassen
 * Sounds verteilen
 *  Sounds abspielen wenn n�tig
 * Automatisch Ranzoomen bei Levelstart
 *  Vielleicht kratzendes oder bl�tterndes Ger�usch
 * Credits
 * Video Levelwechsel
 * IntroScreen
 *  Buttons zur Levelwahl
 *  Button zu Credits
 * R�ckkehr zum IntroScreen
 * Tutorial Texte
 *  Piktogramme
 * Ausrufezeichen bei m�glicher Interaktion
 * VolumeSlider f�r Stimme
 *  Vorlesen unterbrechen
 * 
 * 39 Sekunden 1.
 * 40
 * 80 Sekunden Gesamt
 * Game Over Einblendung? (Szene?)
 * 
 * AUSRUFEZEICHEN RICHTUNG
 * 
 * Maximale L�nge vorwort/nachwort * 
 * Ein Text
 */

public class Stanley : MonoBehaviour
{
    /// <summary>
    /// Richtungsvariable. Gibt die Bewegungsrichtung an. 
    /// </summary>
    [SerializeField] Richtung richtung; //[SerializeField] erm�glicht Zugriff �ber Unity Editor
    /// <summary>
    /// Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    /// </summary>
    [Range(5f,20f)] public float geschwindigkeit=1f;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;

    /// <summary>
    /// Event, das ausgel�st wird wenn ein Buchstabe gesammelt wird
    /// </summary>
    public GameEventChar buchstabeGesammelt;
    /// <summary>
    /// Spielreset Event
    /// </summary>
    public GameEvent resetGame;
    /// <summary>
    /// Kollisions Event
    /// </summary>
    public GameEvent kollisionEvent;

    /// <summary>
    /// Startpunkt f�r SoftReset
    /// </summary>
    private Vector2 start;
    private Richtung startRichtung;
    /// <summary>
    /// Startpunkt f�r Mitte Level
    /// </summary>
    public Vector2 resetMitte;

    //K�rperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das ber�hrt wird
    private GameObject sammelObjekt;
    //Richtungspfeil, der ber�hrt wird
    private GameObject pfeilObjekt;
    //Richtung in die sich bewegt werden soll
    private Vector2 zielrichtung;
    //Animationskomponente
    private Animator anim;
    //Erstes Wort gel�st
    private bool wortGel�st=false;
    //Aktionen die auf Inputs reagieren
    private InputAction drehenAktion;
    private InputAction sammelAktion;
    private InputAction startAktion;
    private InputAction stopAktion;

    ////TEST Aktionen die Abbiegen
    //private InputAction linksAktion;
    //private InputAction rechtsAktion;

    private void Awake()
    {
        start= transform.position;
        startRichtung=richtung;
        //Weise Aktionen den Tasten zu
        drehenAktion = actions.FindActionMap("Player").FindAction("DrehenAktion");
        sammelAktion = actions.FindActionMap("Player").FindAction("SammelAktion");
        stopAktion = actions.FindActionMap("Player").FindAction("StillstandAktion");
        startAktion = actions.FindActionMap("Menu").FindAction("StartGameAktion");

        ////TEST
        //linksAktion = actions.FindActionMap("Player").FindAction("LinksAktion");
        //rechtsAktion = actions.FindActionMap("Player").FindAction("RechtsAktion");

        //Verkn�pfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += Abbiegen;
        //Verkn�pfe startAktion mit der Bewegung starten Methode
        startAktion.performed += StarteBewegung;
        //Verkn�pfe stopAktion mit der Bewegung stoppen Methode
        stopAktion.performed += StoppeBewegung;
        stopAktion.canceled += StarteBewegung;
        //Verkn�pfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;

        ////TEST
        //linksAktion.performed += linksAbbiegen;
        //rechtsAktion.performed += rechtsAbbiegen;
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Hole Animator Komponente des Objekts
        anim= GetComponent<Animator>();
        //Zielrichtung zum Start ist vorw�rts
        zielrichtung = transform.up;
    }

    ////TEST
    //private void RechtsAbbiegen(InputAction.CallbackContext context)
    //{
    //    rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
    //    anim.SetTrigger("TriggerRechtskurve"); //Rechtskurve
    //}

    //private void LinksAbbiegen(InputAction.CallbackContext context)
    //{
    //    rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
    //    anim.SetTrigger("TriggerLinkskurve"); //Linkskurve
    //}

    /// <summary>
    /// H�lt die Bewegung des Spielers an
    /// </summary>
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
        // Vorw�rtsbewegung basierend auf der aktuellen Richtung
        rigidbody2d.velocity = transform.up * geschwindigkeit;
    }
    /// <summary>
    /// Setzt die Richtung in die Abgebogen werden soll
    /// </summary>
    public void RichtungWechsel()
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
    }
    /// <summary>
    /// Das Objekt biegt in Zielrichtung ab
    /// </summary>
    private void Abbiegen(InputAction.CallbackContext context) 
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
            drehenAktion.Disable();
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
            if (Vector2.Angle(transform.up, zielrichtung) == 180)
            {
                anim.SetTrigger("TriggerWenden"); //Wenden
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) <= 0f)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RechtsKurve"))
                {
                    anim.SetTrigger("TriggerRechtskurve"); //Rechtskurve
                }
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) >= 0f)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("LinksKurve"))
                {
                    anim.SetTrigger("TriggerLinkskurve"); //Linkskurve
                }
            }
        }
    }
    public void AnimationBeendet()
    {
        drehenAktion.Enable();
    }
    /// <summary>
    /// Sammelt das momentan ber�hrte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
        if (sammelObjekt != null)
        {
            //Wenn der Objektname nur ein Zeichen lang ist und im L�sungswort enthalten ist
            if (sammelObjekt.name.Length == 1)
            {
                //L�se ein Buchstabe gesammelt Event aus
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
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        startAktion.performed -= StarteBewegung;
    }
    /// <summary>
    /// Setzt den Spieler an eine neue Position und gibt ihm eine neue Richtung
    /// </summary>
    /// <param name="newPos">Neue Position</param>
    /// <param name="newRichtung">Neue Richtung</param>
    public void Sprung(Vector3 newPos, Richtung newRichtung)
    {
        //Bewege den Spieler
        rigidbody2d.MovePosition(newPos);
        //Setze neue Richtung
        richtung = newRichtung;
        switch (richtung)
        {
            case Richtung.Oben:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Richtung.Unten:
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case Richtung.Rechts:
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case Richtung.Links:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
        }
    }
    public void WortGel�st()
    {
        wortGel�st = true;
    }
    public void SoftReset(Vector3 newPos, Richtung newRichtung)
    {
        if (!wortGel�st)
        {
            rigidbody2d.MovePosition(start);
            richtung = startRichtung;
            switch (richtung)
            {
                case Richtung.Oben:
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case Richtung.Unten:
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Richtung.Rechts:
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case Richtung.Links:
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
            }
        }
        else if(wortGel�st)
        {
            transform.position = newPos;
            richtung = newRichtung;
            switch (richtung)
            {
                case Richtung.Oben:
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case Richtung.Unten:
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Richtung.Rechts:
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case Richtung.Links:
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
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
        else if(collision.CompareTag("PfeilRechts"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            RichtungWechsel();
        }
        else if (collision.CompareTag("PfeilLinks"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            RichtungWechsel();
        }
        else if (collision.CompareTag("PfeilOben"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            RichtungWechsel();
        }
        else if (collision.CompareTag("PfeilUnten"))
        {
            //Speichere das Objekt
            pfeilObjekt = collision.gameObject;
            RichtungWechsel();
        }
        //Wenn es ein drehender Pfeil ist
        else if (collision.CompareTag("PfeilDrehend"))
        {
            richtung = collision.gameObject.GetComponent<Weiche>().richtung;
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
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //Bei Zusammensto� mit einer Wand
    //    if (collision.gameObject.CompareTag("Wand"))
    //    {
    //        //Starte das Level neu
    //        kollisionEvent.TriggerEvent();
    //    }
    //}
    private void OnEnable()
    {
        //Aktiviere InputActions
        drehenAktion.Enable();
        sammelAktion.Enable();
        stopAktion.Enable();
        startAktion.Enable();

        ////TEST
        //linksAktion.Enable();
        //rechtsAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        drehenAktion.Disable();
        sammelAktion.Disable();
        stopAktion.Disable();
        startAktion.Disable();

        ////TEST
        //linksAktion.Disable();
        //rechtsAktion.Disable();
    }    private void OnDestroy()
    {
        //L�se Verkn�pfungen
        drehenAktion.performed -= Abbiegen;
        startAktion.performed -= StarteBewegung;
        stopAktion.performed -= StoppeBewegung;
        sammelAktion.performed -= Sammeln;

        ////TEST
        //linksAktion.performed -= linksAbbiegen;
        //rechtsAktion.performed -= rechtsAbbiegen;
    }
}
