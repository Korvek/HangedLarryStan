using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event/Char")]
public class GameEventChar : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener> ();

    public void TriggerEvent(char buchstabe)
    {
        foreach(GameEventListener listener in listeners)
        {
            listener.OnEventTriggered(buchstabe);
        }
    }

    public void AddListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
