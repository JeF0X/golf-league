using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match Settings")]
public class MatchSettings : ScriptableObject
{
    [System.Serializable]
    public class PlayerInfo
    {
        public string playerName = "";
        public Color playerColor;
    }

    public PlayerInfo[] players;
    public string levelName = "";
}


