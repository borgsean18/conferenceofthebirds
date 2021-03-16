using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main_Bird : MonoBehaviour
{
    private StateMachine<Main_Bird> m_stateMachine;
    Slider stamina_meter;
    [HideInInspector]
    public Text text;
    public float walk_speed;
    public float jump_speed;
    public float charge_jump_speed;
    [HideInInspector]
    public Transform Bird_Bone;
    //public SpriteRenderer sprite;
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
    public float gliding_time_max;
    [HideInInspector]
    public float gliding_time;
    public float fall_x_speed;
    public float health;
    [HideInInspector]
    public Slider health_slider;
    public float max_magic_to_save;
    public float magic_to_save;
    public float magic_cost_to_save;
    public float hold_to_save_time;
    [HideInInspector]
    public Slider magic_to_save_slider;
    [HideInInspector]
    public Vector3 save_point_position;
    [HideInInspector]
    public GameObject respawn_point;
    bool is_grab_thing;
    Transform grabbed_thing;
    public float grab_decrease_gliding_speed;
    public float grab_decrease_jump_speed;
    public float grab_decrease_gliding_time;
    public float grab_state_gravity;

    Vector3[] RayCheckOffset;
    Vector3[] RayOriginalOffsets;
    bool is_grabbed;
    // Start is called before the first frame update
    void Start()
    {
        m_stateMachine = new StateMachine<Main_Bird>(this);// initial
        m_stateMachine.SetCurrentState(Walk.Instance);// set first state
        //sprite = GetComponent<SpriteRenderer>();
        Bird_Bone = GameObject.Find("bone_1").transform;
        rb = GetComponent<Rigidbody2D>();
        face_direction = -1;
        text = GetComponentInChildren<Text>();
        text.text = "hello";
        Slider[] slider_array = GetComponentsInChildren<Slider>();
        health_slider = slider_array[0];
        print(health_slider.maxValue);
        health_slider.maxValue = health;
        health_slider.value = health_slider.maxValue;
        magic_to_save_slider = slider_array[1];
        magic_to_save_slider.maxValue = max_magic_to_save;
        magic_to_save_slider.value = magic_to_save;
        print(magic_to_save_slider.value);
        save_point_position = transform.position;
        respawn_point = GameObject.FindGameObjectWithTag("RespawnPoint");
        respawn_point.transform.position = transform.position;
        gliding_time = gliding_time_max;
        stamina_meter = slider_array[2];
        stamina_meter.maxValue = gliding_time_max;
        print(stamina_meter.maxValue);
        RayCheckOffset = new Vector3[3];
        RayOriginalOffsets = new Vector3[3];

        for (int i=0;i<3;i++)
        {
            RayCheckOffset[i] = transform.position - ground_checks[i].position;
            RayOriginalOffsets[i] = RayCheckOffset[i];
        }
    }
    public void get_hurt(float damage)
    {
        GetFSM().ChangeState(Hurt.Instance);
        health -= damage;
        health_slider.value = health;
        if(health<=0)
        {
            GetFSM().ChangeState(Death.Instance);
        }
    }
    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);//运行到这，暂停t秒
        //t秒后，继续运行下面代码
        print("Time over.");
    }
    void grab_thing()
    {
        if(!is_grab_thing)
        {
            RaycastHit2D temp_result = Physics2D.Linecast(transform.position,
                          ground_checks[1].position,
                          1 << LayerMask.NameToLayer("GrabbableObject"));

            if (temp_result)
            {
                grabbed_thing = temp_result.transform;
                grabbed_thing.GetComponent<Rigidbody2D>().mass = 0.1f;
                grabbed_thing.GetComponent<FixedJoint2D>().enabled = true;
                print("Time over213.");

                grabbed_thing.GetComponent<FixedJoint2D>().connectedBody = rb;
                Collider2D grabbed_thing_collider = grabbed_thing.GetComponent<Collider2D>();
                print(grabbed_thing_collider.bounds.center);
                print(transform.position);
                RayCheckOffset[0] =new Vector3(0,0,0);
                RayCheckOffset[1] = grabbed_thing_collider.bounds.extents+new Vector3(0,0.5f,0);
                RayCheckOffset[2] = grabbed_thing_collider.bounds.extents + new Vector3(0, 0.5f, 0);
                grabbed_thing.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                grabbed_thing.gameObject.layer = 0;
                is_grab_thing = true;
            }
        }
        else
        {
            is_grab_thing = false;

            grabbed_thing.gameObject.layer = 9;
            grabbed_thing.GetComponent<Rigidbody2D>().mass = 50;
            for(int i=0;i<3;i++)
            {
                RayCheckOffset[i] = RayOriginalOffsets[i];
            }
            grabbed_thing.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            grabbed_thing.GetComponent<FixedJoint2D>().enabled=false;
            grabbed_thing = null;
        }
    }

    IEnumerator wait3()
    {
        yield return new WaitForSeconds(1);    //注意等待时间的写法
    }

    public StateMachine<Main_Bird> GetFSM()
    {
        return m_stateMachine;
    }
    // Update is called once per frame
    void check_face_direction()
    {
        if (face_direction == -1)
            Bird_Bone.localScale = new Vector3(1, -1, 1);
        else
            Bird_Bone.localScale = new Vector3(1, 1, 1);
    }
    void ray_check_transforms_follow()
    {
        if (!is_grab_thing)
        {
            for (int i = 0; i < 3; i++)
            {
                ground_checks[i].position = transform.position - RayCheckOffset[i];
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                ground_checks[i].position = grabbed_thing.position - RayCheckOffset[i];
            }
        }

    }
    void Update()
    {
        text.text=GetFSM().CurrentState().ToString();
        m_stateMachine.StateMachineUpdate();
        Check_On_The_Ground();
        check_face_direction();
        stamina_meter.value = gliding_time;
        ray_check_transforms_follow();
        if (Input.GetKeyDown(KeyCode.E))
        {
            grab_thing();
        }
    }
    private void Check_On_The_Ground()
    {
        for (int i = 0; i < 3; i++)
        {
            RaycastHit2D checkResult = Physics2D.Linecast(transform.position,
                          ground_checks[i].position,
                          1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D temp_result = Physics2D.Linecast(transform.position,
                          ground_checks[i].position,
                          1 << LayerMask.NameToLayer("GrabbableObject"));
            is_on_ground = checkResult| temp_result;
            if (is_on_ground)
            {
                //print("onground");
                break;
            }

        }

    }
}
