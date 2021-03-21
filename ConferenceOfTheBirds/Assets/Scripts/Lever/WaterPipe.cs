using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPipe : MonoBehaviour
{
    //Variables
    public bool isPipeActive;
        
    //GameObjects
    public GameObject pipeWaterPoint;
    public GameObject soilObject;

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
        //do water flow animation
        //...

        soilObject.GetComponent<SoilScript>().isWatered = true;
    }
}
