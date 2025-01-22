using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerFloat : MonoBehaviour
{
    public GameEventFloat floatEvent;
    public FloatEvent onEventTriggeredFloat;
    

    void OnEnable()
    {
        floatEvent.AddListener(this);
    }

    void OnDisable()
    {
        floatEvent.RemoveListener(this);
    }
    public void OnEventTriggered(float zahl)
    {
        onEventTriggeredFloat.Invoke(zahl);
    }
}