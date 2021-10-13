using System;
using UnityEngine;

namespace Templates.LevelManagement
{
    [CreateAssetMenu(fileName = "Playable levels")]
    public class LevelsQueue : ScriptableObject
    {
        public PlayableLevel[] levels;
    }

    [Serializable]
    public class PlayableLevel
    {
        public string sceneName;
        public bool isRepeatable = true;
    }
}