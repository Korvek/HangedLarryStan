using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using System; // Namespace für TextMeshPro

public class BackgroundFader : MonoBehaviour
{
    public Image backgroundImage; // Das UI-Image-Element für den Hintergrund
    public TextMeshProUGUI backgroundText; // TMP-Text für den Hintergrundtext
    public float fadeDuration = 0.5f; // Dauer der Überblendung
    public float delay = 0.5f; // Verzögerung vor der Überblendung
    /// <summary>
    /// Objekt das alle Spielelemente enthält, die nach dem Fade In erscheinen sollen
    /// </summary>
    [Header("Enthält Spielelemente, die nach dem Fade In erscheinen sollen")]
    public GameObject spielfeld;
    /// <summary>
    /// Objekt das alle UI-Elemente enthält, die nach dem Fade In erscheinen sollen
    /// </summary>
    [Header("Enthält UI-Elemente, die nach dem Fade In erscheinen sollen")]
    public GameObject uI;
    /// <summary>
    /// Set von Input Aktionen
    /// </summary>
    public InputActionAsset actions;

    private Color initialImageColor;
    private Color initialTextColor;
    private List<SpriteRenderer> spriteRenderers;
    private List<CanvasRenderer> canvasRenderers;
    private InputAction fadeInAktion;

    void Awake()
    {
        // Speichere die ursprünglichen Farben
        initialImageColor = backgroundImage.color;
        initialTextColor = backgroundText.color;

        //InputAktion zuweisen
        fadeInAktion = actions.FindActionMap("Menu").FindAction("FadeInAktion");
        fadeInAktion.performed += StartFadeIn;

        // Bild und Text unsichtbar machen, wenn das Spiel startet
        backgroundImage.color = new Color(initialImageColor.r, initialImageColor.g, initialImageColor.b, 0f); // Alpha = 0
        backgroundText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0f);     // Alpha = 0
        //Finde alle Spriterenderer des Spielfelds
        spriteRenderers = spielfeld.GetComponentsInChildren<SpriteRenderer>().ToList();
        //Finde alle Canvasrenderer des UIs
        canvasRenderers = uI.GetComponentsInChildren<CanvasRenderer>().ToList();
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

    private void StartFadeIn(InputAction.CallbackContext context)
    {
        // Überblendung starten
        StartCoroutine(FadeIn());
        fadeInAktion.performed -= StartFadeIn;
    }

    private IEnumerator FadeIn()
    {
        // Warte die Verzögerungszeit
        yield return new WaitForSeconds(delay);

        // Führe die Überblendung durch
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration); // Fortschritt berechnen (0-1)

            // Interpoliere die Farbe für das Hintergrundbild
            backgroundImage.color = new Color(
                initialImageColor.r,
                initialImageColor.g,
                initialImageColor.b,
                Mathf.Lerp(0f, initialImageColor.a, t)
            );

            // Interpoliere die Farbe für den TMP-Text
            backgroundText.color = new Color(
                initialTextColor.r,
                initialTextColor.g,
                initialTextColor.b,
                Mathf.Lerp(0f, initialTextColor.a, t)
            );
            // Interpoliere die Farbe für alle Sprites
            foreach ( SpriteRenderer spriteRenderer in spriteRenderers )
            {
                spriteRenderer.color = new Color(
                    spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                    Mathf.Lerp(0f, 1, t));
            }
            // Interpoliere die Farbe für alle UI-Elemente
            foreach (CanvasRenderer canvasRenderer in canvasRenderers)
            {
                canvasRenderer.SetColor(new Color(
                    canvasRenderer.GetColor().r,
                    canvasRenderer.GetColor().g,
                    canvasRenderer.GetColor().b,
                    Mathf.Lerp(0f, 1, t)));
            }

            yield return null; // Warte bis zum nächsten Frame
        }

        // Sicherstellen, dass die Farben vollständig sichtbar sind
        backgroundImage.color = initialImageColor;
        backgroundText.color = initialTextColor;
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
        //Löse Verknüpfungen
        fadeInAktion.performed -= StartFadeIn;
    }
}
