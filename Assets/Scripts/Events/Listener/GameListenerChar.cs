using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerChar : MonoBehaviour
{
    public GameEventChar charEvent;
    public CharEvent onEventTriggeredChar;
    

    void OnEnable()
    {
        charEvent.AddListener(this);
    }

    void OnDisable()
    {
        charEvent.RemoveListener(this);
    }
    public void OnEventTriggered(char buchstabe)
    {
        onEventTriggeredChar.Invoke(buchstabe);
    }
}