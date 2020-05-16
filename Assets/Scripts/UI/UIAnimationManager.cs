using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UIAnimationManager : MonoBehaviour, ServiceRequester, ObserverSubscriber
{
	private GameObject confettiVFX;
    private GameObject fireworkVFX;
    private StimulationText stimulText;

    public Observer observer { get; private set; }

    private UIManager uiManager;
    private UIEffectsHolder uiEffectsHolder;

    private void OnEnable()
	{
        CacheNecessaryService();
        SubscribeToNecessaryEvets();
    }

    public void CacheNecessaryService()
    {
        observer = ServiceLocator.Instance.Get<Observer>();
        uiManager = ServiceLocator.Instance.Get<UIManager>();
        uiEffectsHolder = ServiceLocator.Instance.Get<UIEffectsHolder>();
    }

    public void SubscribeToNecessaryEvets()
    {
        observer.OnLoadMainMenu += ShowMainMenu;
        observer.OnStartGame += CloseMainMenu;
        observer.OnFinish += ShowWinPanel;
        observer.OnFinish += delegate { StartCoroutine(PlayUIVFX(confettiVFX, 0f, 2.5f)); };
        observer.OnPlayerDie += SlideLosePanel;
        observer.OnGetStimulationText += ShowStimulationText;
    }

    private void Start()
    {
        CacheVFX();
    }

    private void CacheVFX()
    {
        confettiVFX = uiEffectsHolder.confettiVFX;
        fireworkVFX = uiEffectsHolder.fireworkVFX;
        stimulText = uiEffectsHolder.stimulText.GetComponentInChildren<StimulationText>();
    }

    private void ShowMainMenu()
	{
		uiManager.mainMenuPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.8f);
	}
	
	private void CloseMainMenu()
	{
        uiManager.mainMenuPanel.GetComponent<RectTransform>().DOAnchorPosY(4000, 2f);
	}

	private void ShowWinPanel()
	{
        uiManager.winPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.8f);
	}

	private void SlideLosePanel()
	{
        uiManager.losePanel.GetComponent<RectTransform>().DOAnchorPosX(0, 0.8f);
    }

    private void ShowStimulationText(StimulType stimulationTextType)
    {
        stimulText.SetTextAndPlay(stimulationTextType);
        if (stimulationTextType == StimulType.Insane)
            StartCoroutine(PlayUIVFX(fireworkVFX, 0.5f, 0.8f));
    }

    private IEnumerator PlayUIVFX(GameObject vfx, float delay, float playTime)
    {
        yield return new WaitForSeconds(delay);
        vfx.SetActive(true);
        yield return new WaitForSeconds(playTime);
        vfx.SetActive(false);
    }
}
