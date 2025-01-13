using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event/Position")]
public class GameEventPosition : ScriptableObject
{
    private List<GameEventListenerPosition> listeners = new List<GameEventListenerPosition> ();

    public void TriggerEvent(Vector3 pos)
    {
        foreach(GameEventListenerPosition listener in listeners)
        {
            listener.OnEventTriggered(pos);
        }
    }

    public void AddListener(GameEventListenerPosition listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListenerPosition listener)
    {
        listeners.Remove(listener);
    }
}
