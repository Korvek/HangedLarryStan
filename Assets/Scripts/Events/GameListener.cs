using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEventChar charEvent;
    public UnityEvent onEventTriggered;
    public CharEvent onEventTriggeredChar;

    void OnEnable()
    {
        charEvent.AddListener(this);
    }

    void OnDisable()
    {
        charEvent.RemoveListener(this);
    }
    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();
    }
    public void OnEventTriggered(char buchstabe)
    {
        onEventTriggeredChar.Invoke(buchstabe);
    }
}