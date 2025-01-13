using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerPosition : MonoBehaviour
{
    public GameEventPosition charEvent;
    public CharEvent onEventTriggeredChar;
    

    void OnEnable()
    {
        charEvent.AddListener(this);
    }

    void OnDisable()
    {
        charEvent.RemoveListener(this);
    }
    public void OnEventTriggered(Vector3 pos)
    {
        //onEventTriggeredChar.Invoke(pos);
    }
}