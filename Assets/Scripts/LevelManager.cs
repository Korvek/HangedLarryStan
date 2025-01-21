using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject stanley;
    public GameObject spielfeld;
    public GameObject resetPunkt;
    public GameObject zielwort1;
    public GameObject zielwort2;
    public Richtung resetRichtung;
    public GameEvent seite1;
    public GameEvent seite2;
    public GameEvent zeitBonus;
    [SerializeField] BackgroundFader backgroundFader;
    private GameObject stan;

    private bool zweiteSeiteerreicht=false;
    private void Start()
    {
        InitGame();        
        backgroundFader.Init();
        backgroundFader.enabled = true;
        seite1.TriggerEvent();
    }
    /// <summary>
    /// Lade das nächste Level
    /// </summary>
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 4)
        {
            StartCoroutine(LoadAsyncNextLevel(0));
            //SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(LoadAsyncNextLevel(SceneManager.GetActiveScene().buildIndex + 1));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    /// <summary>
    /// Beende das Spiel
    /// </summary>
    public void EndGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    /// <summary>
    /// Starte das aktuelle Level neu
    /// </summary>
    public void ResetGame()
    {
        StartCoroutine(LoadAsyncReset(SceneManager.GetActiveScene().buildIndex));
        Renderer.Destroy(stan);
    }
    /// <summary>
    /// Instanziere Stanley
    /// </summary>
    public void InitGame()
    {
        stan = Renderer.Instantiate(stanley,spielfeld.transform);
        stan.SetActive(true);
    }
    /// <summary>
    /// Reaktion auf Kollision
    /// </summary>
    public void SoftReset()
    {
        Quaternion rot = Quaternion.identity;
        Renderer.Destroy(stan);
        if (!zweiteSeiteerreicht)
        {
            stan = Renderer.Instantiate(stanley, spielfeld.transform);
            stan.SetActive(true);
        }
        else if (zweiteSeiteerreicht)
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
    /// <summary>
    /// Versetze den Spieler
    /// </summary>
    /// <param name="newPos">Neue Position</param>
    /// <param name="newRichtung">Neue Richtung</param>
    public void Sprung(Vector3 newPos, Richtung newRichtung)
    {
        Renderer.Destroy(stan); //Lösche alte Spielerfigur
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
        //Erzeuge neue Spielerfigur
        stan = Renderer.Instantiate(stanley, newPos, rot, spielfeld.transform);
        stan.GetComponent<Stanley>().richtung = newRichtung;
        stan.SetActive(true);
    }
    /// <summary>
    /// Übergang zur nächsten Seite
    /// </summary>
    public void Seitenwechsel()
    {
        zweiteSeiteerreicht = true;
        zielwort2.SetActive(true);
        zielwort1.SetActive(false);
        zeitBonus.TriggerEvent();

    }
    IEnumerator LoadAsyncReset(int buildIndex)
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        asyncLoad.allowSceneActivation = false;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(4.33f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadAsyncNextLevel(int buildIndex)
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        asyncLoad.allowSceneActivation = false;
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(2f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
