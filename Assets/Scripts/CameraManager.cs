using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera[] cameras;

    private static CameraManager _instance;

    public static CameraManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        cameras = FindObjectsOfType<CinemachineVirtualCamera>();
    }

    public void SetActiveCamera(CinemachineVirtualCamera cameraToActivate)
    {
        foreach (var camera in cameras)
        {
            if (camera == cameraToActivate)
            {
                camera.enabled = true;
            }
            else
            {
                camera.enabled = false;
            }
        }
    }
}
