using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractButton : MonoBehaviour
{
    [SerializeField] private GameObject panelUI;

    public void OpenPanelUI(GameObject panelUI)
    {
        panelUI.SetActive(true);
    }

    public void ClosePanelUI(GameObject panelUI)
    {
        panelUI.SetActive(false);
    }

    public void BackHome()
    {
        SceneManager.LoadSceneAsync("Lobby");
    }
}
