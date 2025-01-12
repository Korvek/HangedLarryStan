using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZielWort : MonoBehaviour
{
    [Header("Zielwort des Levels")]
    public string zielwort;
    private string gelöstesWort;
    /// <summary>
    /// Textanzeige für das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortText;

    public GameEvent zeitBonus;
    public GameEvent zeitAbzug;
    public GameEvent wortGefunden;
    void Awake()
    {
        zielwortText = gameObject.GetComponent<TextMeshProUGUI>();
        //Füllen der Lösungswortanzeige mit '_'
        gelöstesWort = new string('_', zielwort.Length);
        zielwortText.text = gelöstesWort;
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
                    //Ersetze ein _ im gelösten Wort
                    gelöstesWort = gelöstesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortText.text = gelöstesWort;                    
                }
            }
            zeitBonus.TriggerEvent();
            if(!gelöstesWort.Contains('_'))
            {
                wortGefunden.TriggerEvent();
            }
        }
    }
}
