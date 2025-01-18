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

    private void Start()
    {
        cameraMoveAktion = actions.FindActionMap("Menu").FindAction("CameraMoveAktion");
        cameraMoveAktion.performed += Verschwinde;
        GetComponent<CanvasRenderer>().SetColor(new Color(
            GetComponent<CanvasRenderer>().GetColor().r,
            GetComponent<CanvasRenderer>().GetColor().g,
            GetComponent<CanvasRenderer>().GetColor().b,
            0));
        StartCoroutine(Erscheine());
    }

    private void Verschwinde(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        cameraMoveAktion.performed -= Verschwinde;
    }
    IEnumerator Erscheine()
    {
        yield return new WaitForSeconds(3);
        GetComponent<CanvasRenderer>().SetColor(new Color(
            GetComponent<CanvasRenderer>().GetColor().r,
            GetComponent<CanvasRenderer>().GetColor().g,
            GetComponent<CanvasRenderer>().GetColor().b,
            1));
        //yield return null;
    }
}
