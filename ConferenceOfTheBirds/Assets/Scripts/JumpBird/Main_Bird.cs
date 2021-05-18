using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Main_Bird : MonoBehaviour
{
    private StateMachine<Main_Bird> m_stateMachine;
    [HideInInspector]
    public Image stamina_meter;
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
    public float fly_up_speed;
    public float fly_down_speed;
    public float fly_up_extra_stamina_cost;
    public float fly_down_extra_stamina_save;
    public float fall_stamina_recovery;
    [HideInInspector]
    public float gliding_time;
    public float fall_x_speed;

    public float max_health;

    [HideInInspector]
    public float health;
    [HideInInspector]
    public Image health_slider;

    [HideInInspector]
    public Slider magic_to_save_slider;
    [HideInInspector]
    public Vector3 save_point_position;
    [HideInInspector]
    public GameObject respawn_point;
    bool is_grab_thing;
    Transform grabbed_thing;
    [HideInInspector]
    public float grab_decrease_gliding_speed;
    [HideInInspector]
    public float grab_decrease_jump_speed;
    [HideInInspector]
    public float grab_decrease_gliding_time;
    [HideInInspector]
    public float grab_state_gravity;
    Collider2D grabbed_thing_collider;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Cinemachine.CinemachineCollisionImpulseSource MyInpulse;

    Collider2D collider;
    //[HideInInspector]
    [HideInInspector]
    public bool is_hit_wall;
    [HideInInspector]
    public int hit_point_is_right;
    // Start is called before the first frame update
    public float Dash_Speed;

    public float Dash_Distance;
    public float Dash_Stamina_Cost;
    AudioSource FlappingSound;
    AudioSource DyingSound;
    public bool CanMove;
    [HideInInspector]
    public TrailRenderer[] trails;
    void Start()
    {
        m_stateMachine = new StateMachine<Main_Bird>(this);// initial
        animator = GetComponent<Animator>();
        m_stateMachine.SetCurrentState(Idle.Instance);// set first state
        Bird_Bone = GameObject.Find("bone_1").transform;
        rb = GetComponent<Rigidbody2D>();
        face_direction = 1;
        text = GetComponentInChildren<Text>();
        text.text = "hello";
        GameObject BirdHealth_O = GameObject.FindGameObjectWithTag("BirdHealthSlider");
        //print(BirdHealth_O.name);
        health_slider = BirdHealth_O.GetComponent<Image>();
        //print(health_slider.maxValue);
        health = max_health;
        health_slider.fillAmount = 1;
        //magic_to_save_slider = BirdMagic_O.GetComponent<Slider>();
        save_point_position = transform.position;
        //respawn_point = GameObject.FindGameObjectWithTag("RespawnPoint");
        //respawn_point.transform.position = transform.position;
        gliding_time = gliding_time_max;
        GameObject BirdStamina_O = GameObject.FindGameObjectWithTag("BirdStaminaSlider");
        stamina_meter= BirdStamina_O.GetComponent<Image>();
        stamina_meter.fillAmount = 1;
        collider = GetComponent<Collider2D>();
        fix_ground_checks_positions(collider);
        CanMove = true;
        MyInpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
        trails = GetComponentsInChildren<TrailRenderer>();
        for(int i=0;i<trails.Length;i++)
        {
            trails[i].enabled = false;
        }
        FlappingSound = GameObject.Find("FlappingSound").GetComponent<AudioSource>();
        DyingSound = GameObject.Find("DyingSound").GetComponent<AudioSource>();
    }
    public void get_hurt(float damage)
    {
        if(!m_stateMachine.IsInState(Hurt.Instance))
        {
            GetFSM().ChangeState(Hurt.Instance);
            health -= damage;
            float temp = health / max_health;
            health_slider.fillAmount = temp;
            if (health <= 0)
            {
                GetFSM().ChangeState(Death.Instance);
            }
        }
            
    }
    public void ControlFlappingSound(bool a)
    {
        if(a)
        {
            FlappingSound.Play();
        }
        else
        {
            FlappingSound.Stop();
        }
    }
    public void ControlDyingSound(bool a)
    {
        if(a)
        {
            DyingSound.Play();
        }
        else
        {
            DyingSound.Stop();
        }
    }
    public void test_co()
    {
        StartCoroutine(Wait(0.1f));
    }
    public void set_trail(bool on)
    {
        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].enabled = on;
        }
    }
    public void start_acc(Vector2 v)
    {
        //StopCoroutine("VeclocityTrans");
        //StartCoroutine("VeclocityTrans", v);
        StartCoroutine(VeclocityTrans(v));
    }
    public IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        print("Time over.");
    }
    public IEnumerator VeclocityTrans(Vector2 new_v)
    {
        Vector2 ori_v = rb.velocity;
        for(float timer = 0;timer<0.05f;timer+=Time.deltaTime)
        {
            rb.velocity = Vector2.Lerp(ori_v, new_v, timer);
            yield return 0;
        }
    }
    void grab_thing()
    {
        if (!is_grab_thing)
        {
            RaycastHit2D temp_result = Physics2D.Linecast(transform.position,
                          ground_checks[1].position,
                          1 << LayerMask.NameToLayer("GrabbableObject"));

            if (temp_result)
            {
                grabbed_thing = temp_result.transform;
                grabbed_thing.GetComponent<Rigidbody2D>().mass = 0.1f;
                grabbed_thing.GetComponent<FixedJoint2D>().enabled = true;
                grabbed_thing.GetComponent<FixedJoint2D>().connectedBody = rb;
                grabbed_thing_collider = grabbed_thing.GetComponent<Collider2D>();
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
            grabbed_thing.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            grabbed_thing.GetComponent<FixedJoint2D>().enabled = false;
            grabbed_thing = null;
            grabbed_thing_collider = null;
        }
    }

    void fix_ground_checks_positions(Collider2D this_collider)
    {
        ground_checks[0].position = this_collider.bounds.center;
        ground_checks[1].position = this_collider.bounds.center - this_collider.bounds.extents + new Vector3(0.2f, -0.2f, 0);
        ground_checks[2].position = this_collider.bounds.center - this_collider.bounds.extents + new Vector3(this_collider.bounds.size.x, 0, 0) + new Vector3(-0.2f, -0.2f, 0);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            is_hit_wall = true;
            if(collision.GetContact(0).point.x>collider.bounds.center.x)
            {
                hit_point_is_right = 1;
            }
            else
            {
                hit_point_is_right = -1;
            }
            print("hit");
        }
        if(collision.gameObject.tag=="Spike")
        {
            Vector2 temp;
            if (collision.GetContact(0).point.x - collider.bounds.center.x>0)
            {
                temp = new Vector2(-8, -6*(collision.GetContact(0).point.y - collider.bounds.center.y));
            }
            else
            {
                temp = new Vector2(8, -6 * (collision.GetContact(0).point.y - collider.bounds.center.y));
            }
            rb.AddForce(temp, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            is_hit_wall = false;
            print("not hit");
        }
    }
    public StateMachine<Main_Bird> GetFSM()
    {
        return m_stateMachine;
    }
    // Update is called once per frame
    void check_face_direction()
    {
        if (face_direction == -1)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }
    void ray_check_transforms_follow()
    {
        if (!is_grab_thing)
        {
            fix_ground_checks_positions(collider);
        }
        else
        {
            fix_ground_checks_positions(grabbed_thing_collider);
        }

    }

    void hit_wall_check()
    {
        if (GetFSM().CurrentState() == Walk.Instance|| (GetFSM().CurrentState() == Idle.Instance))
        {
            is_hit_wall = false;
        }
    }

    void Update()
    {
        //print(GetFSM().CurrentState());
        text.text = GetFSM().CurrentState().ToString();
        if(CanMove)
        {
            m_stateMachine.StateMachineUpdate();
        }
        Check_On_The_Ground();
        check_face_direction();
        hit_wall_check();
        stamina_meter.fillAmount = gliding_time / gliding_time_max;
        health_slider.fillAmount = health/max_health;
        ray_check_transforms_follow();
        if (Input.GetKeyDown(KeyCode.E))
        {
            grab_thing();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            m_stateMachine.ChangeState(Death.Instance);
        }
        
    }
    private void Check_On_The_Ground()
    {

        RaycastHit2D checkResult = Physics2D.Linecast(ground_checks[0].position,
                          ground_checks[1].position,
                          1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D checkResult2 = Physics2D.Linecast(ground_checks[0].position,
                          ground_checks[2].position,
                          1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D temp_result = Physics2D.Linecast(ground_checks[0].position,
                      ground_checks[1].position,
                      1 << LayerMask.NameToLayer("GrabbableObject"));
        RaycastHit2D temp_result2 = Physics2D.Linecast(ground_checks[0].position,
                      ground_checks[2].position,
                      1 << LayerMask.NameToLayer("GrabbableObject"));
        is_on_ground = checkResult | temp_result | checkResult2 | temp_result2;
    }
    public void ResetAllTriggers(Animator animator)
    {
        AnimatorControllerParameter[] aps = animator.parameters;
        for (int i = 0; i < aps.Length; i++)
        {
            AnimatorControllerParameter paramItem = aps[i];
            if (paramItem.type == AnimatorControllerParameterType.Trigger)
            {
                string triggerName = paramItem.name;
                bool isActive = animator.GetBool(triggerName);
                if (isActive)
                {
                    animator.ResetTrigger(triggerName);
                }
            }
        }
    }
}
