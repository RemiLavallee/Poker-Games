using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CarteManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerComboText;
    [SerializeField] private TMP_Text enemyComboText;
    [SerializeField] private Carte[] main = new Carte[5];
    [SerializeField] private Carte[] enemyHand = new Carte[5];
    [SerializeField] private Button dealButton;
    [SerializeField] private Button drawButton;
    [SerializeField] private Button startButton;
    private List<CarteData> deck = new List<CarteData>(52);
    private int indexCarte = 0;
    private int hitDraw = 2;
    private bool isFirstDraw = true;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                deck.Add(new CarteData(j, (Enseigne)i));
            }
        }
        
        MixDeck();

        ButtonStartGame();
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
        isFirstDraw = false;

        if (hitDraw == 1)
        {
            drawButton.enabled = false;
            DownOpacColor(drawButton);
        }
        
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
        
        string playerCombo = DetectPokerHandCombo(handData);
        playerComboText.text = playerCombo;
    }
    
    private void DetectEnemyComboPoker()
    {
        List<CarteData> enemyHandData = enemyHand.Select(x => x.Data).ToList();
        
         string enemyCombo = DetectPokerHandCombo(enemyHandData);
         enemyComboText.text = enemyCombo;
    }

    public void Deal()
    {
        hitDraw = 2;
        dealButton.enabled = false;
        DownOpacColor(dealButton);
        drawButton.enabled = false;
        DownOpacColor(drawButton);
        
        DetectEnemyComboPoker();
            
        for (int i = 0; i < 5; i++)
        {
            enemyHand[i].SetData(deck[indexCarte++]);
            if (indexCarte >= deck.Count)
            {
                indexCarte = 0;
                MixDeck();
            }
        }
        
        DetectComboPoker();

        StartCoroutine(ResetCardSprites());
    }

    public void StartGame()
    {
        if(isFirstDraw) Piger();
        isFirstDraw = false;
        DownOpacColor(startButton);
        ResetOpacColor(drawButton);
        ResetOpacColor(dealButton);
        startButton.enabled = false;
        drawButton.enabled = true;
        dealButton.enabled = true;
    }

    public void HitDraw()
    {
        hitDraw--;
    }

    private bool IsRoyalFlush(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        foreach (var card in hand)
        {
            if (card == null || !new HashSet<int> { 10, 11, 12, 13, 1 }.Contains(card.valeur)) return false;
        }
        return IsFlush(hand);
    }
    
    private bool IsStraightFlush(List<CarteData> hand)
    {
        return IsFlush(hand) && IsStraight(hand);
    }
    
    private bool IsFourKind(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        if (hand[0] == null || hand[3] == null || hand[1] == null || hand[4] == null) return false;
        
        return (hand[0].valeur == hand[3].valeur || hand[1].valeur == hand[4].valeur);
    }
    
    private bool IsFullHouse(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        if (hand[0] == null || hand[1] == null || hand[2] == null || hand[3] == null || hand[4] == null) return false;
        
        return (hand[0].valeur == hand[2].valeur && hand[3].valeur == hand[4].valeur) ||
               (hand[0].valeur == hand[1].valeur && hand[2].valeur == hand[4].valeur);
    }
    
    private bool IsFlush(List<CarteData> hand)
    {
        if (hand == null || hand.Count == 0 || hand[0] == null || hand[0].enseigne == null)
        {
            return false;
        }

        for (int i = 1; i < hand.Count; i++)
        {
            if (hand[i] == null || hand[i].enseigne == null || hand[i].enseigne != hand[0].enseigne)
            {
                return false;
            }
        }
        
        return true;
    }
    
    private bool IsStraight(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i] == null || hand[i + 1] == null || hand[i].valeur + 1 != hand[i + 1].valeur)
                return false;
        }
        return true;
    }
    
    private bool IsBrelan(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 3) return false;
        for (int i = 0; i < hand.Count - 2; i++)
        {
            if (hand[i] == null || hand[i + 1] == null || hand[i + 2] == null)
                return false;
            if (hand[i].valeur == hand[i + 1].valeur && hand[i].valeur == hand[i + 2].valeur)
                return true;
        }
        return false;
    }
    
    private bool IsTwoPairs(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 4) return false;
        int pairCount = 0;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i] == null || hand[i + 1] == null) continue;
            if (hand[i].valeur == hand[i + 1].valeur)
            {
                pairCount++;
                i++; 
            }
        }
        return pairCount == 2;
    }
    
    private bool IsOnePair(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 2) return false;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i] == null || hand[i + 1] == null) continue;
            if (hand[i].valeur == hand[i + 1].valeur)
                return true;
        }
        return false;
    }
    
    private string DetectPokerHandCombo(List<CarteData> handData)
    {
        handData.Sort();

        if (IsRoyalFlush(handData)) return "ROYAL FLUSH";
        if (IsStraightFlush(handData)) return "STRAIGHT FLUSH";
        if (IsFourKind(handData)) return "FOUR OF A KIND";
        if (IsFullHouse(handData)) return "FULL HOUSE";
        if (IsFlush(handData)) return "FLUSH";
        if (IsStraight(handData)) return "STRAIGHT";
        if (IsBrelan(handData)) return "BRELAN";
        if (IsTwoPairs(handData)) return "TWO PAIRS";
        if (IsOnePair(handData)) return "PAIR";
    
        return "HIGH CARD";
    }

    private IEnumerator ResetCardSprites()
    {
        yield return new WaitForSeconds(2f);

        foreach (var card in enemyHand)
        {
            card.ShowBackCard();
        }
        
        foreach (var card in main)
        {
            card.ShowBackCard();
        }
        
        isFirstDraw = true;
        playerComboText.text = "";
        enemyComboText.text = "";
        ButtonStartGame();
    }

    private void DownOpacColor(Button button)
    {
        Color newColor = new Color(1f, 1f, 1f, 0.7f);
        button.GetComponent<Image>().color = newColor;
    }
    
    private void ResetOpacColor(Button button)
    {
        Color newColor = new Color(1f, 1f, 1f, 1f);
        button.GetComponent<Image>().color = newColor;
    }

    private void ButtonStartGame()
    {
        dealButton.enabled = false;
        drawButton.enabled = false;
        startButton.enabled = true;
        DownOpacColor(dealButton);
        DownOpacColor(drawButton);
        ResetOpacColor(startButton);
    }
}
