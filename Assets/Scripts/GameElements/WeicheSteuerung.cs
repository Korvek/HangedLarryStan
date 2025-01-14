using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeicheSteuerung : MonoBehaviour
{
    public GameEvent weicheStellen;
    public GameEvent weicheBlocken;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            weicheStellen.TriggerEvent();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Wenn ein Spieler den Umschaltbereich verlässt
        if (collision.CompareTag("Player"))
        {
            //Deaktiviere InputActions
            weicheBlocken.TriggerEvent();
        }
    }
}
