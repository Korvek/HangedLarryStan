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
    public void SchlossKnacken()
    {
        audioSources["SchlossKnacken"].PlayDelayed(audioSources["BuchstabeRichtig"].clip.length);
    }

}
