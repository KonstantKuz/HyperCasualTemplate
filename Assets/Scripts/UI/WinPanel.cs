using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField]
    GameObject winBanner;
    [SerializeField]
    GameObject loseBanner;

    [SerializeField]
    Animator animator1;
    [SerializeField]
    Animator animator2;
    [SerializeField]
    Animator animator3;

    [SerializeField]
    GameObject nextBtn;
    [SerializeField]
    Text scoresLabel;

    private int rating;
    
    void Start()
    {
        winBanner.SetActive(false);
        loseBanner.SetActive(false);
        gameObject.SetActive(false);
        
        Observer.Instance.OnWinLevel += delegate { OnGameOver(3); };
        Observer.Instance.OnLoseLevel += delegate { OnGameOver(1); };
    }

    private void OnGameOver(int rating)
    {
        this.rating = rating;
        gameObject.SetActive(true);
        if(this.rating < 3)
        {
            nextBtn.SetActive(false);
            loseBanner.SetActive(true);
        }
        else
        {
            winBanner.SetActive(true);
        }

        StartCoroutine(AnimateStars());
    }

    private IEnumerator AnimateStars()
    {
        animator1.transform.GetChild(1).gameObject.SetActive(true);
        animator1.SetBool("active", true);
        
        yield return new WaitForSeconds(0.6f);
        if (rating > 1)
        {
            animator2.transform.GetChild(1).gameObject.SetActive(true);
            animator2.SetBool("active", true);
        }
        
        yield return new WaitForSeconds(0.6f);
        if (rating > 2)
        {
            animator3.transform.GetChild(1).gameObject.SetActive(true);
            animator3.SetBool("active", true);
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
