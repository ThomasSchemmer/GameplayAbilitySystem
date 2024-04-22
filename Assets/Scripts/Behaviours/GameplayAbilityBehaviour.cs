using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAbilityBehaviour : MonoBehaviour
{
    void Start()
    {
        Game.RunAfterServiceInit((GameplayAbilitySystem System) =>
        {
            this.System = System;
            System.Register(this);
        });
    }

    public void AddTag(string Tag) {
        GameplayTagMask.AddTag(Tag);

        _OnTagsChanged?.Invoke();
        _OnTagAdded?.Invoke(Tag);
    }

    public bool HasTag(string Tag)
    {
        return GameplayTagMask.HasTag(Tag);
    }

    [SerializeField]
    public GameplayTagRegularContainer Container = new();
    private GameplayAbilitySystem System;
    private GameplayTagMask GameplayTagMask = new();

    public delegate void OnTagsChanged();
    public delegate void OnTagAdded(string Tag);
    public delegate void OnTagRemoved(string Tag);
    public OnTagsChanged _OnTagsChanged;
    public OnTagAdded _OnTagAdded;
    public OnTagRemoved _OnTagRemoved;
}