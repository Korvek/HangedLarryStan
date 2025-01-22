using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZielWort : MonoBehaviour
{
    /// <summary>
    /// Das erste Zielwort des Levels
    /// </summary>
    [Header("Zielwort des Levels")]
    public string zielWort;
    /// <summary>
    /// Das zweite Zielwort des Levels
    /// </summary>
    [Header("Zweites Zielwort des Levels")]
    public string zielWort2;
    /// <summary>
    /// Satzst�ck vor dem jeweiligen Zielwort
    /// </summary>
    public string vorWort;
    public string vorWort2;
    /// <summary>
    /// Satzst�ck nach dem jeweiligen Zielwort
    /// </summary>
    public string nachWort;
    public string nachWort2;
    /// <summary>
    /// Textelement f�r Vorwort
    /// </summary>
    public TextMeshProUGUI vorWortT;
    /// <summary>
    /// Textelement f�r Nachwort
    /// </summary>
    public TextMeshProUGUI nachWortT;

    /// <summary>
    /// Event f�r Zeitbonus
    /// </summary>
    public GameEvent zeitBonus;
    public GameEventFloat zeitBonusZahl;
    /// <summary>
    /// Event f�r Zeitabzug
    /// </summary>
    public GameEvent zeitAbzug;
    /// <summary>
    /// Event f�r erstes gefundenes Wort
    /// </summary>
    public GameEvent wortGefunden;
    /// <summary>
    /// Event f�r richtigen Buchstaben
    /// </summary>
    public GameEvent buchstabeRichtig;
    /// <summary>
    /// Event f�r falschen Buchstaben
    /// </summary>
    public GameEvent buchstabeFalsch;
    public float bonusZeit;
    //String zum speichern der L�sung
    private string gel�stesWort;
    /// <summary>
    /// Textanzeige f�r das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortT;
    void Awake()
    {
        //Finde Textkomponente
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //F�llen der L�sungswortanzeige mit '_'
        gel�stesWort = new string('_', zielWort.Length);
        //Setzte Textelemente zu Beginn
        zielwortT.text = gel�stesWort;
        vorWortT.text = vorWort;
        nachWortT.text = nachWort;
    }
    /// <summary>
    /// Pr�ft, ob der Buchstabe im aktuellen L�sungswort ist
    /// </summary>
    /// <param name="buchstabe">Zu pr�fender Buchstabe</param>
    public void checkBuchstabe(char buchstabe)
    {
        //Wenn der Buchstabe nicht im Zielwort enthalten ist
        if (!zielWort.ToUpper().Contains(buchstabe))
        {
            //Ziehe Zeit ab
            zeitAbzug.TriggerEvent();
            buchstabeFalsch.TriggerEvent();
        }
        //Wenn der gesammelte Buchstabe im Zielwort enthalten ist
        else
        {
            //Suche nach dem Buchstaben im Zielwort
            for (int i = 0; i < zielWort.Length; i++)
            {
                //Wenn der Buchstabe gefunden wurde
                if (zielWort.ToUpper()[i].Equals(buchstabe))
                {
                    //Ersetze ein _ an der passenden Stelle
                    gel�stesWort = gel�stesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gel�stesWort;
                }
            }
            //Wenn keine _ mehr im gel�sten Wort �brig sind
            if (!gel�stesWort.Contains('_'))
            {
                //L�se ein Wort gefunden Event aus
                wortGefunden.TriggerEvent();
                zeitBonusZahl.TriggerEvent(bonusZeit);

            }
            else
            {
                buchstabeRichtig.TriggerEvent();
                zeitBonusZahl.TriggerEvent(bonusZeit);
            }
        }
    }
}
