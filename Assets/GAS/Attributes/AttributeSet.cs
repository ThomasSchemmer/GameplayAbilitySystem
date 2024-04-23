using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeSet", menuName = "ScriptableObjects/AttributeSet", order = 2)]
public class AttributeSet : ScriptableObject
{
    public List<Attribute> Attributes = new();

    public void Initialize()
    {
        foreach (Attribute Attribute in Attributes)
        {
            Attribute.Initialize();
        }
    }

    public void Tick()
    {
        foreach (Attribute Attribute in Attributes)
        {
            float OldValue = Attribute.CurrentValue;
            Attribute.Tick();
            if (!Mathf.Approximately(OldValue, Attribute.CurrentValue))
            {
                _OnAnyAttributeChanged?.Invoke(Attribute);
            }
        }
    }

    public delegate void OnAnyAttributeChanged(Attribute Attribute);
    public OnAnyAttributeChanged _OnAnyAttributeChanged;

}
