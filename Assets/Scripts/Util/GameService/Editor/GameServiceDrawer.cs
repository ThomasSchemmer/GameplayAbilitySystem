using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(GameServiceWrapper))]
public class GameServiceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect Position, SerializedProperty ServiceProperty, GUIContent Label)
    {
        SerializedProperty EditorProperty = ServiceProperty.FindPropertyRelative("IsForEditor");
        SerializedProperty GameProperty = ServiceProperty.FindPropertyRelative("IsForGame");
        SerializedProperty ScriptProperty = ServiceProperty.FindPropertyRelative("TargetScript");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(500));
        EditorGUILayout.PropertyField(ScriptProperty);
        EditorGUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

}