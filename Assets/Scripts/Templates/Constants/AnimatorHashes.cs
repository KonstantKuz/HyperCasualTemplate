using UnityEngine;

namespace Templates.Constants
{
    public static class AnimatorHashes
    {
        public static int Idle;
    
        public static int FinishStarPopup;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CacheHashes()
        {
            Idle = Animator.StringToHash("Idle");
            FinishStarPopup = Animator.StringToHash("FinishStarPopup");
        }
    }
}