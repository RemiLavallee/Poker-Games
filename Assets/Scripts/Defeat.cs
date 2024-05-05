using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Defeat : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    [SerializeField] private GameObject panelDefeat;
    private void Awake()
    {
        player = FindObjectOfType<PlayerHealth>();
    }

    public void DefeatCondition()
    {
        if (player.currentHealth <= 0)
        {
            StartCoroutine(ActiveDefeatPanel());
        }
    }

    private IEnumerator ActiveDefeatPanel()
    {
        yield return new WaitForSeconds(2f);
        panelDefeat.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MoteurJeu");
    }
}