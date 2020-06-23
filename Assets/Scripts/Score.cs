using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    List<Goal> goals = new List<Goal>();
    Dictionary<Player, int> scores = new Dictionary<Player, int>();

    public void AddGoal(Goal goal)
    {
        goals.Add(goal);
    }

    public void DebugScores()
    {
    }

    private void ParseScores()
    {
    }
}
