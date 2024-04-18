using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * Main access point for all gameplay tag related things
 * Should only be created once per project
 * If you want to add / query tags, do it here
 */
[CreateAssetMenu(fileName = "GameplayTag", menuName = "ScriptableObjects/GameplayTag", order = 1)]
public class GameplayTags : ScriptableObject
{
    public GameplayTagContainer Tags = new();

    public GameplayTags()
    {
        string Test1 = "Tag.Element.Fire";
        string Test2 = "Tag.Element.Cold";
        string Test3 = "Tag.Damage.Fire";

        Tags.Add(Test1);
        Tags.Add(Test2);
        Tags.Add(Test3);
    }
}
