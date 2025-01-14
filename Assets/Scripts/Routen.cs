using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routen : MonoBehaviour
{
    public GameEvent resetGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("Enter"+collision.gameObject.name);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.LogWarning("Exit"+collision.gameObject.name);
        resetGame.TriggerEvent();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
