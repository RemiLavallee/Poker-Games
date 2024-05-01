using System;
using System.Collections.Generic;
using System.Linq;
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

    public void DetectProbability()
    {
        List<CarteData> handData = deckManager.main.Select(x => x.Data).ToList();

        string prob = DetectHandProbability(handData);
        probText.text = prob;
    }

    public void DetectBetterProbability()
    {
        List<CarteData> handData = deckManager.main.Select(x => x.Data).ToList();

        string prob = DetectHandBetterProbability(handData);
        betterComboText.text = prob;
    }

    private string DetectHandProbability(List<CarteData> handData)
    {
        handData.Sort();
        double royalFlushProbability = CalculateRoyalFlushProbability();
        double straightFlushProbability = CalculateStraightFlushProbability();
        double fourKindProbability = CalculateFourOfAKindProbability();
        double fullHouseProbability = CalculateFullHouseProbability();
        double flushProbability = CalculateFlushProbability();
        double straightProbability = CalculateStraightProbability();
        double threeKindProbability = CalculateThreeOfAKindProbability();
        double twoPairsProbability = CalculateTwoPairsProbability();
        double onePairProbability = CalculateOnePairProbability();
        double allHandsProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                     fullHouseProbability + flushProbability + straightProbability +
                                     threeKindProbability + twoPairsProbability + onePairProbability;
        double highCardProbability;
        if(deckManager.deck.Count == 0) highCardProbability = 0;
        else highCardProbability = 1 - allHandsProbability;


        if (deckManager.IsRoyalFlush(handData)) return (royalFlushProbability * 100).ToString("F6");
        if (deckManager.IsStraightFlush(handData)) return (straightFlushProbability * 100).ToString("F5");
        if (deckManager.IsFourKind(handData)) return (fourKindProbability * 100).ToString("F4");
        if (deckManager.IsFullHouse(handData)) return (fullHouseProbability * 100).ToString("F4");
        if (deckManager.IsFlush(handData)) return (flushProbability * 100).ToString("F4");
        if (deckManager.IsStraight(handData)) return (straightProbability * 100).ToString("F4");
        if (deckManager.IsBrelan(handData)) return (threeKindProbability * 100).ToString("F4");
        if (deckManager.IsTwoPairs(handData)) return (twoPairsProbability * 100).ToString("F4");
        if (deckManager.IsOnePair(handData)) return (onePairProbability * 100).ToString("F4");
        return (highCardProbability * 100).ToString("F4");
    }

    private string DetectHandBetterProbability(List<CarteData> handData)
    {
        handData.Sort();
        double royalFlushProbability = CalculateRoyalFlushProbability();
        double straightFlushProbability = CalculateStraightFlushProbability();
        double fourKindProbability = CalculateFourOfAKindProbability();
        double fullHouseProbability = CalculateFullHouseProbability();
        double flushProbability = CalculateFlushProbability();
        double straightProbability = CalculateStraightProbability();
        double threeKindProbability = CalculateThreeOfAKindProbability();
        double twoPairsProbability = CalculateTwoPairsProbability();
        double onePairProbability = CalculateOnePairProbability();
        double allHandsProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                     fullHouseProbability + flushProbability + straightProbability +
                                     threeKindProbability + twoPairsProbability + onePairProbability;

        double betterProbability = 0;

        if (deckManager.IsRoyalFlush(handData))
            betterProbability = 0;
        else if (deckManager.IsStraightFlush(handData))
            betterProbability = royalFlushProbability;
        else if (deckManager.IsFourKind(handData))
            betterProbability = royalFlushProbability + straightFlushProbability;
        else if (deckManager.IsFullHouse(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability;
        else if (deckManager.IsFlush(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                fullHouseProbability;
        else if (deckManager.IsStraight(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                fullHouseProbability + flushProbability;
        else if (deckManager.IsBrelan(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                fullHouseProbability + flushProbability + straightProbability;
        else if (deckManager.IsTwoPairs(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                fullHouseProbability + flushProbability + straightProbability + threeKindProbability;
        else if (deckManager.IsOnePair(handData))
            betterProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                fullHouseProbability + flushProbability + straightProbability + threeKindProbability +
                                twoPairsProbability;
        else betterProbability = allHandsProbability;

        return (betterProbability * 100).ToString("F4");
    }

    private static double Combination(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    private static double Factorial(int n)
    {
        double result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    private double CalculateRoyalFlushProbability()
    {
        Enseigne[] possibleSuits = Enum.GetValues(typeof(Enseigne)).Cast<Enseigne>().ToArray();
        int[] neededValues = { 1, 10, 11, 12, 13 };
        int royalFlushCount = 0;

        foreach (Enseigne suit in possibleSuits)
        {
            bool hasAllCards = true;
            foreach (int value in neededValues)
            {
                if (!deckManager.deck.Any(card => card.valeur == value && card.enseigne == suit))
                {
                    hasAllCards = false;
                    break;
                }
            }

            if (hasAllCards)
            {
                royalFlushCount++;
            }
        }
        
        if (royalFlushCount == 0) return 0;
        double totalHands = Combination(deckManager.deck.Count, 5);
        return (royalFlushCount / totalHands);
    }

    private double CalculateStraightFlushProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int straightFlushCount = 0;

        foreach (Enseigne enseigne in Enum.GetValues(typeof(Enseigne)))
        {
            for (int start = 1; start <= 10; start++)
            {
                int end = start + 4;
                if (deckManager.deck.Count(card =>
                        card.enseigne == enseigne && card.valeur >= start && card.valeur <= end) == 5)
                {
                    straightFlushCount++;
                }
            }
        }

        if (totalHands == 0) return 0;
        return (straightFlushCount / totalHands);
    }

    private double CalculateFourOfAKindProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int fourOfAKindCount = 0;

        for (int value = 1; value <= 13; value++)
        {
            int countOfThisValue = deckManager.deck.Count(card => card.valeur == value);

            if (countOfThisValue >= 4)
            {
                int otherCards = totalCards - 4;
                fourOfAKindCount += otherCards;
            }
        }

        if (totalHands == 0) return 0;
        return (fourOfAKindCount / totalHands);
    }

    private double CalculateFullHouseProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int fullHouseCount = 0;

        for (int tripletValue = 1; tripletValue <= 13; tripletValue++)
        {
            int countOfTripletValue = deckManager.deck.Count(card => card.valeur == tripletValue);
            if (countOfTripletValue >= 3)
            {
                for (int pairValue = 1; pairValue <= 13; pairValue++)
                {
                    if (pairValue == tripletValue) continue;
                    int countOfPairValue = deckManager.deck.Count(card => card.valeur == pairValue);
                    if (countOfPairValue >= 2)
                    {
                        int waysToChooseTriplet = (int)Combination(countOfTripletValue, 3);
                        int waysToChoosePair = (int)Combination(countOfPairValue, 2);

                        fullHouseCount += waysToChooseTriplet * waysToChoosePair;
                    }
                }
            }
        }

        if (totalHands == 0) return 0;
        return (fullHouseCount / totalHands);
    }

    private double CalculateFlushProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int flushCount = 0;

        foreach (Enseigne enseigne in Enum.GetValues(typeof(Enseigne)))
        {
            int countOfThisSuit = deckManager.deck.Count(card => card.enseigne == enseigne);
            if (countOfThisSuit >= 5)
            {
                int waysToChooseFlush = (int)Combination(countOfThisSuit, 5);
                flushCount += waysToChooseFlush;
            }
        }

        if (totalHands == 0) return 0;
        return (flushCount / totalHands);
    }

    private double CalculateStraightProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int straightCount = 0;

        for (int start = 1; start <= 10; start++)
        {
            int[] counts = new int[5];
            bool possible = true;
            for (int j = 0; j < 5; j++)
            {
                int cardValue = start + j;
                counts[j] = deckManager.deck.Count(card => card.valeur == cardValue);
                if (counts[j] == 0)
                {
                    possible = false;
                    break;
                }
            }

            if (possible)
            {
                int combinationsForThisStart = 1;
                for (int j = 0; j < 5; j++)
                {
                    combinationsForThisStart *= counts[j];
                }
                straightCount += combinationsForThisStart;
            }
        }

        if (totalHands == 0) return 0;
        return straightCount / totalHands;
    }

    private double CalculateThreeOfAKindProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int threeOfAKindCount = 0;

        int[] valueCounts = new int[14];

        foreach (var card in deckManager.deck)
        {
            valueCounts[card.valeur]++;
        }

        for (int i = 1; i <= 13; i++)
        {
            if (valueCounts[i] >= 3)
            {
                int waysToChooseThree = (int)Combination(valueCounts[i], 3);
                int remainingCards = totalCards - valueCounts[i];

                int waysToChooseTwo = (int)Combination(remainingCards, 2);

                threeOfAKindCount += waysToChooseThree * waysToChooseTwo;
            }
        }

        if (totalHands == 0) return 0;
        return threeOfAKindCount / totalHands;
    }

    private double CalculateTwoPairsProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int twoPairsCount = 0;

        int[] valueCounts = new int[14];

        foreach (var card in deckManager.deck)
        {
            valueCounts[card.valeur]++;
        }

        for (int firstValue = 1; firstValue <= 13; firstValue++)
        {
            if (valueCounts[firstValue] >= 2)
            {
                for (int secondValue = firstValue + 1; secondValue <= 13; secondValue++)
                {
                    if (valueCounts[secondValue] >= 2)
                    {
                        int waysToChooseFirstPair = (int)Combination(valueCounts[firstValue], 2);
                        int waysToChooseSecondPair = (int)Combination(valueCounts[secondValue], 2);

                        int remainingCards = totalCards - valueCounts[firstValue] - valueCounts[secondValue];
                        int waysToChooseFifthCard = remainingCards;

                        twoPairsCount += waysToChooseFirstPair * waysToChooseSecondPair * waysToChooseFifthCard;
                    }
                }
            }
        }

        if (totalHands == 0) return 0;
        return twoPairsCount / totalHands;
    }

    private double CalculateOnePairProbability()
    {
        int totalCards = deckManager.deck.Count;
        if (totalCards < 5) return 0;

        double totalHands = Combination(totalCards, 5);
        int onePairCount = 0;

        for (int value = 1; value <= 13; value++)
        {
            int cardsWithValue = deckManager.deck.Count(c => c.valeur == value);

            if (cardsWithValue >= 2)
            {
                double pairCombinations = Combination(cardsWithValue, 2);
                double remainingCardsCount = totalCards - cardsWithValue;
                double otherCombinations = 0;

                if (remainingCardsCount >= 3)
                {
                    otherCombinations = (int)Combination((int)remainingCardsCount, 3);
                    for (int otherValue = 1; otherValue <= 13; otherValue++)
                    {
                        if (otherValue != value)
                        {
                            int otherCardsWithSameValue = deckManager.deck.Count(c => c.valeur == otherValue);
                            if (otherCardsWithSameValue >= 2)
                            {
                                double secondPairCombinations = Combination(otherCardsWithSameValue, 2);
                                otherCombinations -= secondPairCombinations * (remainingCardsCount - otherCardsWithSameValue);
                            }
                        }
                    }
                }
                
                onePairCount += (int)pairCombinations * (int)otherCombinations;
            }
        }
            
        if (totalHands == 0) return 0;
        return onePairCount / totalHands;
    }
}

// Premiere essaie avec les probabilité sans prendre en compte les cartes restantes dans le deck
/*
    private string DetectHandProbability(List<CarteData> handData)
    {
        handData.Sort();
        double totalHands = Combination(deckManager.deck.Count, 5);
        double royalFlushProbability = 4.0 / totalHands;
        double straightFlushProbability = 36.0 / totalHands;
        double fourKindProbability = 13.0 * Combination(48,1) / totalHands;
        double fullHouseProbability = 13.0 * Combination(4,3) * 12.0 * Combination(4,2) / totalHands;
        double flushProbability = (Combination(13,5) * 4.0 - straightFlushProbability) / totalHands;
        double straightProbability = (10 * Math.Pow(4,5) - straightFlushProbability) / totalHands;
        double threeKindProbability = (13.0 * Combination(4,3) * Combination(49,2)) / totalHands;
        double twoPairsProbability = (Combination(13,2) * Combination(4,2) * Combination(4,2) * 11 * 4) / totalHands;
        double onePairProbability = (13 * Combination(4,2) * Combination(12,3) * Math.Pow(4,3)) / totalHands;
        double allHandsProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                     fullHouseProbability + flushProbability + straightProbability +
                                     threeKindProbability + twoPairsProbability + onePairProbability;
        double highCardProbability = 1 - allHandsProbability;
        
        
        if (deckManager.IsRoyalFlush(handData)) return (royalFlushProbability * 100).ToString("F6");
        if (deckManager.IsStraightFlush(handData)) return (straightFlushProbability * 100).ToString("F5");
        if (deckManager.IsFourKind(handData)) return (fourKindProbability * 100).ToString("F4");
        if (deckManager.IsFullHouse(handData)) return (fullHouseProbability * 100).ToString("F4");
        if (deckManager.IsFlush(handData)) return (flushProbability * 100).ToString("F4");
        if (deckManager.IsStraight(handData)) return (straightProbability * 100).ToString("F4");
        if (deckManager.IsBrelan(handData)) return (threeKindProbability * 100).ToString("F4");
        if (deckManager.IsTwoPairs(handData)) return (twoPairsProbability * 100).ToString("F4");
        if (deckManager.IsOnePair(handData)) return (onePairProbability * 100).ToString("F4");
        return (highCardProbability * 100).ToString("F4");
    }
    
        private string DetectHandBetterProbability(List<CarteData> handData)
    {
        handData.Sort();
        double totalHands = Combination(deckManager.deck.Count, 5);
        double royalFlushProbability = 4.0 / totalHands;
        double straightFlushProbability = 36.0 / totalHands;
        double fourKindProbability = 13.0 * Combination(48,1) / totalHands;
        double fullHouseProbability = 13.0 * Combination(4,3) * 12.0 * Combination(4,2) / totalHands;
        double flushProbability = (Combination(13,5) * 4.0 - straightFlushProbability) / totalHands;
        double straightProbability = (10 * Math.Pow(4,5) - straightFlushProbability) / totalHands;
        double threeKindProbability = (13.0 * Combination(4,3) * Combination(49,2)) / totalHands;
        double twoPairsProbability = (Combination(13,2) * Combination(4,2) * Combination(4,2) * 11 * 4) / totalHands;
        double onePairProbability = (13 * Combination(4,2) * Combination(12,3) * Math.Pow(4,3)) / totalHands;
        double allHandsProbability = royalFlushProbability + straightFlushProbability + fourKindProbability +
                                     fullHouseProbability + flushProbability + straightProbability +
                                     threeKindProbability + twoPairsProbability + onePairProbability;
        double highCardProbability = 1 - allHandsProbability;
        
        
        if (deckManager.IsRoyalFlush(handData)) return (royalFlushProbability * 100).ToString("F6");
        if (deckManager.IsStraightFlush(handData)) return (straightFlushProbability * 100).ToString("F5");
        if (deckManager.IsFourKind(handData)) return (fourKindProbability * 100).ToString("F4");
        if (deckManager.IsFullHouse(handData)) return (fullHouseProbability * 100).ToString("F4");
        if (deckManager.IsFlush(handData)) return (flushProbability * 100).ToString("F4");
        if (deckManager.IsStraight(handData)) return (straightProbability * 100).ToString("F4");
        if (deckManager.IsBrelan(handData)) return (threeKindProbability * 100).ToString("F4");
        if (deckManager.IsTwoPairs(handData)) return (twoPairsProbability * 100).ToString("F4");
        if (deckManager.IsOnePair(handData)) return (onePairProbability * 100).ToString("F4");
        return (highCardProbability * 100).ToString("F4");
    }
    
   - PROBABILITER 4ième As
    
    static void Main(string[] args)
{
    int remainingCards = 47; 
    int acesLeft = 1; 

    double probabilityFirstCardAce = (double)acesLeft / remainingCards;
    double probabilitySecondCardAce = (double)acesLeft / (remainingCards - 1); 
    double combinedProbability = probabilityFirstCardAce + (1 - probabilityFirstCardAce) * probabilitySecondCardAce;

    Console.WriteLine($"La probabilité d'obtenir un quatrième as est de {combinedProbability:P2}.");
}
*/