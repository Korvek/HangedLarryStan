using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeicheSteuerung : MonoBehaviour
{
    public GameEventObject weicheStellen;
    public GameEventObject weicheBlocken;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Wenn der Spieler im richtigen Bereich ist
        if (collision.CompareTag("Player"))
        {
            //Weiche ist drehbar
            weicheStellen.TriggerEvent(transform.parent.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Wenn ein Spieler den Umschaltbereich verlässt
        if (collision.CompareTag("Player"))
        {
            //Weiche ist fest
            weicheBlocken.TriggerEvent(transform.parent.gameObject);
        }
    }
}
