using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject stanley;
    public GameObject spielfeld;
    public GameObject resetPunkt;
    public GameObject zielwort1;
    public GameObject zielwort2;
    public GameObject video;
    public Richtung resetRichtung;
    public GameEvent seite1;
    public GameEvent seite2;
    public GameEvent zeitBonus;
    public GameEvent stanEintritt;
    [SerializeField] BackgroundFader backgroundFader;
    private GameObject stan;
    public InputActionAsset inputActions;
    private InputAction endGame;

    private bool zweiteSeiteerreicht=false;
    private void Start()
    {
        endGame = inputActions.FindActionMap("Menu").FindAction("EscAktion");
        endGame.performed += EndGame;
        endGame.Enable();
        Time.timeScale = 1f;
        InitGame();        
        backgroundFader.Init();
        backgroundFader.enabled = true;
        seite1.TriggerEvent();
    }
    /// <summary>
    /// Beende das Spiel
    /// </summary>
    private void EndGame(InputAction.CallbackContext context)
    {
        Debug.Log("Ende");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    /// <summary>
    /// Lade das n�chste Level
    /// </summary>
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 5)
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
        stan.GetComponent<SpriteRenderer>().enabled = false;
        stan.SetActive(true);
    }
    /// <summary>
    /// Reaktion auf Kollision
    /// </summary>
    public void SoftReset()
    {
        StartCoroutine(Kollision());
        
    }
    public void StanleyEnter()
    {
        stan.GetComponent<SpriteRenderer>().enabled = true;
    }
    /// <summary>
    /// Versetze den Spieler
    /// </summary>
    /// <param name="newPos">Neue Position</param>
    /// <param name="newRichtung">Neue Richtung</param>
    public void Sprung(Vector3 newPos, Richtung newRichtung)
    {
        Renderer.Destroy(stan); //L�sche alte Spielerfigur
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
        stan.GetComponent<SpriteRenderer>().enabled = true;
        stan.SetActive(true);
    }
    /// <summary>
    /// Versetze den Spieler
    /// </summary>
    /// <param name="newPos">Neue Position</param>
    /// <param name="newRichtung">Neue Richtung</param>
    public void Tunneln(Vector3 newPos, Richtung newRichtung)
    {
        StartCoroutine(LochBewegung(newPos, newRichtung));
    }
    /// <summary>
    /// �bergang zur n�chsten Seite
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
        yield return new WaitForSeconds(4.33f);
        Time.timeScale = 0.0f;
        SceneManager.LoadScene(buildIndex);
    }

    IEnumerator LoadAsyncNextLevel(int buildIndex)
    {
        yield return new WaitForSeconds(1.2f);
        video.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(buildIndex);
    }

    IEnumerator LochBewegung(Vector3 newPos, Richtung newRichtung)
    {
        yield return new WaitForSeconds(2);
        Renderer.Destroy(stan); //L�sche alte Spielerfigur
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
        stan.GetComponent<SpriteRenderer>().enabled = false;
        stan.SetActive(true);
        stanEintritt.TriggerEvent();

    }
    IEnumerator Kollision()
    { 
        yield return new WaitForSeconds(1.5f);
        Quaternion rot = Quaternion.identity;
        Renderer.Destroy(stan);
        if (!zweiteSeiteerreicht)
        {
            stan = Renderer.Instantiate(stanley, spielfeld.transform);
            stan.GetComponent<SpriteRenderer>().enabled = false;
            stan.SetActive(true);
            stanEintritt.TriggerEvent();
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
            stan.GetComponent<SpriteRenderer>().enabled = false;
            stan.SetActive(true);
            stanEintritt.TriggerEvent();
        }
    }
}
