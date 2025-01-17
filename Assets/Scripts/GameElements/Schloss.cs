using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour
{
    public GameEvent kollision;
    //Animator Komponente
    private Animator anim;

    void Awake()
    {
        //Finde Animator Komponente
        anim = GetComponent<Animator>();
        //L�se Animation aus
    }
    /// <summary>
    /// Animation bei Schlossentfernung
    /// </summary>
    public void Schlossaufl�sen()
    {
        GetComponent<Collider2D>().enabled = false;
        //L�se Animation aus
        anim.SetTrigger("TriggerSchloss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            kollision.TriggerEvent();
        }
    }
}
