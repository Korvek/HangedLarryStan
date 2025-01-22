using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuchstabenSalat : MonoBehaviour
{
    public GameEvent endGame;
    public GameEventFloat zeitBonus;
    public float bonusZeit;
    public void BuchstabenSalatAuflösen()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endGame.TriggerEvent();
            zeitBonus.TriggerEvent(bonusZeit);
            
        }
    }
}
