using UnityEngine;

public class Gegner : MonoBehaviour
{
    /// <summary>
    /// Objekt das Zielpunkt des Gegners gibt
    /// </summary>
    public GameObject zielpunkt;
    /// <summary>
    /// Bewegungsgeschwindigkeit
    /// </summary>
    public int tempo = 1;
    /// <summary>
    /// Kollisionsevent
    /// </summary>
    public GameEvent kollisionEvent;
    //Startpunkt, des Gegners
    private Vector2 startpunkt;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("A");
        //Wenn mit dem Spieler kollidiert wird
        if (collision.CompareTag("Player"))
        {
            kollisionEvent.TriggerEvent();
        }
    }
    private void Awake()
    {
        //Speichere den Startpunkt
        startpunkt = transform.position;
    }
    private void FixedUpdate()
    {
        //Bewegung Richtung zielpunkt
        transform.position = Vector2.MoveTowards(transform.position, zielpunkt.transform.position, tempo * Time.fixedDeltaTime);
        //Bei Erreichen des Zielpunkts
        if (transform.position == zielpunkt.transform.position)
        {
            //Vertausche Ziel und Startpunkt
            Vector2 tmpStart = new Vector2(startpunkt.x, startpunkt.y);
            startpunkt = zielpunkt.transform.position;
            zielpunkt.transform.position = tmpStart;
            transform.Rotate(new Vector3(0f, 0f, 180f));
        }
    }
}
