using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MatchUI : MonoBehaviour
{
    [SerializeField] TMP_Text[] teamScores = null;
    [SerializeField] TMP_Text timer = null;
    [SerializeField] TMP_Text winner = null;

    List<Team> teams = new List<Team>();

    private void Start()
    {
        Hole.OnHoleEntered += Hole_OnHoleEntered;
        teams = MatchManager.Instance.teams;
        winner.enabled = false;
        StartCoroutine(UpdateScoreUI());
    }

    private void Update()
    {
        timer.text = Mathf.RoundToInt(MatchManager.Instance.matchTimer).ToString();

        if (MatchManager.Instance.matchState == MatchState.End)
        {
            winner.text = MatchManager.Instance.DebugScores();
            winner.enabled = true;
        }
    }

    private void Hole_OnHoleEntered(Ball ball, Hole hole)
    {
        StartCoroutine(UpdateScoreUI());
    }

    IEnumerator UpdateScoreUI()
    {
        yield return new WaitForSeconds(0.5f);
        for (int teamIndex = 0; teamIndex < teams.Count; teamIndex++)
        {
            teamScores[teamIndex].text = teams[teamIndex].score.ToString();
        }
    }

    private void OnDestroy()
    {
        Hole.OnHoleEntered -= Hole_OnHoleEntered;
    }
}
