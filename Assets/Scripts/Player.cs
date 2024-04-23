using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameplayEffect BurnEffect, DousingEffect;

    void Update()
    {
        CheckForBurning();
        CheckForDousing();
    }

    private void CheckForBurning()
    {
        if (!Input.GetKeyDown(KeyCode.B))
            return;

        if (!Game.TryGetService(out GameplayAbilitySystem GAS))
            return;

        GAS.TryApplyEffectTo(GameplayAbilityBehaviour.Get(this.gameObject), BurnEffect);
    }

    private void CheckForDousing()
    {

        if (!Input.GetKeyDown(KeyCode.D))
            return;

        if (!Game.TryGetService(out GameplayAbilitySystem GAS))
            return;

        GAS.TryApplyEffectTo(GameplayAbilityBehaviour.Get(this.gameObject), DousingEffect);
    }
}
