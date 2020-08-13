using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeBallButton : MonoBehaviour
{
    private void Start()
    {
        PlayerTurn.OnPlayerTurn += PlayerTurn_OnPlayerTurn;
    }


    private void PlayerTurn_OnPlayerTurn()
    {
        if (MatchManager.Instance.GetCurrentPlayer().balls.Count <= 1)
        {
            gameObject.SetActive(false);
        }
    }
}
