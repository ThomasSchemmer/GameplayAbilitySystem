using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameplayEffect BurnEffect;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.B))
            return;

        if (!Game.TryGetService(out GameplayAbilitySystem GAS))
            return;

        GAS.TryApplyEffectTo(GameplayAbilityBehaviour.Get(this.gameObject), BurnEffect);
    }
}
