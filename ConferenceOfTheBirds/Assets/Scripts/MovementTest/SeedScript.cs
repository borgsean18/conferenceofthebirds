using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "soil")
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
            collision.gameObject.GetComponent<SoilScript>().isSeeded = true;
            Destroy(gameObject, 1.5f);
        }
    }
}
