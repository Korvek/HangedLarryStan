using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ausgang : MonoBehaviour
{
    /// <summary>
    /// Nächstes Level Event
    /// </summary>
    public GameEvent nextLevel;
    /// <summary>
    /// Level Reset Event
    /// </summary>
    public GameEvent resetGame;
    //Ob das Zielwort gelöst wurde
    private bool wortGelöst =false;
    /// <summary>
    /// Setze True, wenn das Zielwort gelöst wurde
    /// </summary>
    public void WortGelöst()
    {
        wortGelöst=true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIER");
        
        //Wenn der Spieler das Element betritt
        if (collision.CompareTag("Player"))
        {
            Debug.Log("AUCH");
            //Wenn das Wort gelöst worden ist
            if (wortGelöst)
            {
                nextLevel.TriggerEvent(); //Starte nächstes Level
            }
            else
            {
                resetGame.TriggerEvent(); //Starte Level neu
            }
        }
    }
}
