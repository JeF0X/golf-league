using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MatchUI : MonoBehaviour
{
    [SerializeField] TMP_Text[] teamScores = null;
    [SerializeField] TMP_Text timer = null;
    [SerializeField] TMP_Text winner = null;


    UIElement[] uIElements;
    List<Team> teams = new List<Team>();

    private void Start()
    {
        MatchStart.OnMatchStart += MatchStart_OnMatchStart;
        CoureOverview.OnCourseOverview += CoureOverview_OnCourseOverview;
        PlaceBalls.OnPlaceBalls += PlaceBalls_OnPlaceBalls;
        PlayerTurn.OnPlayerTurn += PlayerTurn_OnPlayerTurn;
        ShotInProgress.OnShotInProgress += ShotInProgress_OnShotInProgress;
        MatchEnd.OnMatchEnd += MatchEnd_OnMatchEnd;

        uIElements = GetComponentsInChildren<UIElement>();
        teams = MatchManager.Instance.teams;
        winner.enabled = false;
    }

    private void Update()
    {
        if (MatchManager.Instance.gameType == GameType.GolfLeague)
        {
            timer.text = Mathf.RoundToInt(MatchManager.Instance.matchTimer).ToString();
        }
        else
        {
            timer.gameObject.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    private void MatchStart_OnMatchStart()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onMatchStart);
        }
    }

    private void CoureOverview_OnCourseOverview()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onCourseOverview);
        }
    }

    private void PlaceBalls_OnPlaceBalls()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onPlaceBalls);
        }
    }

    private void PlayerTurn_OnPlayerTurn()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onPlayerTurn);
        }
    }

    private void ShotInProgress_OnShotInProgress()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onShotInProgress);
        }
    }

    private void MatchEnd_OnMatchEnd()
    {
        foreach (var element in uIElements)
        {
            element.gameObject.SetActive(element.onMatchEnd);
        }

        winner.text = MatchManager.Instance.DebugScores();
        winner.enabled = true;
    }

    private void OnDestroy()
    {
        MatchStart.OnMatchStart -= MatchStart_OnMatchStart;
        CoureOverview.OnCourseOverview -= CoureOverview_OnCourseOverview;
        PlaceBalls.OnPlaceBalls -= PlaceBalls_OnPlaceBalls;
        PlayerTurn.OnPlayerTurn -= PlayerTurn_OnPlayerTurn;
        ShotInProgress.OnShotInProgress -= ShotInProgress_OnShotInProgress;
        MatchEnd.OnMatchEnd -= MatchEnd_OnMatchEnd;
    }
}
