using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplayEffectModifier
{
    public enum Type
    {
        Add,
        Multiply,
        Divide,
        Override
    }

    public string Attribute;
    public Type Operation;
    public float Period = 0;
    public float Value = 0;

    private GameplayAbilityBehaviour Target;

    public void SetTarget(GameplayAbilityBehaviour Target)
    {
        this.Target = Target;
    }

    public void Execute()
    {

    }

    public void Add()
    {

    }

    public void Remove()
    {

    }

    public void Tick(float Delta)
    {

    }
}
