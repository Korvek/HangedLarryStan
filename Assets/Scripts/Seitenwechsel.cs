using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seitenwechsel : MonoBehaviour
{
    public GameEvent seitenWechsel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            seitenWechsel.TriggerEvent();
        }
    }
}
