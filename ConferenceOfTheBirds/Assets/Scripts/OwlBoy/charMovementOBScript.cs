using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charMovementOBScript : MonoBehaviour
{
    //singleton
    public static charMovementOBScript current;

    //stats
    public float normalSpeed = 2f;
    private float speed = 0;
    public int jumps = 1;
    public bool flyMode = false;

    [Header("Scene Objects")]
    //Camera
    public GameObject mainCameraGameObj;
    private Camera mainCameraComponent;

    //Triggers
    private bool enteredFlymode = false;

    //Components
    private Rigidbody2D rb;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCameraComponent = mainCameraGameObj.GetComponent<Camera>();
        mainCameraComponent.backgroundColor = Color.blue;

        //stats
        speed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        Fly();

        enteredFlymode = false;
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && floorCheckScript.current.isOnFloor)
        {
            jumps--;
            rb.AddForce(transform.up * 250f);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !floorCheckScript.current.isOnFloor && flyMode == false)
        {
            flyMode = true;
            enteredFlymode = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    private void Fly()
    {
        if (flyMode)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space) && enteredFlymode == false)
            {
                flyMode = false;
                rb.gravityScale = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColdPatch")
        {
            speed = 1;
            mainCameraComponent.backgroundColor = Color.Lerp(Color.blue, Color.cyan, 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColdPatch")
        {
            speed = normalSpeed;
            mainCameraComponent.backgroundColor = Color.Lerp(Color.cyan, Color.blue, 3f);
        }
    }
}
