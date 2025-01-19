using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoplinie : MonoBehaviour
{
    public GameObject tutorialText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Stoppe die Zeit
            Time.timeScale = 0;
            //Deaktiviere Stoplinie            
            if (tutorialText != null)
            {
                tutorialText.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (tutorialText != null)
            {
                tutorialText.SetActive(false);
            }
        }
    }
}
