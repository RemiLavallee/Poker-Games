using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItems : MonoBehaviour
{
    public ItemsScriptable itemData;
    protected CarteManager player;

    protected virtual void ApplyModifier(){}

    public void Start()
    {
        player = FindObjectOfType<CarteManager>();
        ApplyModifier();
    }
}
