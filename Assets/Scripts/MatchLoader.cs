using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLoader : MonoBehaviour
{
    [SerializeField] MatchSettings matchSettings = null;
    [SerializeField] Player playerPrefab = null;

    Player[] players;

    private void Awake()
    {
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
        foreach (var playerInfo in matchSettings.players)
        {
            Player player = playerPrefab;
            player.SetPlayerInfo(playerInfo);
            Instantiate(player);
        }
    }
}
