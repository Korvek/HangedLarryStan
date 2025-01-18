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
    /// Satzstück vor dem jeweiligen Zielwort
    /// </summary>
    public string vorWort;
    public string vorWort2;
    /// <summary>
    /// Satzstück nach dem jeweiligen Zielwort
    /// </summary>
    public string nachWort;
    public string nachWort2;
    /// <summary>
    /// Textelement für Vorwort
    /// </summary>
    public TextMeshProUGUI vorWortT;
    /// <summary>
    /// Textelement für Nachwort
    /// </summary>
    public TextMeshProUGUI nachWortT;

    /// <summary>
    /// Event für Zeitbonus
    /// </summary>
    public GameEvent zeitBonus;
    /// <summary>
    /// Event für Zeitabzug
    /// </summary>
    public GameEvent zeitAbzug;
    /// <summary>
    /// Event für erstes gefundenes Wort
    /// </summary>
    public GameEvent wortGefunden;
    /// <summary>
    /// Event für zweites gefundenes Wort
    /// </summary>
    public GameEvent wortGefunden2;
    /// <summary>
    /// Event für richtigen Buchstaben
    /// </summary>
    public GameEvent buchstabeRichtig;
    /// <summary>
    /// Event für falschen Buchstaben
    /// </summary>
    public GameEvent buchstabeFalsch;

    //String zum speichern der Lösung
    private string gelöstesWort;
    /// <summary>
    /// Textanzeige für das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortT;
    //Ob das erste Zielwort gelöst worden ist
    private bool erstesWortgelöst = false;
    void Awake()
    {
        //Finde Textkomponente
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //Füllen der Lösungswortanzeige mit '_'
        gelöstesWort = new string('_', zielWort.Length);
        //Setzte Textelemente zu Beginn
        zielwortT.text = gelöstesWort;
        vorWortT.text = vorWort;
        nachWortT.text = nachWort;
    }
    /// <summary>
    /// Prüft, ob der Buchstabe im aktuellen Lösungswort ist
    /// </summary>
    /// <param name="buchstabe">Zu prüfender Buchstabe</param>
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
                    gelöstesWort = gelöstesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gelöstesWort;
                }
            }
            //Gib Bonus Zeit
            
            //Wenn keine _ mehr im gelösten Wort übrig sind
            if (!gelöstesWort.Contains('_'))
            {
                zeitBonus.TriggerEvent();
                //Löse ein Wort gefunden Event aus
                wortGefunden.TriggerEvent();
                //Lade das zweite Zielwort
                zielWort = zielWort2;
                //Fülle das gelöste Wort mit _
                gelöstesWort = new string('_', zielWort.Length);
                //Textelemente neu setzen
                zielwortT.text = gelöstesWort;
                vorWortT.text = vorWort2;
                nachWortT.text = nachWort2;
                //Wenn das erste Wort gelöst war
                if (erstesWortgelöst)
                {
                    //Löse das zweite Wort gefunden Event aus
                    wortGefunden2.TriggerEvent();
                    zielwortT.text = zielWort2;
                    vorWortT.text = vorWort2;
                    nachWortT.text = nachWort2;
                }
                //Setze den erstes Wort gelöst Marker
                erstesWortgelöst = true;
            }
            else
            {
                buchstabeRichtig.TriggerEvent();
            }
        }
    }
}
