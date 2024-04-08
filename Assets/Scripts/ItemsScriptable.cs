using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "itemsScriptableObject", 
    menuName = "ScriptableObject/Item")]
public class ItemsScriptable : ScriptableObject
{
    [SerializeField] internal GameObject prefab;
    [SerializeField] internal string nameItem;
    [SerializeField] internal int cost;
    [SerializeField] internal Sprite image;
    [SerializeField] internal string description;
    [SerializeField] internal int itemCount;
    
    public GameObject Prefab
    {
        get => prefab;
        private set => prefab = value;
    }
    
    public string Name
    {
        get => nameItem;
        private set => nameItem = value;
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
