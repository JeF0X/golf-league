using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoureOverview : State
{
    public CoureOverview(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        MatchManager.SetState(new PlaceBalls(MatchManager));
        return base.Enter();
    }
}
