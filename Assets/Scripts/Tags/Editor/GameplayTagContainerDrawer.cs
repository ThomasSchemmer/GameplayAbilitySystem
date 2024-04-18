
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(GameplayTagContainer))]
public class GameplayTagContainerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect Position, SerializedProperty GameplayTagNodeProperty, GUIContent Label)
    {
        EditorGUI.BeginProperty(Position, Label, GameplayTagNodeProperty);
        SerializedProperty TagsProperty = GameplayTagNodeProperty.FindPropertyRelative("Tags");

        DisplayAddTag(TagsProperty);
        EditorGUILayout.Space(5);
        DisplayButtons(TagsProperty);
        EditorGUILayout.Space(5);
        DisplayTags(TagsProperty);

        EditorGUI.EndProperty();
    }

    private void DisplayAddTag(SerializedProperty TagsProperty)
    {
        EditorGUILayout.BeginVertical();
        bShowAddTag = EditorGUILayout.Foldout(bShowAddTag, "Add new GameplayTag");
        if (bShowAddTag)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name:", GUILayout.MaxWidth(50));
            TagToAddString = EditorGUILayout.TextField(TagToAddString);
            if (GUILayout.Button("Add Tag", GUILayout.MaxWidth(75)))
            {
                AddTag(TagsProperty);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void AddTag(SerializedProperty TagsProperty) {
        if (TagToAddString.Equals(GlobalTagToAddString))
            return;

        string[] Tokens = TagToAddString.Split(GameplayTagNode.Divisor);
        int FoundDepth = -1;
        int TokenIndex = 0;
        int FoundIndex = -1;
        for (int i = 0; i < TagsProperty.arraySize; i++)
        {
            SerializedProperty TargetProp = TagsProperty.GetArrayElementAtIndex(i);
            SerializedProperty TargetTokenProp = TargetProp.FindPropertyRelative("Token");
            SerializedProperty TargetDepthProp = TargetProp.FindPropertyRelative("Depth");

            // a previous one was mismatched
            if (TargetDepthProp.intValue != TokenIndex)
                continue;

            if (!Tokens[TokenIndex].Equals(TargetTokenProp.stringValue))
                continue;

            TokenIndex++;
            FoundIndex = i;
            FoundDepth = TargetDepthProp.intValue;
        }

        if (FoundIndex == -1)
        {
            FoundIndex = TagsProperty.arraySize - 1;
            FoundDepth = -1;
        }

        int InsertCount = 0;
        for (int i = TokenIndex; i < Tokens.Length; i++) {
            int TargetIndex = FoundIndex + InsertCount;
            TagsProperty.InsertArrayElementAtIndex(TargetIndex);
            TagsProperty.serializedObject.ApplyModifiedProperties();
            SerializedProperty NewTagProp = TagsProperty.GetArrayElementAtIndex(TargetIndex + 1);
            SerializedProperty NewTokenProp = NewTagProp.FindPropertyRelative("Token");
            SerializedProperty NewDepthProp = NewTagProp.FindPropertyRelative("Depth");

            NewTokenProp.stringValue = Tokens[i];
            NewDepthProp.intValue = FoundDepth + 1;
            InsertCount++;
        }

        TagsProperty.serializedObject.ApplyModifiedProperties();
    }

    private void DisplayTags(SerializedProperty TagsProperty)
    {
        EditorGUILayout.BeginVertical();
        int FoldDepth = -1;
        Dictionary<int, string> TokenDic = new();

        for (int i = 0; i < TagsProperty.arraySize; i++)
        {
            SerializedProperty TagProperty = TagsProperty.GetArrayElementAtIndex(i);
            SerializedProperty IsFoldedProp = TagProperty.FindPropertyRelative("bIsFolded");
            SerializedProperty TokenProp = TagProperty.FindPropertyRelative("Token");
            SerializedProperty DepthProp = TagProperty.FindPropertyRelative("Depth");

            if (!TokenDic.ContainsKey(DepthProp.intValue))
            {
                TokenDic.Add(DepthProp.intValue, TokenProp.stringValue);
            }
            TokenDic[DepthProp.intValue] = TokenProp.stringValue;

            string Tag = GetTotalTag(TokenDic, TokenProp, DepthProp);
            if (bIsSearching && !Tag.Contains(SearchString))
                continue;

            if (FoldDepth >= 0 && FoldDepth < DepthProp.intValue)
                continue;

            if (FoldDepth >= 0 && FoldDepth == DepthProp.intValue)
            {
                FoldDepth = -1;
            }

            if (IsFoldedProp.boolValue)
            {
                FoldDepth = DepthProp.intValue;
            }

            EditorGUILayout.PropertyField(TagProperty);
        }
        EditorGUILayout.EndVertical();
    }

    private void DisplayButtons(SerializedProperty TagsProperty)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Expand All", GUILayout.MaxWidth(100)))
        {
            FoldAll(TagsProperty, false);
        }
        if (GUILayout.Button("Collapse All", GUILayout.MaxWidth(100)))
        {
            FoldAll(TagsProperty, true);
        }

        SearchString = EditorGUILayout.DelayedTextField(SearchString, new GUIStyle("ToolbarSearchTextField"));
        if (SearchString.Equals(string.Empty))
        {
            SearchString = GlobalSearchString;
        }
        bIsSearching = !SearchString.Equals(GlobalSearchString);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

    }

    private string GetTotalTag(Dictionary<int, string> TokenDic, SerializedProperty TokenProp, SerializedProperty DepthProp)
    {
        string Tag = "";
        for (int i = 0; i < DepthProp.intValue; i++)
        {
            Tag += TokenDic[i] + GameplayTagNode.Divisor;
        }
        Tag += TokenProp.stringValue;
        return Tag;
    }

    private void FoldAll(SerializedProperty TagsProperty, bool bIsFolded)
    {
        for (int i = 0; i < TagsProperty.arraySize; i++)
        {
            SerializedProperty TagProperty = TagsProperty.GetArrayElementAtIndex(i);
            SerializedProperty IsFoldedProp = TagProperty.FindPropertyRelative("bIsFolded");

            IsFoldedProp.boolValue = bIsFolded;
        }
    }

    private static string GlobalSearchString = "Search Gameplay Tags..";
    private static string GlobalTagToAddString = "X.Y.Z";

    private string SearchString = GlobalSearchString;
    private string TagToAddString = GlobalTagToAddString;
    private bool bIsSearching = false;
    private bool bShowAddTag = false;
}