using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
