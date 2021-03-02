using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{
    float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 0.5)
        {
            transform.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

}
