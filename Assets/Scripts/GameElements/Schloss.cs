using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour
{
    public GameEvent kollision;
    public AudioClip buchstabeRichtig;
    //Animator Komponente
    private Animator anim;

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
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Knacken());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            kollision.TriggerEvent();
        }
    }

    IEnumerator Knacken()
    {
        yield return new WaitForSeconds(buchstabeRichtig.length);
        
        //Löse Animation aus
        anim.SetTrigger("TriggerSchloss");
    }
}
