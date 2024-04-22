
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/** Adaptive drawer for source containers
 * supports creation of new tags by inserting tokens into the global truth
 */
[CustomPropertyDrawer(typeof(GameplayTagSourceContainer))]
public class GameplayTagTokenContainerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect Position, SerializedProperty GameplayTagContainerProperty, GUIContent Label)
    {
        EditorGUI.BeginProperty(Position, Label, GameplayTagContainerProperty);

        DisplayGlobalSource(GameplayTagContainerProperty);

        EditorGUI.EndProperty();
    }

    private void DisplayGlobalSource(SerializedProperty GameplayTagContainerProperty)
    {
        SerializedProperty TagsProperty = GameplayTagContainerProperty.FindPropertyRelative("Tags");

        ContainerSourceDrawerLibrary.DisplayAddTag(GameplayTagContainerProperty);
        EditorGUILayout.Space(5);
        ContainerDrawerLibrary.DisplayButtons(TagsProperty);
        EditorGUILayout.Space(5);
        ContainerSourceDrawerLibrary.DisplaySourceTags(TagsProperty);
    }
}