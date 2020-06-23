using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string teamName = "TeamName";

    public int score = 0;
    Player[] players;
    Color color;

    public void IncrementScore()
    {
        score++;
    }
}
