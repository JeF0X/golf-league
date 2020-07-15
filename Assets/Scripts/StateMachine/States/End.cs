﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class End : State
{

    public static event Action OnMatchEnd;

    public End(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        
        MatchManager.DebugScores();
        if (OnMatchEnd != null)
        {
            OnMatchEnd();
        }
        return base.Enter();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
