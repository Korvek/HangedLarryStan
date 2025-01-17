using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Event/Object")]
public class GameEventObject : ScriptableObject
{
    private List<GameEventListenerObject> listeners = new List<GameEventListenerObject> ();

    public void TriggerEvent(GameObject gO)
    {
        foreach(GameEventListenerObject listener in listeners)
        {
            listener.OnEventTriggered(gO);
        }
    }

    public void AddListener(GameEventListenerObject listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListenerObject listener)
    {
        listeners.Remove(listener);
    }
}
