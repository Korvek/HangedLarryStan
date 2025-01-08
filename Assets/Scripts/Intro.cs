using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public InputAction startAktion;
    void Awake()
    {
        startAktion.performed += StarteSpiel;
    }
    private void OnEnable()
    {
        startAktion.Enable();
    }
    private void OnDisable()
    {
        startAktion.Disable();
    }
    private void OnDestroy()
    {
        startAktion.performed -= StarteSpiel;
    }
    private void StarteSpiel(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
