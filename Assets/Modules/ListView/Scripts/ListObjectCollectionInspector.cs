using Microsoft.MixedReality.Toolkit.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ListObjectCollection), true)]
public class GridObjectCollectionInspector : BaseCollectionInspector
{
    private SerializedProperty layout;
    private SerializedProperty gap;
    private SerializedProperty hAlignment;
    private SerializedProperty vAlignment;


    protected override void OnEnable()
    {
        base.OnEnable();
        layout = serializedObject.FindProperty("layout");
        gap = serializedObject.FindProperty("gap");
        hAlignment = serializedObject.FindProperty("hAlignment");
        vAlignment = serializedObject.FindProperty("vAlignment");
    }

    protected override void OnInspectorGUIInsertion()
    {
        EditorGUILayout.PropertyField(gap, new GUIContent("Gap", "gap between two objects"));
        EditorGUILayout.PropertyField(layout, new GUIContent("Layout", "Specify direction in which children are laid out"));

        ListLayout layoutTypeIndex = (ListLayout)layout.enumValueIndex;
        if (layoutTypeIndex == ListLayout.Vertical)
        {
            EditorGUILayout.PropertyField(hAlignment, new GUIContent("Horizontal Alignment"));
        }
        else
        {
            EditorGUILayout.PropertyField(vAlignment, new GUIContent("Vertical Alignment"));
        }
    }
}
