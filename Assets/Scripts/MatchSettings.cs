using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Match Settings")]
public partial class MatchSettings : ScriptableObject
{
    public PlayerInfo[] players;
    public string levelName = "";
}


