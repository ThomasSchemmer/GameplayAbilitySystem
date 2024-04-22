using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Container for all gameplaytags currently applied to a single object 
 * Contains ids instead of the usual tags
 */
[Serializable]
public class GameplayTagRegularContainer
{
    public List<string> IDs = new();
    public bool bIsEditing = false;
}
