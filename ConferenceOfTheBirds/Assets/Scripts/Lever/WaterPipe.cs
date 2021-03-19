using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipe : MonoBehaviour
{
    //Variables
    [HideInInspector]
    public bool isPipeActive;
        
    //GameObjects
    public GameObject pipeWaterPoint;

    // Start is called before the first frame update
    void Start()
    {
        pipeWaterPoint = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPipeActive)
        {
            FlowWater();
        }
    }

    private void FlowWater()
    {

    }
}
