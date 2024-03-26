using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarteManager : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private Carte[] main = new Carte[5];
    private List<CarteData> deck = new List<CarteData>(52);
    private int indexCarte = 0;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                deck.Add(new CarteData(j, (Enseigne)i));
            }
        }

        // Melanger le deck
        MixDeck();
        
        Piger();
    }

    private void MixDeck()
    {
        var temp = new List<CarteData>();
        while (deck.Count > 0)
        {
            int indexRandom = Random.Range(0, deck.Count);
            temp.Add(deck[indexRandom]);
            deck.RemoveAt(indexRandom);
        }

        deck = temp;
    }
    public void Piger()
    {
        for (int i = 0; i < 5; i++)
        {
            if (main[i].IsHeld)
            {
                continue;
            }
            
            main[i].SetData(deck[indexCarte++]);
            if (indexCarte >= deck.Count)
            {
                indexCarte = 0;
                MixDeck();
            }
        }

        for (int i = 0; i < 5; i++)
        {
            main[i].ClearHeld();
        }

        DetectComboPoker();
    }

    private void DetectComboPoker()
    {
        List<CarteData> handData = main.Select(x => x.Data).ToList();
        handData.Sort();

        if (handData[0].enseigne == handData[1].enseigne &&
            handData[0].enseigne == handData[2].enseigne &&
            handData[0].enseigne == handData[3].enseigne &&
            handData[0].enseigne == handData[4].enseigne)
        {
            comboText.text = "FLUSH";
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (handData[i].valeur == handData[i + 1].valeur)
            {
                comboText.text = "PAIR";
                return;
            }
        }

        comboText.text = "HIGH CARD";
    }
}
