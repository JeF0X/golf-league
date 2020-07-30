using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Match Settings")]
public partial class MatchSettings : ScriptableObject
{
    public GameType gameType;
    public PlayerInfo[] players;
    public string levelName = "";
}


