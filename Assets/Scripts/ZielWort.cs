using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZielWort : MonoBehaviour
{
    [Header("Zielwort des Levels")]
    public string zielwort;
    private string gel�stesWort;
    /// <summary>
    /// Textanzeige f�r das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortText;

    public GameEvent zeitBonus;
    public GameEvent zeitAbzug;
    public GameEvent wortGefunden;
    void Awake()
    {
        zielwortText = gameObject.GetComponent<TextMeshProUGUI>();
        //F�llen der L�sungswortanzeige mit '_'
        gel�stesWort = new string('_', zielwort.Length);
        zielwortText.text = gel�stesWort;
    }

    public void checkBuchstabe(char buchstabe)
    {
        if (!zielwort.ToUpper().Contains(buchstabe))
        {
            zeitAbzug.TriggerEvent();
        }
        //Wenn der gesammelte Buchstabe im Zielwort enthalten ist
        else
        {
            //Suche nach dem Buchstaben im Zielwort
            for (int i = 0; i < zielwort.Length; i++)
            {
                if (zielwort.ToUpper()[i].Equals(buchstabe))
                {
                    //Ersetze ein _ im gel�sten Wort
                    gel�stesWort = gel�stesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortText.text = gel�stesWort;                    
                }
            }
            zeitBonus.TriggerEvent();
            if(!gel�stesWort.Contains('_'))
            {
                wortGefunden.TriggerEvent();
            }
        }
    }
}
