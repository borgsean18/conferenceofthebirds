using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Bird : MonoBehaviour
{
    private StateMachine<Main_Bird> m_stateMachine;
    public float walk_speed;
    public float jump_speed;
    public float charge_jump_speed;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Transform[] ground_checks;
    public bool is_on_ground;
    public float height;
    public int face_direction;// right is 1, left is -1
    public float gliding_speed;
    // Start is called before the first frame update
    void Start()
    {
        m_stateMachine = new StateMachine<Main_Bird>(this);// initial
        m_stateMachine.SetCurrentState(Walk.Instance);// set first state
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        face_direction = -1;
    }
    public StateMachine<Main_Bird> GetFSM()
    {
        return m_stateMachine;
    }
    // Update is called once per frame
    void Update()
    {
        //print(GetFSM().CurrentState());
        m_stateMachine.StateMachineUpdate();
        Check_On_The_Ground();
        if (face_direction==-1)
            sprite.flipX = false;
        else
            sprite.flipX = true;
    }
    private void Check_On_The_Ground()
    {
        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D checkResult = Physics2D.Linecast(transform.position,
                          ground_checks[i].position,
                          1 << LayerMask.NameToLayer("Ground"));
            is_on_ground = checkResult;
            if (is_on_ground)
            {
                //print("onground");
                break;
            }

        }

    }
}
