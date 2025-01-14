using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schleuder : MonoBehaviour
{
    //Zielpunkt der Schleuder
    public GameObject zielpunkt;
    public GameEventPosition sprung;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Wenn der Spieler die Schleuder berührt
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("TriggerSprungfeder");
            //Setz ihn an den Zielpunkt
            sprung.TriggerEvent(zielpunkt.transform.position);
        }
    }
}
