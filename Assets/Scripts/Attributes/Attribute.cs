using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    public string Name = "";
    public float BaseValue = 0;
    public float CurrentValue = 0;

    public delegate void OnAttributeChanged();
    public OnAttributeChanged _OnAttributeChanged;
}
