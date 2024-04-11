using System;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Main Panel")] [SerializeField]
    private Button rightClick;
    [SerializeField] private Button leftClick;
    [SerializeField] private List<GameObject> imageChange;
    [SerializeField] private List<TMP_Text> coinsInventory;
    private int currentImageIndex = 0;

    [Header("Shop Panel")] [SerializeField]
    private GameObject panelShop;
    [SerializeField] private GameObject panelInventory;
    
    [Header("Item Panel")] [SerializeField]
    private GameObject panelItem;

    [SerializeField] private GameObject itemImage;
    [SerializeField] private TMP_Text nameItem;
    [SerializeField] private TMP_Text descriptionItem;
    [SerializeField] private TMP_Text costItem;
    [SerializeField] private List<ItemsScriptable> itemScriptable;

    [Serializable]
    public class ItemInventory
    {
        public string nameItem;
        public TMP_Text itemCountText;
        public int itemCount;
        [SerializeField] private Sprite imageItem;
    }

    [Serializable]
    public class ReliqueInventory
    {
        public string nameRelique;
        public TMP_Text reliqueCountText;
        public int reliqueCount;
        [SerializeField] private Sprite imageRelique;
    }

    public List<ItemInventory> itemInventory = new List<ItemInventory>();
    public List<ReliqueInventory> reliqueInventory = new List<ReliqueInventory>();
    [SerializeField] private GameObject[] imageItem;
    [SerializeField] private GameObject[] imageRelique;
    private ItemsScriptable selectedItemScriptable = null;

    public void Start()
    {
        
        Inventory.instance.UpdateTextUi();

        foreach (var item in itemInventory)
        {
            if (item.itemCountText != null)
            {
                string itemName = item.nameItem;
                int itemCount = 0;
                InventoryManager.instance.itemCounts.TryGetValue(itemName, out itemCount);
                item.itemCountText.text = itemCount.ToString();
            }
        }

        foreach (var relique in reliqueInventory)
        {
            if (relique.reliqueCountText != null)
            {
                string reliqueName = relique.nameRelique;
                int itemCount = 0;
                InventoryManager.instance.itemCounts.TryGetValue(reliqueName, out itemCount);
                relique.reliqueCountText.text = itemCount.ToString();
            }
        }
        
        LoadAndSave.instance.LoadData();
    }

    public void Exit(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }

    public void BackHome()
    {
        panelShop.SetActive(false);
    }

    public void ClickRightImage()
    {
        if (currentImageIndex < imageChange.Count - 1)
        {
            currentImageIndex++;
            UpdateImage();
        }
    }

    public void ClickLeftImage()
    {
        if (currentImageIndex > 0)
        {
            currentImageIndex--;
            UpdateImage();
        }
    }

    private void UpdateImage()
    {
        for (var i = 0; i < imageChange.Count; i++)
        {
            imageChange[i].SetActive(i == currentImageIndex);
        }
    }

    public void StartGame()
    {
        if (currentImageIndex == 0) SceneManager.LoadSceneAsync("MoteurJeu");
        if (currentImageIndex == 1) panelShop.SetActive(true);
        
        LoadAndSave.instance.SaveData();
    }

    public void BuyItem()
    {
        if (Inventory.instance.coinsCount < selectedItemScriptable.cost)
        {
            return;
        }

        InventoryManager.instance.itemCounts.TryAdd(selectedItemScriptable.nameItem, 0);
        InventoryManager.instance.itemCounts[selectedItemScriptable.nameItem]++;
        UpdateItemCountUI(selectedItemScriptable.nameItem, 
            InventoryManager.instance.itemCounts[selectedItemScriptable.nameItem]);
        UpdateReliqueCountUI(selectedItemScriptable.nameItem, 
            InventoryManager.instance.itemCounts[selectedItemScriptable.nameItem]);
        Inventory.instance.RemoveCoins(selectedItemScriptable.cost);
        Inventory.instance.UpdateTextUi();

    }

    public void ClickOnItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= itemScriptable.Count) return;
        selectedItemScriptable = itemScriptable[itemIndex];

        if (selectedItemScriptable != null)
        {
            nameItem.text = selectedItemScriptable.nameItem;
            descriptionItem.text = selectedItemScriptable.description;
            costItem.text = selectedItemScriptable.cost.ToString();
            itemImage.GetComponent<Image>().sprite = selectedItemScriptable.image;

            panelItem.SetActive(true);
        }
    }

    public void OpenInventory()
    {
        panelInventory.SetActive(true);
    }

    private void UpdateItemCountUI(string itemName, int newCount)
    {
        foreach (var item in itemInventory)
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
        foreach (var relique in reliqueInventory)
        {
            if (relique.nameRelique == itemName)
            {
                relique.reliqueCountText.text = newCount.ToString();
                break;
            }
        }
    }
    
}
