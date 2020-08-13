using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceBalls : State
{
    StartTouchHandler startTouchHandler = new StartTouchHandler();

    public static event Action OnPlaceBalls;

    public PlaceBalls(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        if (OnPlaceBalls != null)
        {
            OnPlaceBalls();
        }

        MatchManager.GetCurrentPlayer().startArea.MoveBallsToStartArea();
        MatchManager.GetCurrentPlayer().startArea.ActivateCamera();
        return base.Enter();
    }

    public override void Tick()
    {
        startTouchHandler.HandleTouchInput();
    }

    public override IEnumerator Exit()
    {
        MatchManager.FindBalls();
        return base.Exit();
    }
}
