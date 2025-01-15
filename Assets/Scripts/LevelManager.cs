using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Lade das nächste Level
    /// </summary>
    public void NextLevel()
    {
        Debug.Log("TTTT");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
}
