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

    private bool wortGelöst=false;
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
        if (SceneManager.GetActiveScene().buildIndex > 4)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
        Renderer.Destroy(stan);
        //SceneManager.LoadScene(.name);
        Debug.Log("MANA");
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
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
        stan.GetComponent<Stanley>().richtung = newRichtung;
        stan.SetActive(true);
    }

    public void Seitenwechsel()
    {
        wortGelöst = true;
        zielwort2.SetActive(true);
        zielwort1.SetActive(false);
        zeitBonus.TriggerEvent();

    }
    IEnumerator LoadAsync(int buildIndex)
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        asyncLoad.allowSceneActivation = false;
        Debug.Log("Start");
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(4.33f);
        Debug.Log("Wait");
        asyncLoad.allowSceneActivation = true;
        Debug.Log("Waited");
        while (!asyncLoad.isDone)
        {
            Debug.Log("Done");
            yield return null;
        }
    }
}
