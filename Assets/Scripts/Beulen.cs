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
    /// Bilder f�r steigende Anzahl Beulen
    /// </summary>
    public List<Sprite> beulenSprites;
    /// <summary>
    /// Farbe f�r steigende Anzahl Beulen
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
    /// Kollisionsfunktion. Erh�ht Beulen Anzahl und startet Level neu, wenn n�tig
    /// </summary>
    public void Autsch()
    {
        beulen++; //Erh�he Beulen Anzahl
        //Wenn die maximale Beulenzahl �berschritten ist
        if (beulen > maxBeulen)
        {
            softResetGame.TriggerEvent(resetPunkt.transform.position,resetRichtung); //Starte Level neu
        }
        else
        {
            img.overrideSprite = beulenSprites[beulen]; //Lade n�chstes Bild aus Liste
            img.color = beulenColors[beulen]; //Lade n�chste Farbe aus Liste
        }
    }
}
