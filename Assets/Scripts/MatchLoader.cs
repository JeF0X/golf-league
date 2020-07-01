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

    private void Awake()
    {
        startAreas = FindObjectsOfType<StartArea>();
        CreatePlayers();
    }

    private void Start()
    {

        //InitializeMatch();
        MatchManager.Instance.matchState = MatchState.Start;
    }

    private void InitializeMatch()
    {
        throw new NotImplementedException();
    }

    public void CreatePlayers()
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
}
