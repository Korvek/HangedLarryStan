using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public void PlayMauerKollision()
    {
        audioSources["GegnerKollision"].Play();
    }
    public void PlaySeite1()
    {
        audioSources["Seite1"].PlayDelayed(0f);
        audioSources["Seite2"].PlayDelayed(audioSources["Seite1"].clip.length);
    }
    public void PlaySeite2()
    {
        audioSources["Seite2"].PlayDelayed(0f);
    }
    public void PlaySeite3()
    {
        audioSources["Seite3"].PlayDelayed(0f);
        audioSources["Seite4"].PlayDelayed(audioSources["Seite3"].clip.length);
    }
    public void PlaySeite4()
    {
        audioSources["Seite4"].PlayDelayed(0f);
    }
    public void PlaySeite5()
    {
        audioSources["Seite5"].PlayDelayed(0f);
        audioSources["Seite6"].PlayDelayed(audioSources["Seite5"].clip.length);
    }
    public void PlaySeite6()
    {
        audioSources["Seite6"].PlayDelayed(0f);
    }
    public void PlaySeite7()
    {
        audioSources["Seite7"].PlayDelayed(0f);
        audioSources["Seite8"].PlayDelayed(audioSources["Seite7"].clip.length);
    }
    public void PlaySeite8()
    {
        audioSources["Seite8"].PlayDelayed(0f);
    }
    public void LevelStart()
    {
        foreach(Audios audio in audiosList)
        {
            audio.source.Stop();
        }
    }
    public void Seitenwechsel()
    {
        audioSources["Seitenwechsel"].Play();
    }
    public void Sprungfeder()
    {
        audioSources["Sprungfeder"].Play();
    }
    public void Reset()
    {
        Debug.Log("Res");
        audioSources["Reset"].Play();
    }



}
