using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routen : MonoBehaviour
{
    /// <summary>
    /// Kollisionsevent
    /// </summary>
    public GameEvent kollisionEvent;
    private void OnTriggerExit2D(Collider2D collision)
    {
        kollisionEvent.TriggerEvent();
    }
}