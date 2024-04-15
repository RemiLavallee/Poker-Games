using UnityEngine;

public class LoadAndSave : MonoBehaviour
{
    public static LoadAndSave instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("2 instance");
            return;
        }

        instance = this;
    }

    void Start()
    {
        LoadData();
    }
    
    public void SaveData()
    {
        PlayerPrefs.SetInt("coinsCount", Inventory.instance.coinsCount);
    }
    
    public void LoadData()
    {
        Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
        Inventory.instance.UpdateTextUi();
    }
}
