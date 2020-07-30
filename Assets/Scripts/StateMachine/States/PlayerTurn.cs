using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
{
    public PlayerTurn(MatchManager matchManager) : base(matchManager)
    {
    }

    public override void Tick()
    {
        MatchManager.touchHandler.HandleTouchInput();
        if (MatchManager.matchTimer <= 0)
        {
            MatchManager.SetState(new MatchEnd(MatchManager));
            MatchManager.startTimer = false;
        }
    }

    public override IEnumerator Enter()
    {
        MatchManager.startTimer = true;
        MatchManager.SetCamera();
        if (MatchManager.AreAllBallsInHole())
        {
            foreach (var ball in MatchManager.Instance.balls)
            {
                ball.isInHole = false;
            }
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
