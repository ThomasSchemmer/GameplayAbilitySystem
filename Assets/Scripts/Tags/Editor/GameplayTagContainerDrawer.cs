
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/** 
 * Looks up tags from the global source as a dropdown and can store a selection of them
 * Does not support updating tags if they are modified in the source!
 */
[CustomPropertyDrawer(typeof(GameplayTagRegularContainer))]
public class GameplayTagContainerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect Position, SerializedProperty GameplayTagContainerProperty, GUIContent Label)
    {
        EditorGUI.BeginProperty(Position, Label, GameplayTagContainerProperty);

        DisplayRegularContainer(GameplayTagContainerProperty);

        EditorGUI.EndProperty();
    }

    private void DisplayRegularContainer(SerializedProperty GameplayTagContainerProperty)
    {
        //https://youtu.be/1T4S2lFf19s?t=309

        GameplayTags GlobalGameplayTags = ContainerRegularDrawerLibrary.LoadGlobalGameplayTags();
        if (GlobalGameplayTags == null)
        {
            EditorGUILayout.LabelField("Could not find a global definition of gameplay tags!");
            return;
        }

        EditorGUILayout.BeginHorizontal();
        SerializedProperty EditingProperty = GameplayTagContainerProperty.FindPropertyRelative("bIsEditing");
        EditingProperty.boolValue = EditorGUILayout.Toggle("", EditingProperty.boolValue, GUILayout.MaxWidth(15));
        EditorGUILayout.LabelField("Edit", GUILayout.MaxWidth(50));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (EditingProperty.boolValue)
        {
            SerializedProperty IDsProperty = GameplayTagContainerProperty.FindPropertyRelative("IDs");
            ContainerRegularDrawerLibrary.DisplayButtons(GlobalGameplayTags);
            EditorGUILayout.Space(5);
            ContainerRegularDrawerLibrary.DisplayGlobalLookupTags(GlobalGameplayTags, IDsProperty);
        }

        ContainerRegularDrawerLibrary.DisplayRegularTags(GlobalGameplayTags, GameplayTagContainerProperty);

    }

}