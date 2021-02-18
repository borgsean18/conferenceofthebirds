using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdPatchScript : MonoBehaviour
{
    private float normalWalkSpeed;
    private float slowWalkSpeed;

    private float normalJumpSpeed;
    private float slowJumpSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            normalWalkSpeed = collision.gameObject.GetComponent<Main_Bird>().walk_speed;
            normalJumpSpeed = collision.gameObject.GetComponent<Main_Bird>().jump_speed;
            slowWalkSpeed = normalWalkSpeed / 3;
            slowJumpSpeed = normalJumpSpeed / 3;
            collision.gameObject.GetComponent<Main_Bird>().walk_speed = slowWalkSpeed;
            collision.gameObject.GetComponent<Main_Bird>().jump_speed = slowJumpSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Main_Bird>().walk_speed = normalWalkSpeed;
            collision.gameObject.GetComponent<Main_Bird>().jump_speed = normalJumpSpeed;
        }
    }
}
