using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Container for all gameplaytags in a given setup */
[Serializable]
public class GameplayTagContainer : ISerializationCallbackReceiver
{
    // only filled in by the serialization
    [HideInInspector]
    public List<GameplayTagToken> Tags = new();
    // this is where the actual data is stored in runtime!
    private GameplayTagNode Root = new(string.Empty);

    public void Add(string Tag)
    {
        Root.Add(Tag);
    }

    public void OnAfterDeserialize()
    {
        // let GC handle the actual deletion
        Root = null;

        Root = GameplayTagNode.ConvertToDeserializedNodes(Tags);
    }

    public void OnBeforeSerialize()
    {
        Tags.Clear();
        Tags = GameplayTagNode.ConvertToSerializedList(Root);
    }
}
