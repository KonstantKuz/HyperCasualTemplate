using System.Linq;
using Templates.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Templates.LevelManagement
{
    public enum ScenesLoadType
    {
        Linear,
        Random,
        RandomAfterLinear,
    }
    
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private ScenesLoadType scenesLoadType;
        [SerializeField] private LevelsQueue levelsQueue;
        [Tooltip("Определяет будет ли увеличиваться счетчик уровней при прохождении бонус уровней.")]
        [SerializeField] private bool countBonusLevels;
    
        [Header("Editor Only")]
        public int currentLevelIndexToSet;
    
        private PlayerPrefsProperty<int> currentLevelIndexPrefs = new PlayerPrefsProperty<int>("CurrentLevelIndex", 0);
        private PlayerPrefsProperty<int> currentDisplayLevelNumberPrefs = new PlayerPrefsProperty<int>("CurrentDisplayLevelNumber", 1);
        private PlayerPrefsProperty<bool> isCircleOfLevelsPassedPrefs = new PlayerPrefsProperty<bool>("IsLevelCirclePassed", false);
    
        private const string BossLevelsNameContainingPart = "Boss";
        private const string BonusLevelsNameContainingPart = "Bonus";

        public PlayableLevel[] PlayableLevels => levelsQueue.levels;
        public int MaxLevelsCount => PlayableLevels.Length;
        public int FirstRepeatableLevelIndex => PlayableLevels.TakeWhile(level => !level.isRepeatable).Count();

        public int CurrentLevelIndex => currentLevelIndexPrefs.Value;
        public int CurrentDisplayLevelNumber => currentDisplayLevelNumberPrefs.Value;
    
        private void OnEnable()
        {
            Observer.Instance.OnLoadNextScene += LoadNextLevel;
            Observer.Instance.OnRestartScene += RestartScene;
        }

        private void OnDisable()
        {
            Observer.Instance.OnLoadNextScene -= LoadNextLevel;
            Observer.Instance.OnRestartScene -= RestartScene;
        }

        public void LoadLastLevel()
        {
            SceneManager.LoadScene(PlayableLevels[CurrentLevelIndex].sceneName);
        }
    
        public void SetCurrentLevelIndex(int index)
        {
            currentLevelIndexPrefs.Value = index;
        }
    
        private void LoadNextLevel()
        {
            IncreaseDisplayLevelNumber();
        
            int nextLevelIndex = UpdateLevelIndex();
            SceneManager.LoadScene(PlayableLevels[nextLevelIndex].sceneName);
        }

        private void IncreaseDisplayLevelNumber()
        {
            if (!countBonusLevels && IsBonusLevel(CurrentLevelIndex))
            {
                return;
            }

            currentDisplayLevelNumberPrefs.Value++;
        }

        private int UpdateLevelIndex()
        {
            int currentLevelIndex = CurrentLevelIndex;
        
            if (IsRandomSceneSelection())
            {
                currentLevelIndex = RandomNotThisLevel(currentLevelIndex);
            }
            else
            {
                currentLevelIndex = UpdateIndexAsLinear(currentLevelIndex);
            }
        
            SetCurrentLevelIndex(currentLevelIndex);
        
            Debug.Log($"<color=red> Saved current level index == {currentLevelIndex} </color>");

            return CurrentLevelIndex;
        }

        private bool IsRandomSceneSelection()
        {
            return scenesLoadType == ScenesLoadType.Random || 
                   scenesLoadType == ScenesLoadType.RandomAfterLinear && isCircleOfLevelsPassedPrefs.Value;
        }

        private int RandomNotThisLevel(int currentIndex)
        {
            int rndSceneIndex = Random.Range(0, MaxLevelsCount - 1);
            if (rndSceneIndex >= currentIndex)
            {
                rndSceneIndex++;
            }
            return rndSceneIndex;
        }

        private int UpdateIndexAsLinear(int currentIndex)
        {
            currentIndex++;
        
            if (currentIndex >= MaxLevelsCount)
            {
                currentIndex = 0;
                isCircleOfLevelsPassedPrefs.Value = true;
            }

            if (isCircleOfLevelsPassedPrefs.Value && !PlayableLevels[currentIndex].isRepeatable)
            {
                return UpdateIndexAsLinear(currentIndex);
            }

            return currentIndex;
        }
    
        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        public int GetNextLevelIndex()
        {
            int nextLevelIndex = CurrentLevelIndex + 1;
            return nextLevelIndex < MaxLevelsCount ? nextLevelIndex : FirstRepeatableLevelIndex;
        }

        public int GetLevelIndexFromLevelsCount(int levelsCount)
        {
            if (levelsCount < PlayableLevels.Length)
            {
                return levelsCount;
            }
        
            levelsCount -= PlayableLevels.Length;
            return levelsCount % (PlayableLevels.Length - FirstRepeatableLevelIndex);
        }

        public bool IsBossLevel(int levelIndex)
        {
            return PlayableLevels[levelIndex].sceneName.Contains(BossLevelsNameContainingPart);
        }

        public bool IsBonusLevel(int levelIndex)
        {
            return PlayableLevels[levelIndex].sceneName.Contains(BonusLevelsNameContainingPart);
        }
    }
}
