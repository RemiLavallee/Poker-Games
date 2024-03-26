using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public enum Enseigne
{
    Coeur,
    Pique,
    Carreau,
    Trefle
}

public class CarteData : IComparable<CarteData>
{
    public int valeur;
    public Enseigne enseigne;

    public CarteData(int valeur, Enseigne enseigne)
    {
        this.valeur = valeur;
        this.enseigne = enseigne;
    }

    public int CompareTo(CarteData other)
    {
        return valeur.CompareTo(other.valeur);
    }
}
public class Carte : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text valeurText;
    [SerializeField] private TMP_Text enseigneText;
    [SerializeField] private TMP_Text heldText;
    public bool IsHeld;
    public CarteData Data { get; private set; }

    private void Awake()
    {
        heldText.enabled = false;
    }
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
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        IsHeld = !IsHeld;
        heldText.enabled = IsHeld;
    }

    internal void ClearHeld()
    {
        IsHeld = false;
        heldText.enabled = false;

    }
}
