using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ausgang : MonoBehaviour
{
    public GameEvent nextLevel;
    public GameEvent resetGane;
    private bool wortGel�st =false;
    public void WortGel�st()
    {
        wortGel�st=true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (wortGel�st)
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
