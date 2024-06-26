using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

// https://gist.github.com/monry/9de7009689cbc5050c652bcaaaa11daa
public static class SerializedPropertyExtensions
{
    public static SerializedProperty FindParentProperty(this SerializedProperty serializedProperty)
    {
        var propertyPaths = serializedProperty.propertyPath.Split('.');
        if (propertyPaths.Length <= 1)
        {
            return default;
        }

        var parentSerializedProperty = serializedProperty.serializedObject.FindProperty(propertyPaths[0]);
        for (var index = 1; index < propertyPaths.Length - 1; index++)
        {
            if (propertyPaths[index] == "Array")
            {
                if (index + 1 == propertyPaths.Length - 1)
                {
                    // reached the end
                    break;
                }
                if (propertyPaths.Length > index + 1 && Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
                {
                    var match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                    var arrayIndex = int.Parse(match.Groups[1].Value);
                    parentSerializedProperty = parentSerializedProperty.GetArrayElementAtIndex(arrayIndex);
                    index++;
                }
            }
            else
            {
                parentSerializedProperty = parentSerializedProperty.FindPropertyRelative(propertyPaths[index]);
            }
        }

        return parentSerializedProperty;
    }

    public static int FindIndexInParentProperty(this SerializedProperty serializedProperty)
    {
        var propertyPaths = serializedProperty.propertyPath.Split('.');
        if (propertyPaths.Length <= 1)
        {
            return default;
        }

        var index = propertyPaths.Length - 2;
        if (propertyPaths[index] == "Array")
        {
            if (Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
            {
                var match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                var arrayIndex = int.Parse(match.Groups[1].Value);
                return arrayIndex;
            }
        }

        return -1;
    }
}