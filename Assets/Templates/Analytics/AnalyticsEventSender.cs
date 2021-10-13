using System;
using UnityEngine;
using UnityEngine.SceneManagement;
//using GameAnalyticsSDK;

public class AnalyticsEventSender : MonoBehaviour
{
    private string CurrentSceneName;
    private bool isPassedScene;
    private bool isFailedScene;
/* ------------------------------------------- delete this line
private void OnEnable()
{
    CurrentSceneName = SceneManager.GetActiveScene().name;
    
    Observer.Instance.OnStartGame += OnStartGame;
    Observer.Instance.OnLoseLevel += OnLoseLevel;
    Observer.Instance.OnWinLevel += OnWinLevel;
    
    Observer.Instance.OnLoadNextScene += OnLoadNextScene;
    Observer.Instance.OnRestartScene += OnRestartScene;
}

private void OnStartGame()
{
    SendSceneStateData(StateEvents.Started, StatePrefs.StartedCount);
}

private void OnLoseLevel()
{
    isFailedScene = true;
    SendSceneStateData(StateEvents.Failed, StatePrefs.FailedCount);
}

private void OnWinLevel()
{
    isPassedScene = true;
    if (IsScenePassedByFirstAttempt())
    {
        SendScenePassedByFirstAttempt();
    }

    SendSceneStateData(StateEvents.Passed, StatePrefs.PassedCount);
}

private void SendSceneStateData(string stateName, string countName)
{
    int enteredInto_stateName_OnThisScene_Count = PlayerPrefs.GetInt(CurrentScene(countName));
    enteredInto_stateName_OnThisScene_Count++;
    PlayerPrefs.SetInt(CurrentScene(countName), enteredInto_stateName_OnThisScene_Count);
    GameAnalytics.NewDesignEvent(CurrentScene(stateName));
    
    SetStatePassedInThisScene(stateName);
    
    Debug.Log($"<color=blue>Send event {CurrentScene(stateName)} with count == {enteredInto_stateName_OnThisScene_Count}</color>");
}

private string CurrentScene(string value)
{
    return CurrentSceneName + value;
}

private void SetStatePassedInThisScene(string stateName)
{
    PlayerPrefs.SetInt(CurrentScene(stateName + StatePrefs.PassedAtLeastOnce), 1);
}

private bool IsStatePassedInThisScene(string stateName)
{
    if (PlayerPrefs.GetInt(CurrentScene(stateName + StatePrefs.PassedAtLeastOnce)) == 1)
        return true;
    else
        return false;
}

private bool IsScenePassedByFirstAttempt()
{
    if (!IsStatePassedInThisScene(StateEvents.Failed) && !IsStatePassedInThisScene(StateEvents.Passed))
        return true;
    else
        return false;
}

private void SendScenePassedByFirstAttempt()
{
    GameAnalytics.NewDesignEvent(CurrentScene(StatePrefs.PassedByFirstAttempt));
    
    Debug.Log($"<color=blue>Send event {CurrentScene(StatePrefs.PassedByFirstAttempt)}</color>");
}

private void OnLoadNextScene()
{
    SendNextClicked();
}

private void SendNextClicked()
{
    int clickedNextCount = PlayerPrefs.GetInt(ButtonPrefs.NextClickedCount);
    clickedNextCount++;
    PlayerPrefs.SetInt(ButtonPrefs.NextClickedCount, clickedNextCount);
    GameAnalytics.NewDesignEvent(ButtonEvents.NextClicked);
    
    Debug.Log($"<color=blue>Send event {ButtonEvents.NextClicked} with count == {clickedNextCount}</color>");
}

private void OnRestartScene()
{
    if (isFailedScene)
    {
        SendRestartClickedAfterFailed();
    }
    if (isPassedScene)
    {
        SendRestartClickedAfterPassed();
    }
}

private void SendRestartClickedAfterFailed()
{
    int clickedAfterFailedCount = PlayerPrefs.GetInt(ButtonPrefs.RestartClickedAfterFailedCount);
    clickedAfterFailedCount++;
    PlayerPrefs.SetInt(ButtonPrefs.RestartClickedAfterFailedCount, clickedAfterFailedCount);
    GameAnalytics.NewDesignEvent(ButtonEvents.RestartClickedAfterFailed);
    
    Debug.Log($"<color=blue>Send event {ButtonEvents.RestartClickedAfterFailed} with count == {clickedAfterFailedCount}</color>");
}

private void SendRestartClickedAfterPassed()
{
    int clickedAfterPassedCount = PlayerPrefs.GetInt(ButtonPrefs.RestartClickedAfterPassedCount);
    clickedAfterPassedCount++;
    PlayerPrefs.SetInt(ButtonPrefs.RestartClickedAfterPassedCount, clickedAfterPassedCount);
    GameAnalytics.NewDesignEvent(ButtonEvents.RestartClickedAfterPassed);
    
    Debug.Log($"<color=blue>Send event {ButtonEvents.RestartClickedAfterPassed} with count == {clickedAfterPassedCount}</color>");
}
------------------------------------------- delete this line */

#if UNITY_EDITOR
[ContextMenu("Clean All Prefs")]
public void CleanAllPrefs()
{
    for (int i = 0; i < UnityEditor.EditorBuildSettings.scenes.Length; i++)
    {
        string sceneName = UnityEditor.EditorBuildSettings.scenes[i].path;
        sceneName = sceneName.Remove(0, 14);
        sceneName = sceneName.Remove(sceneName.Length - 6, 6);
        Debug.Log($"Clean all prefs with {sceneName} scene");
        
        PlayerPrefs.DeleteKey(sceneName + StatePrefs.PassedAtLeastOnce);
        PlayerPrefs.DeleteKey(sceneName + StatePrefs.PassedByFirstAttempt);
        PlayerPrefs.DeleteKey(sceneName + StatePrefs.StartedCount);
        PlayerPrefs.DeleteKey(sceneName + StatePrefs.PassedCount);
        PlayerPrefs.DeleteKey(sceneName + StatePrefs.FailedCount);
    }
    PlayerPrefs.DeleteKey(ButtonPrefs.NextClickedCount);
    PlayerPrefs.DeleteKey(ButtonPrefs.RestartClickedAfterFailedCount);
    PlayerPrefs.DeleteKey(ButtonPrefs.RestartClickedAfterPassedCount);
}
#endif
}

internal class StateEvents
{
// in dashboard Events -> Design
    
// Final event names : A1SceneStarted, 2ScenePassed, A5SceneFailed...
// Parameter : int X times per player
// Meaning : Scene A1 was started X times per player
public const string Started = "SceneStarted"; // with count
public const string Passed = "ScenePassed";   // with count
public const string Failed = "SceneFailed";   // with count
}

internal class StatePrefs
{
public const string PassedByFirstAttempt = "ScenePassedByFirstAttempt"; // without count
public const string PassedAtLeastOnce = "PassedAtLeastOnce"; // 1 == true
    
public const string StartedCount = "SceneStartedCount";
public const string PassedCount = "ScenePassedCount";
public const string FailedCount = "SceneFailedCount";
}

internal class ButtonEvents
{
public const string NextClicked = "NextClicked"; // with count
public const string RestartClickedAfterFailed = "RestartClickedAfterFailed"; // with count
public const string RestartClickedAfterPassed = "RestartClickedAfterPassed"; // with count
}

internal class ButtonPrefs
{
public const string NextClickedCount = "NextClickedCount";
public const string RestartClickedAfterFailedCount = "RestartClickedAfterFailedCount";
public const string RestartClickedAfterPassedCount = "RestartClickedAfterPassedCount";
}

