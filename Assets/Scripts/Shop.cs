using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private Button rightClick;
    [SerializeField] private Button leftClick;
    [SerializeField] private List<GameObject> imageChange;
    [SerializeField] private List<TMP_Text> coinsInventory;
    private int currentImageIndex = 0;
    
    [Header("Shop Panel")]
    [SerializeField] private GameObject panelShop;
    [SerializeField] private GameObject panelInventory;
    
    [Header("Item Panel")]
    [SerializeField] private GameObject panelItem;
    [SerializeField] private GameObject itemImage;
    [SerializeField] private TMP_Text nameItem;
    [SerializeField] private TMP_Text descriptionItem;
    [SerializeField] private TMP_Text costItem;
    [SerializeField] private List<ItemsScriptable> itemScriptable;

    public void Exit()
    {
        if(imageChange[1])
        panelItem.SetActive(false);
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
        if(currentImageIndex == 0) SceneManager.LoadSceneAsync("MoteurJeu");
        if (currentImageIndex == 1) panelShop.SetActive(true);
       // if (imageChange[2]);
    }

    public void BuyItem()
    {
        
    }

    public void ClickOnItem(int itemIndex)
    {
        if(itemIndex < 0 || itemIndex >= itemScriptable.Count) return;
        var selectedItem = itemScriptable[itemIndex];
        
        if (selectedItem != null)
        {
            nameItem.text = selectedItem.nameItem;
            descriptionItem.text = selectedItem.description;
            costItem.text = selectedItem.cost.ToString();
            itemImage.GetComponent<Image>().sprite = selectedItem.image;
            
            panelItem.SetActive(true);
        }
    }

    public void OpenInventory()
    {
        
    }
    
    
}
