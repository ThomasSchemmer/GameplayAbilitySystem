
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GameplayTagToken))]
public class GameplayTagTokenDrawer : PropertyDrawer
{
    public override void OnGUI(Rect Position, SerializedProperty GameplayTagTokenProperty, GUIContent Label)
    {
        EditorGUI.BeginProperty(Position, Label, GameplayTagTokenProperty);

        SerializedProperty TokenProperty = GameplayTagTokenProperty.FindPropertyRelative("Token");
        SerializedProperty DepthProperty = GameplayTagTokenProperty.FindPropertyRelative("Depth");

        string Token = TokenProperty.stringValue;
        int Depth = DepthProperty.intValue;
        Rect Rect = new Rect(Position.x + Depth * Indent + ButtonSize, Position.y, Position.width, Height);
        Rect TextRect = new Rect(Rect.x, Rect.y, 100, Rect.height);
        TokenProperty.stringValue = EditorGUI.DelayedTextField(TextRect, Token);

        DrawButtons(Rect, GameplayTagTokenProperty);

        EditorGUI.EndProperty();
    }

    private void DrawButtons(Rect Rect, SerializedProperty GameplayTagTokenProperty)
    {
        Rect ButtonRectAdd = new Rect(Rect.width - ButtonSize * 2 + ButtonOffsetX, Rect.y + ButtonOffsetY, ButtonSize, ButtonSize);
        Rect ButtonRectRemove = new Rect(Rect.width - ButtonSize + ButtonOffsetX, Rect.y + ButtonOffsetY, ButtonSize, ButtonSize);
        SerializedProperty TagsProperty = GameplayTagTokenProperty.FindParentProperty();
        int Index = GameplayTagTokenProperty.FindIndexInParentProperty();

        DrawFoldButton(Rect, Index, TagsProperty);
        
        if (GUI.Button(ButtonRectAdd, "+"))
        {
            CreateNewEntryAt(Index, TagsProperty);
        }

        if (!HasChildElements(Index, TagsProperty))
        {
            if (GUI.Button(ButtonRectRemove, "-"))
            {
                RemoveEntryAt(Index, TagsProperty);
            }
        }
    }

    private void DrawFoldButton(Rect Rect, int i,  SerializedProperty TagsProperty)
    {
        if (!HasChildElements(i, TagsProperty))
            return;

        Rect ButtonRectFold = new Rect(Rect.x - ButtonSize, Rect.y + ButtonOffsetY, ButtonSize, ButtonSize);
        SerializedProperty TagProp = TagsProperty.GetArrayElementAtIndex(i);
        SerializedProperty IsFoldedProp = TagProp.FindPropertyRelative("bIsFolded");

        bool bNewFold = EditorGUI.Foldout(ButtonRectFold, IsFoldedProp.boolValue, "");
        if (bNewFold != IsFoldedProp.boolValue)
        {
            FoldAt(i, TagsProperty);
        }
    }

    private void FoldAt(int i,  SerializedProperty TagsProperty) {

        SerializedProperty TagProp = TagsProperty.GetArrayElementAtIndex(i);
        SerializedProperty IsFoldedProp = TagProp.FindPropertyRelative("bIsFolded");
        IsFoldedProp.boolValue = !IsFoldedProp.boolValue;
    }

    private bool HasChildElements(int i, SerializedProperty TagsProperty)
    {
        if (TagsProperty.arraySize == 1)
            return true;

        if (i == TagsProperty.arraySize - 1)
            return false;

        SerializedProperty Self = TagsProperty.GetArrayElementAtIndex(i);
        SerializedProperty Other = TagsProperty.GetArrayElementAtIndex(i + 1);
        SerializedProperty SelfDepthProp = Self.FindPropertyRelative("Depth");
        SerializedProperty OtherDepthProp = Other.FindPropertyRelative("Depth");

        return SelfDepthProp.intValue == OtherDepthProp.intValue - 1;
    }

    private void CreateNewEntryAt(int i, SerializedProperty TagsProperty)
    {
        TagsProperty.InsertArrayElementAtIndex(i);
        SerializedProperty OldTag = TagsProperty.GetArrayElementAtIndex(i);
        SerializedProperty NewTag = TagsProperty.GetArrayElementAtIndex(i + 1);
        SerializedProperty TokenProperty = NewTag.FindPropertyRelative("Token");
        SerializedProperty DepthProperty = NewTag.FindPropertyRelative("Depth");
        SerializedProperty OldDepthProperty = OldTag.FindPropertyRelative("Depth");
        TokenProperty.stringValue = "NewTag";
        DepthProperty.intValue = OldDepthProperty.intValue + 1;

        TagsProperty.serializedObject.ApplyModifiedProperties();
    }

    private void RemoveEntryAt(int i, SerializedProperty TagsProperty)
    {
        TagsProperty.DeleteArrayElementAtIndex(i);
        TagsProperty.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Height;
    }

    private static float ButtonSize = 15;
    private static float Height = 20;
    private static float ButtonOffsetX = -15;
    private static float ButtonOffsetY = (Height - ButtonSize) / 2.0f;
    private static float Indent = 10;
}
