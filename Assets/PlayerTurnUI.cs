using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnUI : MonoBehaviour
{
    [SerializeField] Button changeBallsButton;

    private void OnEnable()
    {
        if (!MatchManager.Instance)
        {
            return;
        }
        if (MatchManager.Instance.GetCurrentPlayer().balls.Count <= 1)
        {
            changeBallsButton.gameObject.SetActive(false);
        }
    }
}
