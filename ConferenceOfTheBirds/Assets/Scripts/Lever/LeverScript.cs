using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    //Variables
    private bool canToggleLever = false;
    public bool isActive = false;

    //GameObjects
    public GameObject waterPipe;

    // Update is called once per frame
    void Update()
    {
        if (canToggleLever)
            ToggleLever();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canToggleLever = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canToggleLever = false;
    }

    private void ToggleLever()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            if (isActive)
            {
                waterPipe.GetComponent<WaterPipe>().isPipeActive = true;
            }
        }
    }
}
