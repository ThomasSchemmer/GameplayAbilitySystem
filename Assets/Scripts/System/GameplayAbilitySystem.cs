using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Core component of the GAS, handles communication between different GAB's */
public class GameplayAbilitySystem : GameService
{
    public List<GameplayAbilityBehaviour> Behaviours = new();

    public void Update()
    {
        foreach (var Behaviour in Behaviours)
        {
            Behaviour.Tick(Time.deltaTime);
        }
    }

    public void Register(GameplayAbilityBehaviour Behaviour)
    {
        Behaviours.Add(Behaviour);
    }

    protected override void StartServiceInternal()
    {
        _OnInit?.Invoke();
    }

    protected override void StopServiceInternal() {}

    public bool TryApplyEffectTo(GameplayAbilityBehaviour Target, GameplayEffect Effect)
    {
        if (!Target.HasTags(Effect.ApplicationRequirementTags.IDs))
            return false;

        Effect.SetTarget(Target);
        Target.AddEffect(Effect);
        Effect.Execute();
        return true;
    }
}
