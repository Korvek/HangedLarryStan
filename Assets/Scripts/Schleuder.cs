using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schleuder : MonoBehaviour
{
    //Zielpunkt der Schleuder
    public GameObject zielpunkt;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Wenn der Spieler die Schleuder berührt
        if (collision.gameObject.CompareTag("Player"))
        {
            //Setz ihn an den Zielpunkt
            collision.gameObject.transform.position = zielpunkt.transform.position;
        }
    }
}
