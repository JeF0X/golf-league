using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
{
    TouchHandler touchHandler = null;

    public static event Action OnPlayerTurn;

    public PlayerTurn(MatchManager matchManager) : base(matchManager)
    {
    }

    public override void Tick()
    {
        touchHandler.HandleTouchInput();
        if (MatchManager.matchTimer <= 0 && MatchManager.gameType == GameType.GolfLeague)
        {
            MatchManager.SetState(new MatchEnd(MatchManager));
            MatchManager.startTimer = false;
        }
    }

    public override IEnumerator Enter()
    {
        if (OnPlayerTurn != null)
        {
            OnPlayerTurn();
        }

        if (touchHandler == null)
        {
            touchHandler = new TouchHandler();
        }
        MatchManager.startTimer = true;
        MatchManager.SetCamera();
        if (MatchManager.AreAllBallsInHole())
        {
            MatchManager.Instance.NextHole();
            return base.Enter();
        }
        if (MatchManager.GetCurrentPlayer().AreAllBallsInHole())
        {
            MatchManager.ChangePlayer();
            MatchManager.SetState(new PlayerTurn(MatchManager));
        }

        return base.Enter();
    }
}
