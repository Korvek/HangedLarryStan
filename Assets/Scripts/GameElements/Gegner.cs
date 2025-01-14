using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gegner : MonoBehaviour
{
    public GameObject zielpunkt;
    public Vector2 startpunkt;
    public int tempo = 1;
    private void OnColliderEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void Awake()
    {
        startpunkt = transform.position;
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, zielpunkt.transform.position, tempo * Time.fixedDeltaTime);

        if (transform.position == zielpunkt.transform.position)
        {
            Vector2 tmpStart = new Vector2(startpunkt.x, startpunkt.y);
            startpunkt = zielpunkt.transform.position;
            zielpunkt.transform.position = tmpStart;
        }
    }
}
