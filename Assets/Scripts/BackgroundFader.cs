using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using System; // Namespace f�r TextMeshPro

public class BackgroundFader : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Dauer der �berblendung
    public float delay = 0.5f; // Verz�gerung vor der �berblendung
    /// <summary>
    /// Objekt das alle Spielelemente enth�lt, die nach dem Fade In erscheinen sollen
    /// </summary>
    [Header("Enth�lt Spielelemente, die nach dem Fade In erscheinen sollen")]
    public GameObject spielfeld;
    /// <summary>
    /// Objekt das alle UI-Elemente enth�lt, die nach dem Fade In erscheinen sollen
    /// </summary>
    [Header("Enth�lt UI-Elemente, die nach dem Fade In erscheinen sollen")]
    public GameObject uI;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;

    private List<Color> initialImageColor;
    private List<Color> initialSpriteColor;
    private List<SpriteRenderer> spriteRenderers;
    private List<CanvasRenderer> canvasRenderers;
    private InputAction fadeInAktion;
    private InputAction tutorialPopUpAktion;

    public GameEvent levelEnter;
    public GameEvent stanleyEnter;
    public GameObject startPopUp;

    public void Init()
    {
        actions.FindActionMap("Player").Disable();
        initialImageColor = new List<Color>(); 
        initialSpriteColor = new List<Color>();
        //Finde alle Spriterenderer des Spielfelds
        spriteRenderers = spielfeld.GetComponentsInChildren<SpriteRenderer>().ToList();
        //Finde alle Canvasrenderer des UIs
        canvasRenderers = uI.GetComponentsInChildren<CanvasRenderer>().ToList();
        // Speichere die urspr�nglichen Farben
        for (int i = 0; i < canvasRenderers.Count; i++)
        {
            //Speichere urspr�ngliche Farbe
            initialImageColor.Add(canvasRenderers[i].GetColor());
        }
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            //Speichere urspr�ngliche Farbe
            initialSpriteColor.Add(spriteRenderers[i].color);
        }
        //InputAktion zuweisen
        fadeInAktion = actions.FindActionMap("Menu").FindAction("FadeInAktion");
        fadeInAktion.performed += StartFadeIn;
        tutorialPopUpAktion = actions.FindActionMap("Player").FindAction("StartAktion");
        tutorialPopUpAktion.performed += disableStart;

        //Alle Objekte werden transparent
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                0);
        }
        foreach (CanvasRenderer canvasRenderer in canvasRenderers)
        {
            canvasRenderer.SetColor(new Color(
                canvasRenderer.GetColor().r,
                canvasRenderer.GetColor().g,
                canvasRenderer.GetColor().b,
                0));
        }
    }

    private void disableStart(InputAction.CallbackContext context)
    {
        if (startPopUp != null)
            startPopUp.SetActive(false);
    }

    private void StartFadeIn(InputAction.CallbackContext context)
    {
        levelEnter.TriggerEvent();
        // �berblendung starten
        StartCoroutine(FadeIn());
        fadeInAktion.performed -= StartFadeIn;
    }

    private IEnumerator FadeIn()
    {
        // Warte die Verz�gerungszeit
        yield return new WaitForSeconds(delay);

        // F�hre die �berblendung durch
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // Fortschritt berechnen (0-1)
            // Interpoliere die Farbe f�r alle Sprites
            for (int i = 0; i < canvasRenderers.Count; i++)
            {
                canvasRenderers[i].SetColor(new Color(
                    initialImageColor[i].r,
                    initialImageColor[i].g,
                    initialImageColor[i].b,
                    Mathf.Lerp(0f, initialImageColor[i].a, t)));
            }
            // Interpoliere die Farbe f�r alle UI-Elemente
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                spriteRenderers[i].color = new Color(
                    initialSpriteColor[i].r,
                    initialSpriteColor[i].g,
                    initialSpriteColor[i].b,
                    Mathf.Lerp(0f, initialSpriteColor[i].a, t));
            }
            yield return new WaitForEndOfFrame(); // Warte bis zum n�chsten Frame
        }
        stanleyEnter.TriggerEvent();
        yield return new WaitForSeconds(1.2f);
        actions.FindActionMap("Player").Enable();
        if (startPopUp != null)
            startPopUp.SetActive(true);
    }

    

    private void OnEnable()
    {
        //Aktiviere InputActions
        fadeInAktion.Enable();
    }
    private void OnDisable()
    {
        //Deaktiviere InputActions
        fadeInAktion.Disable();
    }
    private void OnDestroy()
    {
        //L�se Verkn�pfungen
        fadeInAktion.performed -= StartFadeIn;
    }
}
