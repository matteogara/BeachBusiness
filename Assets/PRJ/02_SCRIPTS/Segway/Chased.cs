using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chased : MonoBehaviour
{
    public float walkRadius;
    SegwayController controller;


    private void Awake()
    {
        controller = GetComponent<SegwayController>();
    }


    void Start()
    {
        StartCoroutine("Timer");
    }


    IEnumerator Timer()
    {
        NewDestination();

        int wait = Random.Range(1, 3);
        yield return new WaitForSeconds(wait);
        StartCoroutine("Timer");
    }


    void NewDestination() {

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        Debug.Log(hit.position);

        controller.SetDestination(hit.position);
    }
}
