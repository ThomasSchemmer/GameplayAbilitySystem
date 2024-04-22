using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Core component of the GAS, handles communication between different GAB's */
public class GameplayAbilitySystem : GameService
{
    public List<GameplayAbilityBehaviour> Behaviours = new();

    public void Register(GameplayAbilityBehaviour Behaviour)
    {
        Behaviours.Add(Behaviour);
    }

    protected override void StartServiceInternal()
    {
        _OnInit?.Invoke();
    }

    protected override void StopServiceInternal() {}


}
