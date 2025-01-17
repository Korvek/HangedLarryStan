using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schleuder : MonoBehaviour
{
    /// <summary>
    /// Zielpunkt f�r die Schleuder
    /// </summary>
    public GameObject zielpunkt;
    /// <summary>
    /// Ausgel�stes Event
    /// </summary>
    public GameEventPosition sprung;

    /// <summary>
    /// Richtung des Spielers nachdem er geschleudert wurde
    /// </summary>
    public Richtung richtung;
    //Animator Komponente
    private Animator anim;

    private void Awake()
    {
        //Hole Animator
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Wenn der Spieler die Schleuder ber�hrt
        if (collision.gameObject.CompareTag("Player"))
        {
            //Starte Animation
            anim.SetTrigger("TriggerSprungfeder");
            //Setz ihn an den Zielpunkt
            sprung.TriggerEvent(zielpunkt.transform.position,richtung);
        }
    }
}
