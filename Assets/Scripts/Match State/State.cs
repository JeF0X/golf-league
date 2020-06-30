using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected MatchStateMachine MatchStateMachine;

    public State(MatchStateMachine matchStateMachine)
    {
        MatchStateMachine = matchStateMachine;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }
}
