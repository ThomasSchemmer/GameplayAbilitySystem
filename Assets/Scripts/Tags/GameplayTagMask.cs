using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Inner representation of GameplayTags.
 * Each bool represents one token, so currently up to a maximum of 256 full Tags are supported
 * In theory could have been a long and mask directly, but that would only support up to 64 tokens
 */
public class GameplayTagMask 
{
    private bool[] Mask = new bool[256];

    public void Set(int i)
    {
        Mask[i] = true;
    }

    public void Clear(int i)
    {
        Mask[i] = false;
    }

    public void SetIndices(List<int> Indices)
    {
        foreach (int Index in Indices)
        {
            Mask[Index] = true;
        }
    }

    public void AddTag(string Tag)
    {
        GameplayTagMask NewMask = GameplayTags.ConvertTagToMask(Tag);
        Combine(NewMask);
    }

    public bool HasTag(string Tag)
    {
        List<int> Indices = GameplayTags.ConvertToIndices(Tag);
        for (int i = 0; i < Indices.Count; i++)
        {
            if (!Mask[i])
                return false;
        }

        return true;
    }

    public void Combine(GameplayTagMask OtherMask)
    {
        for (int i = 0; i < Mask.Length; i++)
        {
            Mask[i] |= OtherMask.Mask[i]; 
        }
    }
}
