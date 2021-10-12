using Templates.ItemSystems.GiftSystem.UI;
using Templates.UI.RewardMultiplier;
using UnityEngine;

namespace Templates.UI
{
    public class FinishPanelsQueue : MonoBehaviour
    {
        [SerializeField] private RewardMultiplierPanel _rewardMultiplierPanel;
        [SerializeField] private GiftProgressUpdatePanel _giftProgressUpdatePanel;
        private void Awake()
        {
            Observer.Instance.OnWinLevel += ShowMultiplierPanel;
        }

        private void ShowMultiplierPanel()
        {
            int rewardForCurrentLevel = 125;
            _rewardMultiplierPanel.ShowPanel(rewardForCurrentLevel, ShowGiftPanel);
        }

        private void ShowGiftPanel()
        {
            _giftProgressUpdatePanel.ShowPanel(Observer.Instance.OnLoadNextScene);
        }
    }
}
