using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * Main access point for all gameplay tag related things
 * Should only be created once per project
 * If you want to add / query tags, do it here
 */
[CreateAssetMenu(fileName = "GameplayTags", menuName = "ScriptableObjects/GameplayTags", order = 1)]
public class GameplayTags : ScriptableObject
{
    public GameplayTagSourceContainer Container = new();

    private static GameplayTags Instance;

    public void AddTag(string Tag)
    {
        Container.AddTag(Tag);
    }
    
    public static List<int> ConvertToIndices(string Tag)
    {
        if (Instance == null)
            return new();

        List<int> Indices = new();
        string[] Tokens = Tag.Split(GameplayTagToken.Divisor);
        int TokenIndex = 0;
        for (int i = 0; i < Instance.Container.Tags.Count; i++)
        {
            if (Instance.Container.Tags[i].Token.Equals(Tokens[TokenIndex]))
                continue;

            if (Instance.Container.Tags[i].Depth != TokenIndex)
                continue;

            Indices.Add(i);
            TokenIndex++;
        }

        // we only got a partial match, doesn't count so reset
        if (TokenIndex < Tokens.Length)
        {
            return new();
        }
        return Indices;
    }

    public static GameplayTagMask ConvertTagToMask(string Tag)
    {
        GameplayTagMask Mask = new();
        Mask.SetIndices(ConvertToIndices(Tag));
        return Mask;
    }

    public void SetInstance()
    {
        Instance = this;
    }

    public GameplayTags()
    {
        string Test1 = "Tag.Element.Fire";
        string Test2 = "Tag.Element.Cold";
        string Test3 = "Tag.Damage.Fire";

        Container.AddTag(Test1);
        Container.AddTag(Test2);
        Container.AddTag(Test3);
    }
}
