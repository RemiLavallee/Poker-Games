using System;
using UnityEngine;

public class CardScore : UsingItems
{
    private CarteManager carteManager;
    private string scoreBoost = "Score";

    public override void ApplyModifier()
    {
        carteManager = FindObjectOfType<CarteManager>();
        carteManager.activeCardBoost(scoreBoost);
        carteManager.attackPower *= 2;
    }
}
