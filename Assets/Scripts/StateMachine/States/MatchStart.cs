using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStart : State
{
    public MatchStart(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        yield return new WaitForSeconds(0.1f);
        MatchManager.InitializeMatch();
        MatchManager.SetState(new PlaceBalls(MatchManager));
        yield break;
    }
}
