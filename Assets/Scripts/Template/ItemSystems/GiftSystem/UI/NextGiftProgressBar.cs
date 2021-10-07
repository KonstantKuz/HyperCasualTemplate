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

        private GiftItem _nextGift;
        private int _currentLevelNumber;

        private PlayerPrefsProperty<string> _nextGiftName = new PlayerPrefsProperty<string>("NextGiftName", "");
        private PlayerPrefsProperty<int> _nextGiftLevelsCountToReceive = new PlayerPrefsProperty<int>("NextGiftLevelsCountToReceive", 0);
        private PlayerPrefsProperty<int> _nextGiftProgressStartLevel= new PlayerPrefsProperty<int>("NextGiftProgressStartLevel", 0);
        
        private float _lastProgress;

        public void Initialize()
        {
            _nextGift = GiftGiver.Instance.NextGift();
            _currentLevelNumber = LevelManager.Instance.CurrentDisplayLevelNumber;
            
            if (_nextGiftName.Value != _nextGift.Name)
            {
                _nextGiftName.Value = _nextGift.Name;
                _nextGiftLevelsCountToReceive.Value = _nextGift.DefaultReceiveLevel - _currentLevelNumber;
                _nextGiftProgressStartLevel.Value = _currentLevelNumber;
            }
            
            float levelsCountToReceive = _nextGiftLevelsCountToReceive.Value;
            float levelsCountReached = _currentLevelNumber + _nextGift.ReceiveLevelOffset - _nextGiftProgressStartLevel.Value;

            _lastProgress = Mathf.Abs(levelsCountReached) / levelsCountToReceive;
            
            progressText.SetText($"{_lastProgress * 100}%");

            _initialData.MinValue = 0;
            _initialData.MaxValue = 1;
            _initialData.CurrentValue = _lastProgress;
        
            Initialize(_initialData);
        }

        public void IncreaseVisualProgress()
        {
            _currentLevelNumber++;
            UpdateVisualProgress();
        }
        
        public void UpdateVisualProgress()
        {
            float levelsCountToReceive = _nextGiftLevelsCountToReceive.Value;
            float levelsCountReached = _currentLevelNumber + _nextGift.ReceiveLevelOffset - _nextGiftProgressStartLevel.Value;
            
            float currentProgress = Mathf.Abs(levelsCountReached) / levelsCountToReceive;
            
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
