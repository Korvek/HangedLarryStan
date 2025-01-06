using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    public float geschwindigkeit=0.1f;

    //Richtungsvektor. Gibt die Bewegungsrichtung an
    private Vector2 bewegung;
    //Körperkomponente
    private Rigidbody2D rigidbody2d;
    
    private void Awake()
    {
        //Hole Rigidbody Komponente des Objekts
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Initialisierung des Richtungsvektors
        bewegung = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Wechselt die Richtung je nach Richtungsvariable
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
    void RichtungWechseln()
    {
        if (richtung != Richtung.Links)
        {
            richtung++;
        }
        else
        {
            richtung = Richtung.Oben;
        }
    }
}
