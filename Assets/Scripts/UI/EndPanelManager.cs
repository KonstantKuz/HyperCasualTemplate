using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPanelManager : MonoBehaviour, ServiceRequester
{
	[SerializeField] private TextMeshProUGUI levelText;

    private LevelManager levelManager;

    private void OnEnable()
    {
        CacheNecessaryService();
    }

    public void CacheNecessaryService()
    {
        levelManager = ServiceLocator.Instance.Get<LevelManager>();
    }

    private void Start()
	{
		levelText.SetText("Level " + levelManager.CurrentLevel);
	}
}
