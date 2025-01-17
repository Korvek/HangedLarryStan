using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Beulen : MonoBehaviour
{
    /// <summary>
    /// Maximale Anzahl an erlaubten Beulen
    /// </summary>
    public int maxBeulen;
    /// <summary>
    /// Levelreset Event
    /// </summary>
    public GameEventPosition softResetGame;
    public GameObject resetPunkt;
    public Richtung resetRichtung;
    /// <summary>
    /// Bilder für steigende Anzahl Beulen
    /// </summary>
    public List<Sprite> beulenSprites;
    /// <summary>
    /// Farbe für steigende Anzahl Beulen
    /// </summary>
    public List<Color> beulenColors;

    // Aktuelle Anzahl Beulen
    private int beulen;
    // Image Komponente
    private Image img;
    private void Awake()
    {
        //Hole Image Komponente
        img = GetComponent<Image>();
    }
    /// <summary>
    /// Kollisionsfunktion. Erhöht Beulen Anzahl und startet Level neu, wenn nötig
    /// </summary>
    public void Autsch()
    {
        beulen++; //Erhöhe Beulen Anzahl
        //Wenn die maximale Beulenzahl überschritten ist
        if (beulen > maxBeulen)
        {
            softResetGame.TriggerEvent(resetPunkt.transform.position,resetRichtung); //Starte Level neu
        }
        else
        {
            img.overrideSprite = beulenSprites[beulen]; //Lade nächstes Bild aus Liste
            img.color = beulenColors[beulen]; //Lade nächste Farbe aus Liste
        }
    }
}
