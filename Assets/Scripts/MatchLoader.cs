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
    PlayerScore[] playerScores;

    private void Start()
    {
        MatchManager.Instance.gameType = matchSettings.gameType;
        startAreas = MatchManager.Instance.startAreas;
        playerScores = FindObjectsOfType<PlayerScore>();
        CreatePlayers();
    }

    public void CreatePlayers()
    {
        if (matchSettings.gameType == GameType.GolfLeague)
        {
            int startAreaIndex = 0;
            for (int playerIndex = 0; playerIndex < matchSettings.players.Count; playerIndex++)
            {
                Player player = playerPrefab;
                player.SetPlayerInfo(matchSettings.players[playerIndex], startAreas[startAreaIndex], playerScores[playerIndex]);
                Instantiate(player);
                startAreaIndex++;
            }

        }
        else if (matchSettings.gameType == GameType.MiniGolf)
        {
            int startAreaIndex = 0;

            for (int playerIndex = 0; playerIndex < matchSettings.players.Count; playerIndex++)
            {
                Player player = playerPrefab;
                player.SetPlayerInfo(matchSettings.players[playerIndex], startAreas[0], playerScores[playerIndex]);
                Instantiate(player);
                startAreaIndex++;
            }
        }
        
    }
}
