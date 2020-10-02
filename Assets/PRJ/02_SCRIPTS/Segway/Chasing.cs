using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    public Transform chased;
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

        yield return new WaitForSeconds(0.2f);
        StartCoroutine("Timer");
    }


    void NewDestination() {
        controller.SetDestination(chased.position);
    }
}
