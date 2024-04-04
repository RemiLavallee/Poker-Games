using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "itemsScriptableObject", 
    menuName = "ScriptableObject/Item")]
public class ItemsScriptable : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int cost;
    [SerializeField] private Sprite image;
    [SerializeField] private string description;
    [SerializeField] private int itemCount;
    
    public GameObject Prefab
    {
        get => prefab;
        private set => prefab = value;
    }
    
    public int Cost
    {
        get => cost;
        private set => cost = value;
    }
    
    public Sprite Image
    {
        get => image;
        private set => image = value;
    }
    
    public string Description
    {
        get => description;
        private set => description = value;
    }
    
    public int ItemCount
    {
        get => itemCount;
        private set => itemCount = value;
    }

}
