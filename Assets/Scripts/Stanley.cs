using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


//TODO f�r Pfeile
// 1. Collider in Gang
// 2. Collider l�st Drehung aus


public class Stanley : MonoBehaviour
{
    /// <summary>
    /// Richtungsvariable. Gibt die Bewegungsrichtung an. 
    /// </summary>
    [SerializeField] Richtung richtung; //[SerializeField] erm�glicht Zugriff �ber Unity Editor
    /// <summary>
    /// Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    /// </summary>
    [Range(0f,15f)] public float geschwindigkeit=1f;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    
    /// <summary>
    /// Drehung mithilfe von Pfeilen oder fixe 90�
    /// </summary>
    public bool pfeilsteuerungON = true;
    /// <summary>
    /// Das zu l�sende Wort
    /// </summary>
    public String zielwort="Larry";
    /// <summary>
    /// Speicher f�r gefundene Buchstaben
    /// </summary>
    public String gel�stesWort="";
    /// <summary>
    /// Tempo in dem Abgebogen wird (Testzweck)
    /// </summary>
    [SerializeField]float drehTempo = 100f;

    /// <summary>
    /// Textanzeige f�r das Zielwort
    /// </summary>
    public TextMeshProUGUI ZielwortT;

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
    //Aktionen die auf Inputs reagieren
    private InputAction drehenAktion;
    private InputAction sammelAktion;

    private void Awake()
    {
        //F�llen der L�sungswortanzeige mit '_'
        gel�stesWort = new string('_', zielwort.Length);
        ZielwortT.text=gel�stesWort;

        //Weise Aktionen den Tasten zu
        drehenAktion = actions.FindActionMap("Main").FindAction("DrehenAktion");
        sammelAktion = actions.FindActionMap("Main").FindAction("SammelAktion");

        //Verkn�pfe drehenAktion mit der RichtungsWechsel Methode
        drehenAktion.performed += RichtungWechsel;
        //Verkn�pfe drehenAktion mit dem Starten der Bewegung
        drehenAktion.performed += StarteBewegung;
        //Verkn�pfe sammelAktion mit der Sammeln Methode
        sammelAktion.performed += Sammeln;
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Hole Animator Komponente des Objekts
        anim= GetComponent<Animator>();
        //Zielrichtung zum Start ist vorw�rts
        zielrichtung = transform.up;
    }

    private void Update()
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
    }

    void FixedUpdate()
    {        
        // Vorw�rtsbewegung basierend auf der aktuellen Richtung
        rigidbody2d.velocity = transform.up * geschwindigkeit;
    }

    /// <summary>
    /// Setzt die Richtung in die Abgebogen werden soll
    /// </summary>
    public void RichtungWechsel(InputAction.CallbackContext context)
    {
        //Bewegungsvariante 90 Grad Drehung
        if (!pfeilsteuerungON)
        {            
            //Wenn nicht der letzte Eintrag im Richtungsenum erreicht ist, wechsle einen Eintrag weiter
            if (((int)richtung) != 3)
            {
                richtung++;
            }
            //Sollte der letzte Eintrag erreicht sein, springe zum ersten
            else
            {
                richtung = 0;
            }
        }
        else if (pfeilsteuerungON)
        {
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
            }
        }
    }
    /// <summary>
    /// Das Objekt biegt in Zielrichtung ab
    /// </summary>
    private void Abbiegen() 
    {
        //Wenn abgebogen werden soll
        if (Vector2.SignedAngle(transform.up, zielrichtung) != 0f)
        {
            if (Vector2.SignedAngle(transform.up, zielrichtung) >= 0f)
            {
                anim.SetTrigger("TriggerLinkskurve"); //Linkskurve
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) <= 0f)
            {
                anim.SetTrigger("TriggerRechtskurve"); //Rechtskurve
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) == 180f)
            {
                anim.SetTrigger("TriggerWenden"); //Wenden
            }
        }
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
            if (sammelObjekt.name.Length == 1)
            {
                //Wenn der gesammelte Buchstabe nicht im Zielwort enthalten ist
                if (!zielwort.ToUpper().Contains(sammelObjekt.name))
                {
                    //TODO: Buchstabe nicht im Wort enthalten
                }
                //Wenn der gesammelte Buchstabe im Zielwort enthalten ist
                else
                {
                    //Suche nach dem Buchstaben im Zielwort
                    for (int i = 0; i < zielwort.Length; i++)
                    {
                        if (zielwort.ToUpper()[i].Equals(sammelObjekt.name[0]))
                        {
                            //Ersetze ein _ im gel�sten Wort
                            gel�stesWort = gel�stesWort.Remove(i,1).Insert(i, sammelObjekt.name);
                            ZielwortT.text = gel�stesWort;
                        }
                    }
                }
                
                





                //Deaktiviere den Buchstaben
                sammelObjekt.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void StarteBewegung(InputAction.CallbackContext context)
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        drehenAktion.performed -= StarteBewegung;
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
            richtung = collision.transform.parent.gameObject.GetComponent<DrehenderPfeil>().richtung;
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
        //Bei Zusammensto� mit einer Wand
        if (collision.gameObject.CompareTag("Wand"))
        {
            //Starte das Level neu
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        drehenAktion.performed -= StarteBewegung;
        sammelAktion.performed -= Sammeln;
    }
}
