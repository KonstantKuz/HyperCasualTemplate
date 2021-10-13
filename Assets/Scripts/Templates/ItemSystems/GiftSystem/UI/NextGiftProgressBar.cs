using System.Collections;
using TMPro;
using UnityEngine;

namespace Templates.ItemSystems.GiftSystem.UI
{
    public class NextGiftProgressBar : GOLineProgressBar
    {
        [SerializeField] private TextMeshProUGUI progressText;

        private float _lastProgress;
        private float _currentProgress;

        public void Initialize(float giftReceiveProgress)
        {
            _lastProgress = giftReceiveProgress / 100;
            
            progressText.SetText($"{_lastProgress * 100}%");

            Initialize(0, 1, _lastProgress);
        }

        public void UpdateVisualProgress(float giftReceiveProgress)
        {
            _currentProgress = Mathf.Clamp(giftReceiveProgress / 100, 0, 1);
            UpdateCurrentProgress(_currentProgress);
     
            AnimateProgressText();
        }
        
        private void AnimateProgressText()
        {
            StartCoroutine(AnimateStats());
            IEnumerator AnimateStats()
            {
                float timer = 0;
                float animationTime = 1f;
                while (timer < animationTime)
                {
                    timer += Time.fixedDeltaTime;
                    progressText.SetText($"{(int)Mathf.Lerp(_lastProgress * 100, _currentProgress * 100, timer/animationTime)}%");
                    yield return new WaitForFixedUpdate();
                }

                _lastProgress = _currentProgress;
            }
        }
    }
}
