using UnityEngine;

namespace Templates.SwipeDetectorSystem
{
    public class SwipeLogger : MonoBehaviour
    {
        private void Awake()
        {
            SwipeDetector.Instance.OnSwipe += SwipeDetector_OnSwipe;
        }

        private void SwipeDetector_OnSwipe(SwipeData data)
        {
            Debug.Log("Swipe in Direction: " + data.Direction);
        }
    }
}