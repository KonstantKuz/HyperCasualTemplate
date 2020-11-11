using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private GameObject winBanner;
    [SerializeField] private GameObject loseBanner;

    [SerializeField] private Animator star1;
    [SerializeField] private Animator star2;
    [SerializeField] private Animator star3;

    [SerializeField] private GameObject nextBtn;
    [SerializeField] private Text scoresLabel;

    private int rating;
    
    private void Start()
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
        star1.transform.GetChild(1).gameObject.SetActive(true);
        star1.SetBool("active", true);
        
        yield return new WaitForSeconds(0.6f);
        if (rating > 1)
        {
            star2.transform.GetChild(1).gameObject.SetActive(true);
            star2.SetBool("active", true);
        }
        
        yield return new WaitForSeconds(0.6f);
        if (rating > 2)
        {
            star3.transform.GetChild(1).gameObject.SetActive(true);
            star3.SetBool("active", true);
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
