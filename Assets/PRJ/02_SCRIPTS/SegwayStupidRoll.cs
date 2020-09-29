using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SegwayStupidRoll : MonoBehaviour
{
    public NavMeshAgent agent;
    public float maxRoll;
    public float smoothing;

    float rot, vel;


    // Update is called once per frame
    void Update()
    {
        float targetRot = agent.isStopped ? 0 : maxRoll;
        rot = Mathf.SmoothDamp(rot, targetRot, ref vel, smoothing);

        transform.localRotation = Quaternion.Euler(rot, 0, 0);
    }
}
