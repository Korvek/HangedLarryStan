using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schleuder : MonoBehaviour
{
    //Zielpunkt der Schleuder
    public GameObject zielpunkt;

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
            Debug.Log(collision.gameObject.transform.position);
            collision.gameObject.transform.position = zielpunkt.transform.position;
            Debug.Log(zielpunkt.transform.position);
            Debug.Log(collision.gameObject.transform.position);
        }
    }
}
