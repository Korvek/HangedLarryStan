using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour
{
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
        //Löse Animation aus
        anim.SetTrigger("TriggerSchloss");
    }
}
