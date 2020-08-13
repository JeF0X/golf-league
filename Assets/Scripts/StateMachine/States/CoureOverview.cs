using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoureOverview : State
{

    public static event Action OnCourseOverview;
    public CoureOverview(MatchManager matchManager) : base(matchManager)
    {
    }

    public override IEnumerator Enter()
    {
        if (OnCourseOverview != null)
        {
            OnCourseOverview();
        }

        CameraManager.Instance.PlayOverView();
        return base.Enter();
    }

    public override void Tick()
    {
        base.Tick();
    }
}
