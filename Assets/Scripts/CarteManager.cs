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
    [SerializeField] private PlayerHealth player;
    [SerializeField] private EnemyHealth enemy;
    private List<CarteData> deck = new List<CarteData>(52);
    private int indexCarte = 0;
    public int hitDraw = 2;
    private bool isFirstDraw = true;
    public int attackPower;
    [SerializeField] private ItemsScriptable itemsScriptable;
    private AudioManager audioManager;
    [SerializeField] private GameObject sword1;
    [SerializeField] private GameObject sword2;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private Inventory inventory;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        LoadAndSave.instance.LoadData();
        coinsText.text = inventory.coinsCount.ToString();
        
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
            
            StartCoroutine(FlipCard(main));

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
        audioManager.PlaySound(audioManager.hitDamage);
        StartCoroutine(AnimateSwords());
        
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

        attackPower = 10;

        CompareHandsForAttack();
    }

    public void StartGame()
    {
        if (isFirstDraw) Piger();
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
        yield return new WaitForSeconds(3f);

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
        DesactivateCardBoost();
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

    private int GetHandValue(string handCombo)
    {
        switch (handCombo)
        {
            case "ROYAL FLUSH": return attackPower * 10;
            case "STRAIGHT FLUSH": return attackPower * 9;
            case "FOUR OF A KIND": return attackPower * 8;
            case "FULL HOUSE": return attackPower * 7;
            case "FLUSH": return attackPower * 6;
            case "STRAIGHT": return attackPower * 5;
            case "BRELAN": return attackPower * 4;
            case "TWO PAIRS": return attackPower * 3;
            case "PAIR": return attackPower * 2;
            default: return attackPower;
        }
    }

    private void CompareHandsForAttack()
    {
        var playerHandValue = GetHandValue(playerComboText.text);
        var enemyHandValue = GetHandValue(enemyComboText.text);

        if (playerHandValue > enemyHandValue)
        {
            enemy.TakeDamage(playerHandValue);
        }
        else if (enemyHandValue > playerHandValue)
        {
            player.TakeDamage(enemyHandValue);
        }
    }

    public void PressButttonSound()
    {
        audioManager.PlaySound(audioManager.pressButton);
    }

    public void CardShuffleSound()
    {
        
    }

    public void cardDrawSound()
    {
        
    }

    private IEnumerator AnimateSwords()
    {
        float animSpeed = 3.0f;
        
        for (float t = 0; t < 1f; t += Time.deltaTime * animSpeed)
        {
            sword1.transform.localPosition = Vector3.Lerp(new Vector3(-28, -15, 0), new Vector3(-128, -15, 0), t);
            sword2.transform.localPosition = Vector3.Lerp(new Vector3(12, -15, 0), new Vector3(112, -25, 0), t);
            sword1.transform.Rotate(0, 0, 90 * Time.deltaTime);
            sword2.transform.Rotate(0, 0, -90 * Time.deltaTime);
            sword1.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1.25f, 1.25f, 1.25f), t);
            sword2.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1.25f, 1.25f, 1.25f), t);
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        
        for (float t = 0; t < 1f; t += Time.deltaTime * animSpeed)
        {
            sword1.transform.localPosition = Vector3.Lerp(new Vector3(-128, -15, 0), new Vector3(-28, -15, 0), t);
            sword2.transform.localPosition = Vector3.Lerp(new Vector3(112, -15, 0), new Vector3(12, -15, 0), t);
            sword1.transform.Rotate(0, 0, -90 * Time.deltaTime);
            sword2.transform.Rotate(0, 0, 90 * Time.deltaTime);
            sword1.transform.localScale = Vector3.Lerp(new Vector3(1.25f, 1.25f, 1.25f), new Vector3(1, 1, 1), t);
            sword2.transform.localScale = Vector3.Lerp(new Vector3(1.25f, 1.25f, 1.25f), new Vector3(1, 1, 1), t);
            yield return null;
        }
    }

    private IEnumerator animateCard(Carte carte)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        
        Quaternion initialRotation = carte.transform.rotation;
        Quaternion halfRotation = Quaternion.Euler(0, 180, 0);
        Quaternion fullRotation = Quaternion.Euler(0, 360, 0);
        
        while (elapsedTime < duration)
        {
            carte.transform.rotation = Quaternion.Slerp(initialRotation, halfRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        carte.transform.rotation = halfRotation;
        carte.ShowBackCard();
        
        yield return new WaitForSeconds(0.2f);
        
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            carte.transform.rotation = Quaternion.Slerp(halfRotation, fullRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        carte.transform.rotation = initialRotation;
        carte.ShowFrontCard();

    }

    public IEnumerator FlipCard(Carte[] carte)
    {
        foreach (Carte card in carte)
        {
            StartCoroutine(animateCard(card));
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void activeCardBoost(string boostName)
    {
        List<int> index = new List<int>();

        for (int i = 0; i < main.Length; i++)
        {
            index.Add(i);
        }

        if (index.Count > 0)
        {
            int indexRandom = index[Random.Range(0, index.Count)];

            Carte chooseCard = main[indexRandom];

            bool isAnyBoostActive = false;
            foreach (Transform child in chooseCard.transform)
            {
                if (child.gameObject.activeSelf && child.CompareTag("Boost"))
                {
                    isAnyBoostActive = true;
                    break;
                }
            }

            if (!isAnyBoostActive)
            {
                Transform cardTransform = chooseCard.transform.Find(boostName);
                if (cardTransform != null && cardTransform.CompareTag("Boost"))
                {
                    cardTransform.gameObject.SetActive(true);
                }
            }
        }
    }

    private void DesactivateCardBoost()
    {
        foreach (Carte card in main)
        {
            if (card != null)
            {
                foreach (Transform child in card.transform)
                {
                    if (child.CompareTag("Boost") && child.gameObject.activeSelf)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}