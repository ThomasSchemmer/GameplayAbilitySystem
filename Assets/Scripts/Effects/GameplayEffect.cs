using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayEffect", menuName = "ScriptableObjects/GameplayEffect", order = 3)]
public class GameplayEffect : ScriptableObject
{
    public enum Duration
    {
        Instant, 
        Duration,
        Infinite
    }

    public Duration DurationPolicy;
    public List<GameplayEffectModifier> Modifiers = new();
    private GameplayAbilityBehaviour Target;

    public GameplayTagRegularContainer GrantedTags = new("Granted Tags");
    public GameplayTagRegularContainer ApplicationRequirementTags = new("Application Requirement Tags");
    public GameplayTagRegularContainer OngoingRequirementTags = new("Ongoing Requirement Tags");

    public void SetTarget(GameplayAbilityBehaviour Target)
    {
        this.Target = Target;
        foreach (GameplayEffectModifier Modifier in Modifiers)
        {
            Modifier.SetTarget(Target);
        }
    }

    public void Execute()
    {
        foreach (GameplayEffectModifier Modifier in Modifiers) { 
            Modifier.Execute(); 
        }
    }

    public void Add()
    {
        foreach (GameplayEffectModifier Modifier in Modifiers)
        {
            Modifier.Add();
        }
    }

    public void Remove()
    {
        foreach (GameplayEffectModifier Modifier in Modifiers)
        {
            Modifier.Remove();
        }
    }

    public void Tick(float Delta)
    {
        foreach (GameplayEffectModifier Modifier in Modifiers)
        {
            Modifier.Tick(Delta);
        }
    }
}
