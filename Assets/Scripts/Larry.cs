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
    /// <summary>
    /// Geschwindigkeitsvariable. Gibt die Bewegungsgeschwindigkeit an.
    /// </summary>
    [Range(0.1f,15f)] public float geschwindigkeit=1f;
    //InputActions um auf Tastendr�cke zu reagieren
    public InputAction drehenAktion;
    public InputAction sammelAktion;
    /// <summary>
    /// Drehung mithilfe von Pfeilen oder fixe 90�
    /// </summary>
    public bool pfeilsteuerungON = true;

    public String zielwort="Larry";
    public String gel�stesWort="AAAAAAAAAAAAAAAAAA";
    /// <summary>
    /// Tempo in dem Abgebogen wird (Testzweck)
    /// </summary>
    [SerializeField]float drehTempo = 100f;

    //Textanzeige f�r das Zielwort
    public TextMeshProUGUI ZielwortT;

    //Richtungsvektor. Gibt die Bewegungsrichtung an
    //public Vector2 bewegung;
    //K�rperkomponente
    private Rigidbody2D rigidbody2d;
    //Sammelbares Objekt, das ber�hrt wird
    private GameObject sammelObjekt;
    //Richtungspfeil, der ber�hrt wird
    private GameObject pfeilObjekt;
    //Drehender Richtungspfeil, der ber�hrt wird
    private GameObject drehPfeilObjekt;
    //Richtung in die sich bewegt werden soll
    Vector2 zielrichtung;


    private void Awake()
    {
        gel�stesWort = new string('_', zielwort.Length);
        ZielwortT.text=gel�stesWort;
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
        //Wenn abgebogen werden soll
        if(Vector2.SignedAngle(transform.up,zielrichtung) !=0f)
        {
            if (Vector2.SignedAngle(transform.up, zielrichtung) >= 0f)
            {
                drehGeschwindigkeit = drehTempo * Time.fixedDeltaTime; //Linkskurve
            }
            else if (Vector2.SignedAngle(transform.up, zielrichtung) <= 0f)
            {
                drehGeschwindigkeit = -drehTempo * Time.fixedDeltaTime; //Rechtskurve
            }
        }
        //Wenn nicht mehr abgebogen wird
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
        if (!pfeilsteuerungON)
        {
            //Bewegungsvariante 90 Grad Drehung
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
    /// Sammelt das Momentan ber�hrte Objekt ein
    /// </summary>
    private void Sammeln(InputAction.CallbackContext context)
    {
       
        if (sammelObjekt != null)
        {
            //TODO Objekt entfernen, Buchstaben pr�fen, Wort anzeigen
            //Wenn der Objektname nur ein Zeichen lang ist und im L�sungswort enthalten ist
            if (sammelObjekt.name.Length == 1 && zielwort.ToUpper().Contains(sammelObjekt.name))
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
}
