using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Beulen : MonoBehaviour
{
    public int maxBeulen;
    public int beulen;
    public GameEvent resetGame;
    public List<Sprite> beulenSprites;
    public List<Color> beulenColors;

    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    public void Autsch()
    {
        beulen++;
        if (beulen > maxBeulen)
        {
            resetGame.TriggerEvent();
        }
        else
        {
            img.overrideSprite = beulenSprites[beulen];
            img.color = beulenColors[beulen];
        }
    }
}
