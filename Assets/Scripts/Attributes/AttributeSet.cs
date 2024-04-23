using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeSet", menuName = "ScriptableObjects/AttributeSet", order = 2)]
public class AttributeSet : ScriptableObject
{
    public List<Attribute> Attributes = new();

    public delegate void OnAnyAttributeChanged(Attribute Attribute);
    public OnAnyAttributeChanged _OnAnyAttributeChanged;

}
