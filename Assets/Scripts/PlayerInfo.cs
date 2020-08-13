using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string playerName = "";
    public Color playerColor;
    public List<BallInfo> balls;

    public PlayerInfo(string playerName, Color playerColor, List<BallInfo> balls)
    {
        this.playerName = playerName;
        this.playerColor = playerColor;
        this.balls = balls;
    }
}



