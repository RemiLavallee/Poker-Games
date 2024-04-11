using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int coinsCount;
    public static Inventory instance;
    public TextMeshProUGUI[] coinsCountText;

    private void Awake()
    {
        instance = this;
        UpdateTextUi();
    }

    public void AddCoins(int count)
    {
        coinsCount += count;
        UpdateTextUi();
    }

    public void RemoveCoins(int count)
    {
        coinsCount -= count;
        UpdateTextUi();
    }

    public void UpdateTextUi()
    {
        foreach (var text in coinsCountText)
        {
            text.text = coinsCount.ToString();
        }
    }
}