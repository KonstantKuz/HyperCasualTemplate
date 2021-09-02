using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
