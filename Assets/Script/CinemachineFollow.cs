using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineFollow : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    public  CinemachineVirtualCamera vcam;


    void Update()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.Find("PlayerCameraRoot");
        }
        tFollowTarget = tPlayer.transform;
        vcam.Follow = tFollowTarget;
    }
}
