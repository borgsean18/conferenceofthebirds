using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorCheckScript : MonoBehaviour
{
    public static floorCheckScript current;

    private GameObject parent;
    private Rigidbody2D parentRB;

    public bool isOnFloor = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
        parentRB = parent.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isOnFloor)
        {
            if (!charMovementOBScript.current.flyMode)
            {
                charMovementOBScript.current.jumps = 1;
                charMovementOBScript.current.flyMode = false;
                parentRB.gravityScale = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != parent)
        {
            isOnFloor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != parent)
        {
            isOnFloor = false;
        }
    }
}
