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
        //L�se Animation aus
        //anim.SetTrigger("TriggerSchloss");
    }

    public void Schlossaufl�sen()
    {
        //L�se Animation aus
        anim.SetTrigger("TriggerSchloss");
    }
}
