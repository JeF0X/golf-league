using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    protected MatchManager MatchManager;

    public State(MatchManager matchManager)
    {
        MatchManager = matchManager;
    }

    public virtual IEnumerator Enter()
    {
        Debug.Log(GetType());
        yield break;
    }

    public virtual void Tick()
    {
        return;
    }

    public virtual IEnumerator Exit()
    {
        yield break;
    }
}
