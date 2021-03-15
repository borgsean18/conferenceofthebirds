using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPointScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //end the level
            GameManagerScript.current.EndLevel();
        }
    }
}
