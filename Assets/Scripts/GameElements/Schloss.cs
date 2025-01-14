using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schloss : MonoBehaviour
{
    
    Animator anim;
    void Awake()
    {
        //Finde Animator Komponente
        anim = GetComponent<Animator>();
        //Löse Animation aus
        //anim.SetTrigger("TriggerSchloss");
    }

    public void Schlossauflösen()
    {
        //Löse Animation aus
        anim.SetTrigger("TriggerSchloss");
    }
}
