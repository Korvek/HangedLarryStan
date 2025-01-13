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
    void Awake()
    {
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //F�llen der L�sungswortanzeige mit '_'
        gel�stesWort = new string('_', zielWort.Length);
        zielwortT.text = gel�stesWort;
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
                    //Ersetze ein _ im gel�sten Wort
                    gel�stesWort = gel�stesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gel�stesWort;                    
                }
            }
            zeitBonus.TriggerEvent();
            if(!gel�stesWort.Contains('_'))
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
