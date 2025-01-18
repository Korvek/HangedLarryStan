using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartHinweis : MonoBehaviour
{
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;
    private InputAction cameraMoveAktion;

    private void Awake()
    {
        cameraMoveAktion = actions.FindActionMap("Menu").FindAction("CameraMoveAktion");
        cameraMoveAktion.performed += Verschwinde;
    }

    private void Verschwinde(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        cameraMoveAktion.performed -= Verschwinde;
    }
}
