using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pool))]
//[CanEditMultipleObjects]
public class PoolEditor : Editor
{
    string[] propertiesInBaseClass = new string[] { "autoReturn", "autoReturnDelay", "nameAsTag", "poolTag"};

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, propertiesInBaseClass);

        Pool pool = (Pool)target;

        pool.nameAsTag = EditorGUILayout.Toggle("Use name as tag", pool.nameAsTag);
        if (!pool.nameAsTag)
        {
            pool.poolTag = EditorGUILayout.TextField("Pool tag", pool.poolTag);
        }

        pool.autoReturn = EditorGUILayout.Toggle("Use autoreturn", pool.autoReturn);
        if (pool.autoReturn)
        {
            pool.autoReturnDelay = EditorGUILayout.FloatField("Delay", pool.autoReturnDelay);
        }

        if (GUI.changed)
        {
            Undo.RecordObject(pool, "Test Scriptable Editor Modify");
            EditorUtility.SetDirty(pool);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
