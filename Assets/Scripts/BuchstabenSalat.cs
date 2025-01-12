using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuchstabenSalat : MonoBehaviour
{
    public GameEvent endGame;
    public void BuchstabenSalatAuflösen()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("WOOHOO");
        if (collision.CompareTag("Player"))
        {
            endGame.TriggerEvent();
            
        }
    }
}
