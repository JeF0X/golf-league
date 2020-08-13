using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scores")]
public class Scores : ScriptableObject
{
    Player[] players;
    int[] shots;

    public Player GetPlayers(int index)
    {
        return players[index];
    }

    public int GetShots(int index)
    {
        return shots[index];
    }
}
