using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLoader : MonoBehaviour
{
    [SerializeField] MatchSettings matchSettings = null;
    [SerializeField] Player playerPrefab = null;

    Player[] players;
    StartArea[] startAreas;

    private void Start()
    {
        MatchManager.Instance.gameType = matchSettings.gameType;
        startAreas = MatchManager.Instance.startAreas;
        CreatePlayers();
    }

    public void CreatePlayers()
    {
        if (matchSettings.gameType == GameType.GolfLeague)
        {
            int startAreaIndex = 0;
            foreach (var playerInfo in matchSettings.players)
            {
                Player player = playerPrefab;
                player.SetPlayerInfo(playerInfo, startAreas[startAreaIndex]);
                Instantiate(player);
                startAreaIndex++;
            }
        }
        else if (matchSettings.gameType == GameType.MiniGolf)
        {
            int startAreaIndex = 0;
            foreach (var playerInfo in matchSettings.players)
            {
                Player player = playerPrefab;
                player.SetPlayerInfo(playerInfo, startAreas[0]);
                Instantiate(player);
                startAreaIndex++;
            }
        }
        
    }
}
