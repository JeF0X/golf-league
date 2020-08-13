using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OverviewPlayer : MonoBehaviour
{
    public void OnOverviewFinished() 
    {
        GetComponent<Animator>().SetBool("hasFinished", true);
        MatchManager.Instance.SetState(new PlaceBalls(MatchManager.Instance));
    }

    public void OnOverviewStart()
    {
        CameraManager.Instance.SetActiveCamera(GetComponent<CinemachineVirtualCamera>());
    }
}
