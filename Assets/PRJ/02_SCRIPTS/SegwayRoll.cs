using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class SegwayRoll : MonoBehaviour
{
    public ClickToSteer clickToSteer;
    public NavMeshAgent agent;
    public float rollAmplitude;
    public float smoothing;

    float prevVel, roll;


    void Update()
    {
        //// Calcola accelerazione
        //float vel = clickToSteer.speed;
        //float accel = (vel - prevVel) / Time.deltaTime;
        //if (Mathf.Approximately(vel, prevVel))
        //{
        //    accel = 0;
        //}

        //// Applica rotazione
        //float targetRoll = accel * rollAmplitude;
        //roll = Mathf.Lerp(roll, targetRoll, smoothing);
        //transform.localRotation = Quaternion.Euler(roll, 0, 0);

        //prevVel = vel;

        float targetRoll;

        if (clickToSteer.targetSpeed > 0.99f) {
            targetRoll = (clickToSteer.targetSpeed - clickToSteer.speed) * rollAmplitude;
        } else {
            targetRoll = (0 - clickToSteer.targetSpeed) * rollAmplitude;
        }

        if (agent.isStopped) {
            targetRoll = 0;
        }

        roll = Mathf.Lerp(roll, targetRoll, smoothing);
        transform.localRotation = Quaternion.Euler(roll, 0, 0);
    }
}