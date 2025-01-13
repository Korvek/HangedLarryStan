using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ZielWort : MonoBehaviour
{
    [Header("Zielwort des Levels")]
    public string zielWort;
    public string vorWort;
    public string nachWort;
    //public GameObject neuesZielwort;
    private string gelöstesWort;
    /// <summary>
    /// Textanzeige für das Zielwort
    /// </summary>
    private TextMeshProUGUI zielwortT;
    public TextMeshProUGUI vorWortT;
    public TextMeshProUGUI nachWortT;

    public GameEvent zeitBonus;
    public GameEvent zeitAbzug;
    public GameEvent wortGefunden;
    void Awake()
    {
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //Füllen der Lösungswortanzeige mit '_'
        gelöstesWort = new string('_', zielWort.Length);
        zielwortT.text = gelöstesWort;
    }

    public void checkBuchstabe(char buchstabe)
    {
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
                    //Ersetze ein _ im gelösten Wort
                    gelöstesWort = gelöstesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gelöstesWort;                    
                }
            }
            zeitBonus.TriggerEvent();
            if(!gelöstesWort.Contains('_'))
            {

                wortGefunden.TriggerEvent();
                //if (neuesZielwort!=null)
                //{
                //    neuesZielwort.SetActive(true);
                //    this.gameObject.SetActive(false);
                //}
            }
        }
    }
}
