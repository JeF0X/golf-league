using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBalls : State
{
    StartTouchHandler startTouchHandler = new StartTouchHandler();
    public PlaceBalls(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        MatchManager.GetCurrentPlayer().startArea.MoveBallsToStartArea();
        MatchManager.GetCurrentPlayer().startArea.ActivateCamera();
        yield break;
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
