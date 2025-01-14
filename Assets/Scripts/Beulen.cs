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

    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }
    public void Autsch()
    {        
        beulen++;        
        if(beulen >= maxBeulen )
        {
            resetGame.TriggerEvent();
        }
        img.overrideSprite = beulenSprites[beulen];
    }
}
