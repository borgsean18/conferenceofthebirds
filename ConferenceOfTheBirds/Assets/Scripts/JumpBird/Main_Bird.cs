using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main_Bird : MonoBehaviour
{
    private StateMachine<Main_Bird> m_stateMachine;
    private Text text;
    public float walk_speed;
    public float jump_speed;
    public float charge_jump_speed;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public Rigidbody2D rb;
    public Transform[] ground_checks;
    [HideInInspector]
    public bool is_on_ground;
    [HideInInspector]
    public float height;
    [HideInInspector]
    public int face_direction;// right is 1, left is -1
    public float gliding_speed;
    public float max_charge_jump_holding_time;
    public float min_charge_jump_holding_time;
    public float double_jump_y_speed;
    public float double_jump_x_speed;
    public float gliding_gravity;
    public float fall_x_speed;
    // Start is called before the first frame update
    void Start()
    {
        m_stateMachine = new StateMachine<Main_Bird>(this);// initial
        m_stateMachine.SetCurrentState(Walk.Instance);// set first state
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        face_direction = -1;
        text = GetComponentInChildren<Text>();
        text.text = "hello";
    }
    public StateMachine<Main_Bird> GetFSM()
    {
        return m_stateMachine;
    }
    // Update is called once per frame
    void Update()
    {
        text.text=GetFSM().CurrentState().ToString();
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
