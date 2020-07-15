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
            MatchManager.SetState(new End(MatchManager));
            MatchManager.startTimer = false;
        }
    }

    public override IEnumerator Enter()
    {
        MatchManager.startTimer = true;
        MatchManager.SetCamera();

        return base.Enter();
    }
}
