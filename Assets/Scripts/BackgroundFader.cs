using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Namespace f�r TextMeshPro

public class BackgroundFader : MonoBehaviour
{
    public Image backgroundImage; // Das UI-Image-Element f�r den Hintergrund
    public TextMeshProUGUI backgroundText; // TMP-Text f�r den Hintergrundtext
    public float fadeDuration = 0.5f; // Dauer der �berblendung
    public float delay = 0.5f; // Verz�gerung vor der �berblendung

    private Color initialImageColor;
    private Color initialTextColor;

    void Start()
    {
        // Speichere die urspr�nglichen Farben
        initialImageColor = backgroundImage.color;
        initialTextColor = backgroundText.color;

        // Bild und Text unsichtbar machen, wenn das Spiel startet
        backgroundImage.color = new Color(initialImageColor.r, initialImageColor.g, initialImageColor.b, 0f); // Alpha = 0
        backgroundText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0f);     // Alpha = 0
    }

    void Update()
    {
        // �berblendung starten, wenn Leertaste gedr�ckt wird
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeIn());
        }
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

            // Interpoliere die Farbe f�r das Hintergrundbild
            backgroundImage.color = new Color(
                initialImageColor.r,
                initialImageColor.g,
                initialImageColor.b,
                Mathf.Lerp(0f, initialImageColor.a, t)
            );

            // Interpoliere die Farbe f�r den TMP-Text
            backgroundText.color = new Color(
                initialTextColor.r,
                initialTextColor.g,
                initialTextColor.b,
                Mathf.Lerp(0f, initialTextColor.a, t)
            );

            yield return null; // Warte bis zum n�chsten Frame
        }

        // Sicherstellen, dass die Farben vollst�ndig sichtbar sind
        backgroundImage.color = initialImageColor;
        backgroundText.color = initialTextColor;
    }
}
