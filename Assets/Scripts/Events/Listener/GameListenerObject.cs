using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerObject : MonoBehaviour
{
    public GameEventObject objectEvent;
    public ObjectEvent onEventTriggeredObject;
    

    void OnEnable()
    {
        objectEvent.AddListener(this);
    }

    void OnDisable()
    {
        objectEvent.RemoveListener(this);
    }
    public void OnEventTriggered(GameObject gO)
    {
        onEventTriggeredObject.Invoke(gO);
    }
}