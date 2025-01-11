using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Namespace für TextMeshPro

public class BackgroundFader : MonoBehaviour
{
    public Image backgroundImage; // Das UI-Image-Element für den Hintergrund
    public TextMeshProUGUI backgroundText; // TMP-Text für den Hintergrundtext
    public float fadeDuration = 0.5f; // Dauer der Überblendung
    public float delay = 0.5f; // Verzögerung vor der Überblendung

    private Color initialImageColor;
    private Color initialTextColor;

    void Start()
    {
        // Speichere die ursprünglichen Farben
        initialImageColor = backgroundImage.color;
        initialTextColor = backgroundText.color;

        // Bild und Text unsichtbar machen, wenn das Spiel startet
        backgroundImage.color = new Color(initialImageColor.r, initialImageColor.g, initialImageColor.b, 0f); // Alpha = 0
        backgroundText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0f);     // Alpha = 0
    }

    void Update()
    {
        // Überblendung starten, wenn Leertaste gedrückt wird
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeIn());
        }
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

            yield return null; // Warte bis zum nächsten Frame
        }

        // Sicherstellen, dass die Farben vollständig sichtbar sind
        backgroundImage.color = initialImageColor;
        backgroundText.color = initialTextColor;
    }
}
