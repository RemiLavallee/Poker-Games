using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Probabilities : MonoBehaviour
{
    [SerializeField] private TMP_Text probText;
    [SerializeField] private TMP_Text betterComboText;
    private DeckManager deckManager;

    private void Awake()
    {
        deckManager = FindObjectOfType<DeckManager>();
    }

    private void DetectProbability()
    {
        List<CarteData> handData = deckManager.main.Select(x => x.Data).ToList();

        string prob = DetectHandProbability(handData);
        probText.text = prob;
    }
    
    private string DetectHandProbability(List<CarteData> handData)
    {
        handData.Sort();
        /*
        if (deckManager.IsRoyalFlush(handData)) return "ROYAL FLUSH";
        if (IsStraightFlush(handData)) return "STRAIGHT FLUSH";
        if (IsFourKind(handData)) return "FOUR OF A KIND";
        if (IsFullHouse(handData)) return "FULL HOUSE";
        if (IsFlush(handData)) return "FLUSH";
        if (IsStraight(handData)) return "STRAIGHT";
        if (IsBrelan(handData)) return "BRELAN";
        if (IsTwoPairs(handData)) return "TWO PAIRS";
        if (IsOnePair(handData)) return "PAIR";
        */
         
        return "HIGH CARD";
    }
}
