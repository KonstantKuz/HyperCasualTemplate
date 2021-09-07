﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsQueue))]
public class LevelsQueueEditor : Editor
{
    private string arrayPropertyName = "levels";
    private string[] propertiesNames = {"sceneName", "isRepeatable"};

    private ReorderableDrawer arrayDrawer;

    private void OnEnable()
    {
        arrayDrawer = new ReorderableDrawer(ReorderableType.WithOneLineProperties, propertiesNames);
        arrayDrawer.SetUp(serializedObject, arrayPropertyName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, arrayPropertyName);
        serializedObject.ApplyModifiedProperties();

        arrayDrawer.Draw(serializedObject, target);

        LevelsQueue playableLevels = (LevelsQueue) target;

        if (GUI.changed)
        {
            Undo.RecordObject(playableLevels, $"LevelsQueue Modify");
            EditorUtility.SetDirty(playableLevels);
        }
    }
}