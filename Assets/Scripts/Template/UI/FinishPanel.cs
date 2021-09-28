using System.Collections;
using UnityEngine;

namespace Template.UI
{
    public class FinishPanel : MonoBehaviour
    {
        [SerializeField] private GameObject winBanner;
        [SerializeField] private GameObject loseBanner;

        [SerializeField] private FinishStar[] stars;

        private const int WIN_ACTIVE_STARS_COUNT = 3;
        private const int LOSE_ACTIVE_STARS_COUNT = 1;
        private const float DELAY_BTWN_STARS_SHOW = 0.6f;
    
        private void Start()
        {
            winBanner.SetActive(false);
            loseBanner.SetActive(false);
            gameObject.SetActive(false);
        
            Observer.Instance.OnWinLevel += delegate { OnGameOver(true); };
            Observer.Instance.OnLoseLevel += delegate { OnGameOver(false); };
        }

        private void OnGameOver(bool isWon)
        {
            gameObject.SetActive(true);
            winBanner.SetActive(isWon);
            loseBanner.SetActive(!isWon);

            StartCoroutine(AnimateStars(isWon));
        }

        private IEnumerator AnimateStars(bool isWon)
        {
            foreach (FinishStar star in stars)
            {
                star.SetVisible(false);
            }
        
            int activeStarsCount = isWon ? WIN_ACTIVE_STARS_COUNT : LOSE_ACTIVE_STARS_COUNT;
        
            for (int i = 0; i < stars.Length && i < activeStarsCount; i++) 
            {
                stars[i].SetVisible(true); 
                yield return new WaitForSeconds(DELAY_BTWN_STARS_SHOW);
            }
        }

        public void Next()
        {
            Observer.Instance.OnLoadNextScene();
            gameObject.SetActive(false);
        }

        public void Restart()
        {
            Observer.Instance.OnRestartScene();
            gameObject.SetActive(false);
        }
    }
}
