using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAbilityBehaviour : MonoBehaviour
{
    void Start()
    {
        Game.RunAfterServiceInit((GameplayAbilitySystem System) =>
        {
            System.Register(this);
        });
    }

    public void Tick(float Delta)
    {
        foreach (GameplayEffect ActiveEffect in ActiveEffects)
        {
            if (!HasTags(ActiveEffect.OngoingRequirementTags.IDs))
            {
                RemoveEffect(ActiveEffect);
                continue;
            }

            ActiveEffect.Tick(Delta);
        }
    }

    public void AddTag(string ID) {
        GameplayTagMask.Set(ID);

        _OnTagsChanged?.Invoke();
        _OnTagAdded?.Invoke(ID);
    }

    public void AddTags(List<string> IDs)
    {
        foreach (string ID in IDs)
        {
            AddTag(ID);
        }
    }

    public void RemoveTag(string ID)
    {
        GameplayTagMask.Clear(ID);

        _OnTagsChanged?.Invoke();
        _OnTagAdded?.Invoke(ID);
    }

    public void RemoveTags(List<string> IDs)
    {
        foreach (string ID in IDs)
        {
            RemoveTag(ID);
        }
    }

    public bool HasTag(string Tag)
    {
        return GameplayTagMask.HasID(Tag);
    }

    public bool HasTags(List<string> IDs)
    {
        foreach (var ID in IDs)
        {
            if (!HasTag(ID))
                return false;
        }
        return true;
    }

    public void AddEffect(GameplayEffect Effect) { 
        ActiveEffects.Add(Effect);
        AddTags(Effect.GrantedTags.IDs);
    }

    public void RemoveEffect(GameplayEffect Effect)
    {
        ActiveEffects.Remove(Effect);
        RemoveTags(Effect.GrantedTags.IDs);
    }

    public static GameplayAbilityBehaviour Get(GameObject GameObject)
    {
        return GameObject.GetComponent<GameplayAbilityBehaviour>();
    }

    public AttributeSet Attributes;
    private List<GameplayEffect> ActiveEffects = new();
    private GameplayTagMask GameplayTagMask = new();

    public delegate void OnTagsChanged();
    public delegate void OnTagAdded(string Tag);
    public delegate void OnTagRemoved(string Tag);
    public OnTagsChanged _OnTagsChanged;
    public OnTagAdded _OnTagAdded;
    public OnTagRemoved _OnTagRemoved;
}
