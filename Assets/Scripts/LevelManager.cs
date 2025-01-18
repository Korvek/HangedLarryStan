using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject stanley;
    public GameObject spielfeld;
    public GameObject resetPunkt;
    public Richtung resetRichtung;
    public GameEvent seite1;
    public GameEvent seite2;
    [SerializeField] BackgroundFader backgroundFader;
    private GameObject stan;

    private bool wortGelöst=false;
    private void Start()
    {
        InitGame();        
        backgroundFader.Init();
        backgroundFader.enabled = true;
        Debug.Log("G");
        seite1.TriggerEvent();
    }
    /// <summary>
    /// Lade das nächste Level
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        //TODO: Zurück zum Startscreen
    }
    /// <summary>
    /// Beende das Spiel
    /// </summary>
    public void EndGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    /// <summary>
    /// Starte das aktuelle Level neu
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InitGame()
    {
        stan = Renderer.Instantiate(stanley,spielfeld.transform);
        stan.SetActive(true);
    }
    public void SoftReset()
    {
        Quaternion rot = Quaternion.identity;
        Renderer.Destroy(stan);
        if (!wortGelöst)
        {
            stan = Renderer.Instantiate(stanley, spielfeld.transform);
            stan.SetActive(true);
        }
        else if (wortGelöst)
        {
            switch (resetRichtung)
            {
                case Richtung.Oben:
                    rot = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case Richtung.Unten:
                    rot = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Richtung.Rechts:
                    rot = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case Richtung.Links:
                    rot = Quaternion.Euler(0f, 0f, 90f);
                    break;
            }
            stan = Renderer.Instantiate(stanley, resetPunkt.transform.position, rot, spielfeld.transform);
            stan.GetComponent<Stanley>().richtung = resetRichtung;
            stan.SetActive(true);
        }
    }
    public void Sprung(Vector3 newPos, Richtung newRichtung)
    {
        Renderer.Destroy(stan);
        Quaternion rot = Quaternion.identity;
        switch (newRichtung)
        {
            case Richtung.Oben:
                rot = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Richtung.Unten:
                rot = Quaternion.Euler(0f, 0f, 180f);
                break;
            case Richtung.Rechts:
                rot = Quaternion.Euler(0f, 0f, -90f);
                break;
            case Richtung.Links:
                rot = Quaternion.Euler(0f, 0f, 90f);
                break;
        }
        stan = Renderer.Instantiate(stanley, newPos, rot, spielfeld.transform);
        stan.GetComponent<Stanley>().richtung = resetRichtung;
        stan.SetActive(true);
    }

    public void WortGelöst()
    {
        if (!wortGelöst)
            seite2.TriggerEvent();
        wortGelöst = true;
    }
}
