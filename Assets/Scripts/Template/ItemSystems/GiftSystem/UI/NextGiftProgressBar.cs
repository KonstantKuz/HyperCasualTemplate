using System.Collections;
using Template.LevelManagement;
using Template.Tools;
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
        private float _currentProgress;

        public void Initialize(float giftReceiveProgress)
        {
            _lastProgress = giftReceiveProgress / 100;
            
            progressText.SetText($"{_lastProgress * 100}%");

            _initialData.MinValue = 0;
            _initialData.MaxValue = 1;
            _initialData.CurrentValue = _lastProgress;
        
            Initialize(_initialData);
        }

        public void UpdateVisualProgress(float giftReceiveProgress)
        {
            _currentProgress = giftReceiveProgress / 100;
            _currentProgress = Mathf.Clamp(_currentProgress, 0, 1);
         
            AnimateProgressText();

            _updateData.CurrentValue = _currentProgress;
            UpdateCurrentProgress(_updateData);
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
