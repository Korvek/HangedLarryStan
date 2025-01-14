using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZielWort : MonoBehaviour
{
    [Header("Zielwort des Levels")]
    public string zielWort;
    public string zielWort2;
    public string vorWort;
    public string vorWort2;
    public string nachWort;
    public string nachWort2;
    //public GameObject neuesZielwort;
    private string gel�stesWort;
    /// <summary>
    /// Textanzeige f�r das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortT;
    public TextMeshProUGUI vorWortT;
    public TextMeshProUGUI nachWortT;

    public GameEvent zeitBonus;
    public GameEvent zeitAbzug;
    public GameEvent wortGefunden;
    public GameEvent wortGefunden2;

    private bool erstesWortgel�st = false;
    void Awake()
    {
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //F�llen der L�sungswortanzeige mit '_'
        gel�stesWort = new string('_', zielWort.Length);
        zielwortT.text = gel�stesWort;
        vorWortT.text = vorWort;
        nachWortT.text = nachWort;
    }

    public void checkBuchstabe(char buchstabe)
    {
        //Debug.LogWarning("AAA");
        if (!zielWort.ToUpper().Contains(buchstabe))
        {
            zeitAbzug.TriggerEvent();
        }
        //Wenn der gesammelte Buchstabe im Zielwort enthalten ist
        else
        {
            //Suche nach dem Buchstaben im Zielwort
            for (int i = 0; i < zielWort.Length; i++)
            {
                if (zielWort.ToUpper()[i].Equals(buchstabe))
                {
                    //Ersetze ein _ im gel�sten Wort
                    gel�stesWort = gel�stesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gel�stesWort;
                }
            }
            zeitBonus.TriggerEvent();
            //Debug.Log(gel�stesWort);
            if (!gel�stesWort.Contains('_'))
            {
                //Debug.Log(gel�stesWort);
                wortGefunden.TriggerEvent();
                //F�llen der L�sungswortanzeige mit '_'
                zielWort = zielWort2;
                gel�stesWort = new string('_', zielWort.Length);
                zielwortT.text = gel�stesWort;
                vorWortT.text = vorWort2;
                nachWortT.text = nachWort2;
                //Debug.Log(erstesWortgel�st);
                if (erstesWortgel�st)
                {
                    wortGefunden2.TriggerEvent();
                    zielwortT.text = zielWort2;
                    vorWortT.text = vorWort2;
                    nachWortT.text = nachWort2;
                }

                erstesWortgel�st = true;
            }
        }
    }
}
