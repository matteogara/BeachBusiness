using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class SegwayRoll : MonoBehaviour
{

    public float rollMultiplier = 20;
    public float smoothness = 0.02f;
    public Transform agentTrans;

    Vector3 oldPos;
    float vel;


    void Update()
    {
        vel = Mathf.Lerp(vel, GetVelocity(agentTrans), smoothness);
        transform.localRotation = Quaternion.Euler(vel * rollMultiplier, 0, 0);
        Debug.Log(vel);
    }


    float GetVelocity(Transform _t)
    {
        Vector3 _pos = _t.position;
        Vector3 _shift = Vector3.Project((_pos - oldPos), _t.forward);
        float _vel = _shift.magnitude / Time.deltaTime;

        oldPos = _pos;

        return _vel;
    }
}