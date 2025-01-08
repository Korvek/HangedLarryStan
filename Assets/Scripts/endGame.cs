using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("WOOHOO");
        if (collision.CompareTag("Player"))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
