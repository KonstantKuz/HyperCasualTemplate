using System.Collections;
using Template.LevelManagement;
using TMPro;
using UnityEngine;

namespace Template.ItemSystems.GiftSystem.UI
{
    public class NextGiftProgressBar : SceneLineProgressBar<NextGiftProgressBar>
    {
        [SerializeField] private TextMeshProUGUI progressText;

        private InitialData<NextGiftProgressBar> _initialData;
        private UpdateData<NextGiftProgressBar> _updateData;
    
        private float _lastProgress;
        public void Initialize()
        {
            float levelsReached = GiftGiver.Instance.LevelsReachedToReceiveLastLockedGift();
            float levelsCountToReceive = GiftGiver.Instance.LevelsCountToReceiveLastLockedGift();
            _lastProgress = levelsReached / levelsCountToReceive;
            
            progressText.SetText($"{_lastProgress * 100}%");

            _initialData.MinValue = 0;
            _initialData.MaxValue = 1;
            _initialData.CurrentValue = _lastProgress;
        
            Initialize(_initialData);
        }

        public void UpdateProgress()
        {
            float levelsReached = GiftGiver.Instance.LevelsReachedToReceiveLastLockedGift() + 1;
            float levelsCountToReceive = GiftGiver.Instance.LevelsCountToReceiveLastLockedGift();
            float currentProgress = levelsReached / levelsCountToReceive;
            
            currentProgress = Mathf.Clamp(currentProgress, 0, 1);
         
            AnimateProgressText(currentProgress);

            _updateData.CurrentValue = currentProgress;
            UpdateCurrentProgress(_updateData);
        }
    
        private void AnimateProgressText(float currentProgress)
        {
            StartCoroutine(AnimateStats());
            IEnumerator AnimateStats()
            {
                float timer = 0;
                float animationTime = 1f;
                while (timer < animationTime)
                {
                    timer += Time.fixedDeltaTime;
                    progressText.SetText($"{(int)Mathf.Lerp(_lastProgress * 100, currentProgress * 100, timer/animationTime)}%");
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}
