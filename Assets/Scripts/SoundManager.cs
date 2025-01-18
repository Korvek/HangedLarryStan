using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct Audios
{
    public string name;
    public AudioSource source;
}
public class SoundManager : MonoBehaviour
{
    public List<Audios> audiosList;
    private Dictionary<string, AudioSource> audioSources;
    private void Awake()
    {
        audioSources = new Dictionary<string, AudioSource>();
        foreach (Audios audios in audiosList)
        {
            audioSources[audios.name] = audios.source;
        }
    }
    

    public void PlayBuchstabeRichtig()
    {
        audioSources["BuchstabeRichtig"].Play();
    }
    public void PlayBuchstabeFalsch()
    {
        audioSources["BuchstabeFalsch"].Play();
    }
    public void PlaySchlossKnacken()
    {
        audioSources["SchlossKnacken"].Play();
    }
    public void PlayGegnerKollision()
    {
        audioSources["GegnerKollision"].Play();
    }
    public void PlaySeite1()
    {
        Debug.Log("1");
        audioSources["Seite1"].Play();
    }
    public void PlaySeite2()
    {
        Debug.Log("2");
        audioSources["Seite2"].Play();
    }
    public void PlaySeite3()
    {
        Debug.Log("3");
        audioSources["Seite3"].Play();
    }
    public void PlaySeite4()
    {
        Debug.Log("4");
        audioSources["Seite4"].Play();
    }
    public void PlaySeite5()
    {
        Debug.Log("5");
        audioSources["Seite5"].Play();
    }
    public void PlaySeite6()
    {
        Debug.Log("6");
        audioSources["Seite6"].Play();
    }
    public void PlaySeite7()
    {
        Debug.Log("7");
        audioSources["Seite7"].Play();
    }
    public void PlaySeite8()
    {
        Debug.Log("8");
        audioSources["Seite8"].Play();
    }


}
