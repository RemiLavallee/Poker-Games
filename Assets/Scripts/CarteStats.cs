using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarteStats : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text valeurText;
    [SerializeField] private TMP_Text enseigneText;
    [SerializeField] private GameObject heldImage;
    public bool IsHeld;
    public CarteData Data { get; private set; }
    
    internal void SetData(CarteData carteData)
    {
        Data = carteData;
        string valeurString = carteData.valeur.ToString();

        switch (carteData.valeur)
        {
            case 1:
                valeurString = "A";
                break;
            case 11:
                valeurString = "J";
                break;
            case 12:
                valeurString = "Q";
                break;
            case 13:
                valeurString = "K";
                break;
        }

        valeurText.text = valeurString;

        string enseigneString = valeurString;
        Color color = Color.red;

        switch (carteData.enseigne)
        {
            case Enseigne.Coeur:
                enseigneString = "\u2665";
                break;
            case Enseigne.Pique:
                enseigneString = "\u2660";
                color = Color.black;
                break;
            case Enseigne.Trefle:
                enseigneString = "\u2663";
                color = Color.black;
                break;
            case Enseigne.Carreau:
                enseigneString = "\u2666";
                break;
        }

        enseigneText.text = enseigneString;
        enseigneText.color = color;
        valeurText.color = color;

        valeurText.enabled = true;
        enseigneText.enabled = true;
        Image image = GetComponent<Image>();
        image.sprite = null;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHeld = !IsHeld;
        if (heldImage != null) heldImage.SetActive(IsHeld);
        else this.gameObject.SetActive(false);
    }

    internal void ClearHeld()
    {
        IsHeld = false;
        if (heldImage != null) heldImage.SetActive(!IsHeld);
    }
}
