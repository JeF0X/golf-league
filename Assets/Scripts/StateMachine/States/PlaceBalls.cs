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
        yield break;
    }

    public override void Tick()
    {
        startTouchHandler.HandleTouchInput();
        MatchManager.GetCurrentPlayer().startArea.InstantiatePlayerBalls();
    }

    public override IEnumerator Exit()
    {
        MatchManager.FindBalls();
        return base.Exit();
    }
}
