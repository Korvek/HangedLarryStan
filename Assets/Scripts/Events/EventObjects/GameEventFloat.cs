using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event/Float")]
public class GameEventFloat : ScriptableObject
{
    private List<GameEventListenerFloat> listeners = new List<GameEventListenerFloat> ();

    public void TriggerEvent(float zahl)
    {
        foreach(GameEventListenerFloat listener in listeners)
        {
            listener.OnEventTriggered(zahl);
        }
    }

    public void AddListener(GameEventListenerFloat listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListenerFloat listener)
    {
        listeners.Remove(listener);
    }
}
