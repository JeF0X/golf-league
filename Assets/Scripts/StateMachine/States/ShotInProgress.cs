using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotInProgress : State
{
    public static event Action OnShotInProgress;

    public ShotInProgress(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        MatchManager.DebugScores();
        MatchManager.GetCurrentPlayer().AddShot();
        if (OnShotInProgress != null)
        {
            OnShotInProgress();
        }
        return base.Enter();
    }

    public override void Tick()
    {
        if (!MatchManager.AreBallsMoving())
        {
            MatchManager.SaveBallPositions();
            MatchManager.ChangePlayer();
            MatchManager.SetState(new PlayerTurn(MatchManager));
        }

        if (MatchManager.matchTimer <= 0)
        {
            MatchManager.startTimer = false;
        }
        base.Tick();
    }
}
