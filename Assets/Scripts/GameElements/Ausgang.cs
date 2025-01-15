using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ausgang : MonoBehaviour
{
    /// <summary>
    /// N�chstes Level Event
    /// </summary>
    public GameEvent nextLevel;
    /// <summary>
    /// Level Reset Event
    /// </summary>
    public GameEvent resetGame;
    //Ob das Zielwort gel�st wurde
    private bool wortGel�st =false;
    /// <summary>
    /// Setze True, wenn das Zielwort gel�st wurde
    /// </summary>
    public void WortGel�st()
    {
        wortGel�st=true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIER");
        
        //Wenn der Spieler das Element betritt
        if (collision.CompareTag("Player"))
        {
            Debug.Log("AUCH");
            //Wenn das Wort gel�st worden ist
            if (wortGel�st)
            {
                nextLevel.TriggerEvent(); //Starte n�chstes Level
            }
            else
            {
                resetGame.TriggerEvent(); //Starte Level neu
            }
        }
    }
}
