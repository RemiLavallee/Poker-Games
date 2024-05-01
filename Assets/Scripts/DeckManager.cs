using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public List<CarteData> deck = new List<CarteData>();
    [SerializeField] private List<CarteStats> cardDisplays;
    public CarteStats[] main = new CarteStats[5];
    [SerializeField] private GameObject[] mainPanel;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text cardCount;
    public int heldCount;
    private Probabilities prob;

    private void Awake()
    {
       prob = FindObjectOfType<Probabilities>();
    }

    private void Start()
    {
        CreateDeck();
        if (cardDisplays.Count >= 52)
        {
            AssignCardsToDisplays();
        }
        cardCount.text = deck.Count.ToString();
    }

    private void Update()
    {
        cardCount.text = deck.Count.ToString();
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
    
    private void DetectComboPoker()
    {
        List<CarteData> handData = main.Select(x => x.Data).ToList();

        string playerCombo = DetectPokerHandCombo(handData);
        comboText.text = playerCombo;
    }

    public bool IsRoyalFlush(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        foreach (var card in hand)
        {
            if (card == null || !new HashSet<int> { 10, 11, 12, 13, 1 }.Contains(card.valeur)) return false;
        }

        return IsFlush(hand);
    }

    public bool IsStraightFlush(List<CarteData> hand)
    {
        return IsFlush(hand) && IsStraight(hand);
    }

    public bool IsFourKind(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        if (hand[0] == null || hand[3] == null || hand[1] == null || hand[4] == null) return false;

        return (hand[0].valeur == hand[3].valeur || hand[1].valeur == hand[4].valeur);
    }

    public bool IsFullHouse(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        if (hand[0] == null || hand[1] == null || hand[2] == null || hand[3] == null || hand[4] == null) return false;

        return (hand[0].valeur == hand[2].valeur && hand[3].valeur == hand[4].valeur) ||
               (hand[0].valeur == hand[1].valeur && hand[2].valeur == hand[4].valeur);
    }

    public bool IsFlush(List<CarteData> hand)
    {
        if (hand == null || hand.Count == 0 || hand[0] == null)
        {
            return false;
        }

        for (int i = 1; i < hand.Count; i++)
        {
            if (hand[i] == null || hand[i].enseigne != hand[0].enseigne)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsStraight(List<CarteData> hand)
    {
        if (hand == null || hand.Count < 5) return false;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i] == null || hand[i + 1] == null || hand[i].valeur + 1 != hand[i + 1].valeur)
                return false;
        }

        return true;
    }

    public bool IsBrelan(List<CarteData> hand)
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

    public bool IsTwoPairs(List<CarteData> hand)
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

    public bool IsOnePair(List<CarteData> hand)
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

    public void AddCardHand()
    {
        foreach (var card in cardDisplays)
        {
            if (card.IsHeld && heldCount == 5)
            {
                for (int i = 0; i < main.Length; i++)
                {
                    if (main[i] == null)
                    {
                        CarteStats newCard = Instantiate(card, mainPanel[i].transform, false);
                        newCard.transform.localPosition = Vector3.zero; 
                        newCard.Data = new CarteData(card.Data.valeur, card.Data.enseigne);
                        main[i] = newCard;
                        break;
                    }
                }
            }
            else if(card.IsHeld && heldCount > 5)
            {
                return;
            }
            else if(card.IsHeld && heldCount < 5)
            {
                return;
            }
            
            card.IsHeld = false;
            card.heldImage.SetActive(false);
        }

        UpdateCard();
        DetectComboPoker();
        prob.DetectProbability();
        prob.DetectBetterProbability();
        heldCount = 0;
    }

    private void UpdateCard()
    {
        for (int i = 0; i < main.Length; i++)
        {
            if (main[i] != null)
            {
                mainPanel[i].SetActive(true);
                CarteStats cardStats = mainPanel[i].GetComponent<CarteStats>();
                if (cardStats != null)
                {
                    cardStats.SetData(main[i].Data);
                }
            }
            else
            {
                mainPanel[i].SetActive(false);
            }
        }
    }

    public void RemoveCardHand()
    {
        for (int i = 0; i < main.Length; i++)
        {
            if (main[i] != null && main[i].IsHeld)
            {
                Destroy(main[i].gameObject);
                main[i] = null;
            }
        }
    }

    public void ResetCardHand()
    {
        for (int i = 0; i < main.Length; i++)
        {
            if (main[i] != null)
            {
                Destroy(main[i].gameObject);
                main[i] = null;
            }
        }
    }

    public void RemoveCardDeck()
    {
        for (int i = cardDisplays.Count - 1; i >= 0; i--)
        {
            var cardDisplay = cardDisplays[i];
            if (cardDisplay.IsHeld)
            {
                deck.Remove(cardDisplay.Data);
                var imageComponent = cardDisplay.GetComponent<Image>();
                if (imageComponent != null)
                {
                    float originalAlpha = imageComponent.color.a;
                    imageComponent.color = new Color(0, 0, 0, originalAlpha);
                }
                
                cardDisplay.IsHeld = false;
                cardDisplay.heldImage.SetActive(false);
                cardDisplay.IsActive = false;
            }
        }
        
        heldCount = 0;
    }

    public void AddCardDeck()
    {
        for (int i = cardDisplays.Count - 1; i >= 0; i--)
        {
            var cardDisplay = cardDisplays[i];
            if (cardDisplay.IsHeld && !cardDisplay.IsActive)
            {
                deck.Add(cardDisplay.Data);
                var imageComponent = cardDisplay.GetComponent<Image>();
                if (imageComponent != null)
                {
                    float originalAlpha = imageComponent.color.a;
                    imageComponent.color = new Color(1f, 1f, 1f, originalAlpha);
                }
                
                cardDisplay.IsHeld = false;
                cardDisplay.heldImage.SetActive(false);
                cardDisplay.IsActive = true;
            }

            if (cardDisplay.IsHeld && cardDisplay.IsActive)
            {
                cardDisplay.IsHeld = false;
                cardDisplay.heldImage.SetActive(false);
            }
        }

        heldCount = 0;
    }

    public void ResetCardDeck()
    {
        for (int i = cardDisplays.Count - 1; i >= 0; i--)
        {
            var cardDisplay = cardDisplays[i];
            if (!cardDisplay.IsActive)
            {
                deck.Add(cardDisplay.Data);
                var imageComponent = cardDisplay.GetComponent<Image>();
                if (imageComponent != null)
                {
                    float originalAlpha = imageComponent.color.a;
                    imageComponent.color = new Color(1f, 1f, 1f, originalAlpha);
                }
                
                cardDisplay.IsHeld = false;
                cardDisplay.heldImage.SetActive(false);
                cardDisplay.IsActive = true;
            }
        }

        heldCount = 0;
    }
}
