using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [SerializeField] private List<ItemsScriptable> itemScriptable;
    
    [Serializable]
    public class ItemInventoryGame
    {
        public string nameItem;
        public TMP_Text itemCountText;
        public int itemCount;
    }
    
    [Serializable]
    public class ReliqueInventoryGame
    {
        public string nameRelique;
        public TMP_Text reliqueCountText;
        public int reliqueCount;
    }
    
    public List<ItemInventoryGame> itemInventoryGame = new List<ItemInventoryGame>();
    public List<ReliqueInventoryGame> reliqueInventoryGame = new List<ReliqueInventoryGame>();

    public void Start()
    {
        foreach (var item in itemInventoryGame)
        {
            if (item.itemCountText != null)
            {
                InventoryManager.instance.itemCounts.TryGetValue(item.nameItem, out int itemCount);
                item.itemCount = itemCount;
                item.itemCountText.text = itemCount.ToString();
            }
        }

        foreach (var relique in reliqueInventoryGame)
        {
            InventoryManager.instance.itemCounts.TryGetValue(relique.nameRelique, out int itemCount);
            relique.reliqueCount = itemCount;
            relique.reliqueCountText.text = itemCount.ToString();
        }
    }

    private void UpdateItemCountUI(string itemName, int newCount)
    {
        foreach (var item in itemInventoryGame)
        {
            if (item.nameItem == itemName)
            {
                item.itemCountText.text = newCount.ToString();
                break;
            }
        }
    }
    
    private void UpdateReliqueCountUI(string itemName, int newCount)
    {
        foreach (var relique in reliqueInventoryGame)
        {
            if (relique.nameRelique == itemName)
            {
                relique.reliqueCountText.text = newCount.ToString();
                break;
            }
        }
    }

    public void Use(string itemName)
    {
        foreach (var item in itemScriptable)
        {
            if (item.nameItem == itemName)
            {
                UsingItems usingItemScript = item.prefab.GetComponent<UsingItems>();
                
                if (InventoryManager.instance.itemCounts.ContainsKey(itemName) && InventoryManager.instance.itemCounts[itemName] > 0)
                {
                    InventoryManager.instance.itemCounts[itemName]--;
                    UpdateItemCountUI(itemName, InventoryManager.instance.itemCounts[itemName]);
                    UpdateReliqueCountUI(itemName, InventoryManager.instance.itemCounts[itemName]);
                    usingItemScript.ApplyModifier();
                }
            }
        }
    }
}
