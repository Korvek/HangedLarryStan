using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour
{
    public GameEvent kollision;
    //Animator Komponente
    private Animator anim;
    public bool schloss2;
    public bool wort1;

    void Awake()
    {
        //Finde Animator Komponente
        anim = GetComponent<Animator>();
        //Löse Animation aus
    }
    /// <summary>
    /// Animation bei Schlossentfernung
    /// </summary>
    public void Schlossauflösen()
    {
        if (schloss2)
        {
            if (!wort1)
            {
                wort1 = true;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                //Löse Animation aus
                anim.SetTrigger("TriggerSchloss2");
            }
        }
        else
        {
            Debug.LogWarning(anim.name);
            GetComponent<Collider2D>().enabled = false;
            //Löse Animation aus
            anim.SetTrigger("TriggerSchloss1");
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            kollision.TriggerEvent();
        }
    }
}
