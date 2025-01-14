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
    public GameEvent wortGefunden2;

    private bool erstesWortgelöst = false;
    void Awake()
    {
        zielwortT = gameObject.GetComponent<TextMeshProUGUI>();
        //Füllen der Lösungswortanzeige mit '_'
        gelöstesWort = new string('_', zielWort.Length);
        zielwortT.text = gelöstesWort;
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
                    //Ersetze ein _ im gelösten Wort
                    gelöstesWort = gelöstesWort.Remove(i, 1).Insert(i, buchstabe.ToString());
                    zielwortT.text = gelöstesWort;
                }
            }
            zeitBonus.TriggerEvent();
            //Debug.Log(gelöstesWort);
            if (!gelöstesWort.Contains('_'))
            {
                //Debug.Log(gelöstesWort);
                wortGefunden.TriggerEvent();
                //Füllen der Lösungswortanzeige mit '_'
                zielWort = zielWort2;
                gelöstesWort = new string('_', zielWort.Length);
                zielwortT.text = gelöstesWort;
                vorWortT.text = vorWort2;
                nachWortT.text = nachWort2;
                //Debug.Log(erstesWortgelöst);
                if (erstesWortgelöst)
                {
                    wortGefunden2.TriggerEvent();
                    zielwortT.text = zielWort2;
                    vorWortT.text = vorWort2;
                    nachWortT.text = nachWort2;
                }

                erstesWortgelöst = true;
            }
        }
    }
}
