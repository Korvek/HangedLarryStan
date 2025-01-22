using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stanley : MonoBehaviour
{
    /// <summary>
    /// Richtungsvariable. Gibt die Bewegungsrichtung an. 
    /// </summary>
    public Richtung richtung;
    /// <summary>
    /// Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    /// </summary>
    [Range(5f,20f)] public float geschwindigkeit=1f;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    /// <summary>
    /// Event, das ausgelöst wird wenn ein Buchstabe gesammelt wird
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
    public GameObject LochAnimation;
    public GameObject StanleyEintritt;
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

    private void Awake()
    {
        
        //Weise Aktionen den Tasten zu
        drehenAktion = actions.FindActionMap("Player").FindAction("DrehenAktion");
        sammelAktion = actions.FindActionMap("Player").FindAction("SammelAktion");
        stopAktion = actions.FindActionMap("Player").FindAction("StillstandAktion");
        startAktion = actions.FindActionMap("Player").FindAction("StartAktion");

        //Verknüpfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += Abbiegen;
        //Verknüpfe startAktion mit der Bewegung starten Methode
        startAktion.performed += StarteBewegung;
        //Verknüpfe stopAktion mit der Bewegung stoppen Methode
        stopAktion.performed += StoppeBewegung;
        stopAktion.canceled += StarteBewegung;
        //Verknüpfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;

        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Hole Animator Komponente des Objekts
        anim= GetComponent<Animator>();
        //Zielrichtung zum Start ist vorwärts
        zielrichtung = transform.up;
    }

    /// <summary>
    /// Hält die Bewegung des Spielers an
    /// </summary>
    private void StoppeBewegung(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
            anim.SetBool("BoolStop", true);
        }        
        if (context.canceled)
        {
            anim.SetBool("BoolStop", false);
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
                case "PfeilDrehend":
                    richtung = pfeilObjekt.gameObject.GetComponent<Weiche>().richtung;
                    break;
                default:
                    break;
            }
        }
        
    }
    
    /// <summary>
    /// Startet die Bewegung bei Tastendruck
    /// </summary>
    private void StarteBewegung(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            anim.SetTrigger("TriggerWeiter");
            anim.SetBool("BoolStop",false);
        }
        Time.timeScale = 1;
        if (rigidbody2d != null)
            rigidbody2d.constraints = RigidbodyConstraints2D.None;
        startAktion.performed -= StarteBewegung;
    }
    /// <summary>
    /// Das Objekt biegt in Zielrichtung ab
    /// </summary>
    private void Abbiegen(InputAction.CallbackContext context) 
    {
        RichtungWechsel();
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
            drehenAktion.performed -= Abbiegen;
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
    /// <summary>
    /// Reaktiviere Abbiegen nachdem eine Animation abgeschlossen ist
    /// </summary>
    public void AnimationBeendetRechts()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        drehenAktion.performed += Abbiegen;
    }

    /// <summary>
    /// Reaktiviere Abbiegen nachdem eine Animation abgeschlossen ist
    /// </summary>
    public void AnimationBeendetLinks()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        drehenAktion.performed += Abbiegen;
    }
    /// <summary>
    /// Reaktiviere Abbiegen nachdem eine Animation abgeschlossen ist
    /// </summary>
    public void AnimationBeendetWende()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        drehenAktion.performed += Abbiegen;
    }
    public void OnReset()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        drehenAktion.performed -= Abbiegen;
        startAktion.performed -= StarteBewegung;
        stopAktion.performed -= StoppeBewegung;
        sammelAktion.performed -= Sammeln;
        stopAktion.canceled -= StarteBewegung;
        drehenAktion.Disable();
    }
    public void OnKollision()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        drehenAktion.performed -= Abbiegen;
        startAktion.performed -= StarteBewegung;
        stopAktion.performed -= StoppeBewegung;
        sammelAktion.performed -= Sammeln;
        stopAktion.canceled -= StarteBewegung;
        anim.SetTrigger("TriggerCollision");
    }
    public void Loch()
    {
        LochAnimation.SetActive(true);
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<SpriteRenderer>().forceRenderingOff = true;
        
    }
    public void EintrittStart()
    {
        Debug.Log("Q");
        GetComponent<SpriteRenderer>().enabled = false;
        StanleyEintritt.SetActive(true);
    }
    public void EintrittEnde()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        StanleyEintritt.SetActive(false);
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
            pfeilObjekt = collision.gameObject;
            //richtung = collision.gameObject.GetComponent<Weiche>().richtung;
        }
        //Wenn es eine Stoplinie ist
        else if (collision.CompareTag("Stoplinie"))
        {
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
            //Aktiviere Bewegung fortsetzen
            startAktion.performed += StarteBewegung;
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
        else if (collision.CompareTag("PfeilDrehend"))
        {
            //Vergiss den Pfeil
            pfeilObjekt = null;
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //Bei Zusammenstoß mit einer Wand
    //    if (collision.gameObject.CompareTag("Wand"))
    //    {
    //        //Starte das Level neu
    //        kollisionEvent.TriggerEvent();
    //    }
    //}  
    private void OnDestroy()
    {
        //Löse Verknüpfungen
        drehenAktion.performed -= Abbiegen;
        startAktion.performed -= StarteBewegung;
        stopAktion.performed -= StoppeBewegung;
        sammelAktion.performed -= Sammeln;
        stopAktion.canceled -= StarteBewegung;
    }
}
