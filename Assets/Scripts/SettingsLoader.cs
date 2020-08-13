using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] MatchSettings matchSettings;
    [SerializeField] BallInfo defaultBallInfo;

    GameObject[] playerInfoGameobject;
    List<PlayerInfo> playerInfos = new List<PlayerInfo>();
    string courseName = "";
    Scene course;

    public void SetMatchSettings()
    {
        if (courseName == "")
        {
            Debug.LogError("No level selected");
            return;
        }
        playerInfoGameobject = GameObject.FindGameObjectsWithTag("PlayerInfo");
        FindPlayerInfo();
        matchSettings.SetSettings(GameType.MiniGolf, playerInfos, courseName);
        LoadLevel();
    }

    public void SetScene(string sceneName)
    {
        courseName = sceneName;
    }

    private void LoadLevel()
    {
        //Scene scene = SceneManager.GetSceneByName(courseName);
        SceneManager.LoadScene(courseName);
    }

    private void FindPlayerInfo()
    {
        for (int playerIndex = 0; playerIndex < playerInfoGameobject.Length; playerIndex++)
        {
            string name = playerInfoGameobject[playerIndex].GetComponentInChildren<TMP_InputField>().text;
            Color color = playerInfoGameobject[playerIndex].GetComponentInChildren<TMP_InputField>().colors.normalColor;
            BallInfo ballInfo = defaultBallInfo;
            int ballAmount = 1;
            List<BallInfo> balls = new List<BallInfo>();
            for (int ballIndex = 0; ballIndex < ballAmount; ballIndex++)
            {
                balls.Add(defaultBallInfo);
            }
            playerInfos.Add(new PlayerInfo(name, color, balls));
        }
    }
}
