using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public const string PrefsCurrentScene = "currentLevel";

    private void Start()
    {
        AnimatorHashes.CacheHashes();
    }
}

public class AnimatorHashes
{
    public static int IdleHash;
    
    public static void CacheHashes()
    {
        IdleHash = Animator.StringToHash("Idle");
    }
}
