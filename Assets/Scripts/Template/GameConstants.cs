using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public const string PrefsCurrentScene = "currentLevel";
    public const string PrefsIsLevelCirclePassed = "isLevelCirclePassed";
    
    public const string TagPlayer = "Player";
    public const string TagFinishTrigger = "Finish";
    public const string TagDefeatTrigger = "Defeat";

    public const int LayerGround = 8;
    
    private void Start()
    {
        AnimatorHashes.CacheHashes();
    }
}

public class AnimatorHashes
{
    public static int Idle;
    public static void CacheHashes()
    {
        Idle = Animator.StringToHash("Idle");
    }
}
