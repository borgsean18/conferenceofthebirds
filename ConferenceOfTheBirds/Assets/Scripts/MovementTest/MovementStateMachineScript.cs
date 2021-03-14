using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachineScript : MonoBehaviour
{
    //Movement
    public enum state
    {
        none,
        walking,
        jumping,
        flying
    }
    public state currentState;

    //Components
    BoxCollider2D col;

    //public vars
    public LayerMask groundLayer;

    //settings
    private bool enteredFlying = false;
    
    //Singleton
    public static MovementStateMachineScript current;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = state.walking;
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        switch(currentState)
        {
            case state.none:
                //do nothing
                break;
            case state.walking:
                Walking();
                break;
            case state.jumping:
                Jumping();
                break;
            case state.flying:
                Flying();
                break;
        }
    }

    private void Walking()
    {
        enteredFlying = false;
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 20f * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 20f * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            currentState = state.jumping;
        }
        if (Input.GetKeyDown(KeyCode.W) && !IsGrounded() && !enteredFlying)
        {
            currentState = state.flying;
        }
    }

    private void Jumping()
    {
        currentState = state.walking;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 350f));
    }

    private void Flying()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * 20f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * 20f * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.W) && enteredFlying)
        {
            currentState = state.walking;
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        enteredFlying = true;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hitBox = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down,0.05f,groundLayer);

        if (hitBox.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}