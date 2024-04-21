using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<CarteData> deck = new List<CarteData>();
    [SerializeField] private List<CarteStats> cardDisplays;
    [SerializeField] private Carte[] main = new Carte[5];
    [SerializeField] private TMP_Text comboText;
    
    private void Start()
    {
        CreateDeck();
        if (cardDisplays.Count >= 52)
        {
            AssignCardsToDisplays();
        }
    }

    private void CreateDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                deck.Add(new CarteData(j, (Enseigne)i));
            }
        }
    }
    
    private void AssignCardsToDisplays()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            cardDisplays[i].SetData(deck[i]);
        }
    }
}
