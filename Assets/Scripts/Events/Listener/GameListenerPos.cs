using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerPosition : MonoBehaviour
{
    public GameEventPosition posEvent;
    public PositionsEvent onEventTriggeredPos;
    

    void OnEnable()
    {
        posEvent.AddListener(this);
    }

    void OnDisable()
    {
        posEvent.RemoveListener(this);
    }
    public void OnEventTriggered(Vector3 pos,Richtung richtung)
    {
        onEventTriggeredPos.Invoke(pos, richtung);
    }
}