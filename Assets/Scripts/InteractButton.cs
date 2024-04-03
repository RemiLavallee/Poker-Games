using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [SerializeField] private GameObject scoreUI;

    public void OpenScoreUI()
    {
        scoreUI.SetActive(true);
    }

    public void CloseScoreUI()
    {
        scoreUI.SetActive(false);
    }

    public void OpenItemUI()
    {
        
    }

    public void CloseItemUI()
    {
        
    }

    public void BackHome()
    {
        
    }
}
