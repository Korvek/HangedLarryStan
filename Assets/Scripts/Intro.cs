using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction startAktion;
    void Awake()
    {
        startAktion = actionAsset.FindActionMap("Menu").FindAction("IntroAktion");
        startAktion.performed += StarteSpiel;
    }
    private void OnEnable()
    {
        startAktion.Enable();
    }
    private void OnDestroy()
    {
        startAktion.performed -= StarteSpiel;
    }
    private void StarteSpiel(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex >= 5)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
