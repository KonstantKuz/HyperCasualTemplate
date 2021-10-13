using UnityEditor;
using UnityEngine;

namespace Templates.LevelManagement.Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();
        
            LevelManager levelManager = (LevelManager) target;
        
            if (GUILayout.Button("Set current level index"))
            {
                levelManager.SetCurrentLevelIndex(levelManager.currentLevelIndexToSet);
            }
        
            if (GUI.changed)
            {
                Undo.RecordObject(levelManager, $"LevelManager Modify");
                EditorUtility.SetDirty(levelManager);
            }
        }
    }
}
