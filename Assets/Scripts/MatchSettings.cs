using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match Settings")]
public partial class MatchSettings : ScriptableObject
{
    public GameType gameType;
    public List<PlayerInfo> players = new List<PlayerInfo>();
    public string levelName = "";

    internal void SetSettings(GameType gameType, List<PlayerInfo> playerInfos, string levelName)
    {
        this.gameType = gameType;
        this.players = playerInfos;
        this.levelName  = levelName;
    }
}


