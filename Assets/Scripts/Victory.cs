using System.Collections;
using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public int coinsWin;
    [SerializeField] private Inventory inventory;
    [SerializeField] private EnemyHealth enemy;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private TMP_Text coinsWinText;
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        enemy = FindObjectOfType<EnemyHealth>();
    }

    public void VictoryCondition()
    {
        if (enemy.currentHealth <= 0)
        {
            StartCoroutine(ActiveWinPanel());
        }
    }

    public void Confirm()
    {
        LoadAndSave.instance.SaveData();
    }

    private IEnumerator ActiveWinPanel()
    {
        yield return new WaitForSeconds(2f);
        panelWin.SetActive(true);
        coinsWinText.text = coinsWin.ToString();
        Inventory.instance.AddCoins(coinsWin);
        Inventory.instance.UpdateTextUi();
    }
}
