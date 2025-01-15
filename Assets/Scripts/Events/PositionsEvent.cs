using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PositionsEvent : UnityEvent<Vector3, Richtung> { }
