using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ausgang : MonoBehaviour
{
    public GameEvent nextLevel;
    public GameEvent resetGane;
    private bool wortGelöst =false;
    public void WortGelöst()
    {
        wortGelöst=true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (wortGelöst)
            {
                nextLevel.TriggerEvent();
            }
            else
            {
                resetGane.TriggerEvent();
            }
        }
    }
}
