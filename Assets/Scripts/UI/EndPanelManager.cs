using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndPanelManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI levelText;

    private LevelManager levelManager;

    private void Start()
	{
		levelText.SetText("Level " + levelManager.CurrentLevel);
	}
}
