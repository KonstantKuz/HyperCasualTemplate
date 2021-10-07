using Templates.Constants;
using UnityEngine;

namespace Templates.UI
{
    public class FinishStar : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject starToActivate;

        public void SetVisible(bool value)
        {
            animator.SetBool(AnimatorHashes.FinishStarPopup, value);
            starToActivate.SetActive(value);
        }
    }
}
