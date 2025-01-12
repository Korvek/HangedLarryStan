using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event/Char")]
public class GameEventChar : ScriptableObject
{
    private List<GameEventListenerChar> listeners = new List<GameEventListenerChar> ();

    public void TriggerEvent(char buchstabe)
    {
        foreach(GameEventListenerChar listener in listeners)
        {
            listener.OnEventTriggered(buchstabe);
        }
    }

    public void AddListener(GameEventListenerChar listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListenerChar listener)
    {
        listeners.Remove(listener);
    }
}
