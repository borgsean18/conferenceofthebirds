using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State<T>
{
    /// <summary>
    /// 当状态被进入时执行这个函数
    /// </summary>
    public abstract void Enter(T entity);

    /// <summary>
    /// 旷工更新状态函数
    /// </summary>
    public abstract void Execute(T entity);

    /// <summary>
    /// 当状态退出时执行这个函数
    /// </summary>
    public abstract void Exit(T entity);
}

public class Idle : State<Main_Bird>
{
    
    public static Idle Instance { get; private set; }
    static Idle()
    {
        Instance = new Idle();
    }

    public override void Enter(Main_Bird bird)
    {
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Idle");
        bird.rb.velocity = new Vector2(0, 0);
        bird.gliding_time = bird.gliding_time_max;
    }

    public override void Execute(Main_Bird bird)
    {
        if(!bird.is_on_ground)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
                bird.GetFSM().ChangeState(Jump.Instance);
            else if (Input.GetKeyDown(KeyCode.A))
            {
                bird.face_direction = -1;
                bird.GetFSM().ChangeState(Walk.Instance);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                bird.face_direction = 1;
                bird.GetFSM().ChangeState(Walk.Instance);
            }
        }
        

    }

    public override void Exit(Main_Bird bird)
    {
    }
}
public class Walk : State<Main_Bird>
{
    public static Walk Instance { get; private set; }
    float timer;
    bool ready_to_jump;
    bool is_down;
    static Walk()
    {
        Instance = new Walk();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
        ready_to_jump = false;
        is_down = false;
        bird.gliding_time = bird.gliding_time_max;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Hop");
    }

    public override void Execute(Main_Bird bird)
    {
        
        if(!bird.is_on_ground)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //bird.rb.velocity = new Vector2(bird.face_direction * bird.walk_speed/2, bird.jump_speed);
            bird.GetFSM().ChangeState(Jump.Instance);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            bird.face_direction = 1;
            bird.rb.velocity = new Vector2(bird.walk_speed, bird.rb.velocity.y);
            
        }
        else if(Input.GetKey(KeyCode.A))
        {
            bird.face_direction = -1;
            bird.rb.velocity = new Vector2(-bird.walk_speed, bird.rb.velocity.y);
        }
        
        else
        {
            bird.GetFSM().ChangeState(Idle.Instance);
        }
    }

    public override void Exit(Main_Bird bird)
    {
        ready_to_jump = false;

        timer = 0;
    }
}

public class Jump : State<Main_Bird>
{
    public static Jump Instance { get; private set; }
    float height;
    bool double_fly;
    int jump_has_direction;
    float timer;
    bool is_press_space;
    static Jump()
    {
        Instance = new Jump();
    }

    public override void Enter(Main_Bird bird)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bird.face_direction = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            bird.face_direction = 1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            jump_has_direction = 1;
        }
        else
        {
            jump_has_direction = 0;
        }
        bird.rb.velocity += new Vector2(jump_has_direction * bird.walk_speed / 2, bird.jump_speed);
        height = 0;
        double_fly = false;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Ground_Fly");
        timer = 0;
        is_press_space = true;
        //bird.ControlFlappingSound(true);
    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.rb.velocity.y>0)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                is_press_space = false;
            }
            else
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    timer += Time.deltaTime;
                    if(timer<bird.max_charge_jump_holding_time)
                    {
                        bird.rb.velocity = new Vector2(bird.rb.velocity.x, bird.jump_speed);
                    }
                }
                
            }
        }
        else
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        if (Input.GetKey(KeyCode.D))
        {
            bird.face_direction = 1;
            bird.rb.velocity = new Vector2(bird.walk_speed/2, bird.rb.velocity.y);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            bird.face_direction = -1;
            bird.rb.velocity = new Vector2(-bird.walk_speed/2, bird.rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            bird.GetFSM().ChangeState(Air_Dash.Instance);

        }
    }

    public override void Exit(Main_Bird bird)
    {
        height = 0;

    }
}

public class Air_Dash : State<Main_Bird>
{
    public static Air_Dash Instance { get; private set; }
    float distance_flied;
    int horizontal_direction;
    float temp_x_dash_speed;
    Vector2 dash_V;
    static Air_Dash()
    {
        Instance = new Air_Dash();
    }

    public override void Enter(Main_Bird bird)
    {
        
        if(Input.GetKey(KeyCode.A))
        {
            bird.face_direction = -1;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            bird.face_direction = 1;
        }
        if(Input.GetKey(KeyCode.W))
        {
            horizontal_direction = 1;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            horizontal_direction = -1;
        }
        else
        {
            horizontal_direction = 0;
        }
        if(horizontal_direction!=0)
        {
            temp_x_dash_speed = bird.Dash_Speed;
        }
        else
        {
            temp_x_dash_speed = bird.Dash_Speed * 1.4f;
        }
        bird.rb.velocity = new Vector2(temp_x_dash_speed * bird.face_direction, horizontal_direction*bird.Dash_Speed);
        distance_flied = 0;
        if (bird.gliding_time > bird.Dash_Stamina_Cost)
            bird.gliding_time -= bird.Dash_Stamina_Cost;
        else
            bird.gliding_time = 0;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Glide");
        dash_V = new Vector2(0, 0);
        bird.MyInpulse.GenerateImpulse();
    }

    public override void Execute(Main_Bird bird)
    {
        if (bird.is_hit_wall)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        dash_V += bird.rb.velocity * Time.deltaTime;
        distance_flied = dash_V.magnitude;
        if (distance_flied > bird.Dash_Distance)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }

    }

    public override void Exit(Main_Bird bird)
    {
        distance_flied = 0;

    }
}
public class Gliding : State<Main_Bird>
{
    public static Gliding Instance { get; private set; }
    static Gliding()
    {
        Instance = new Gliding();
    }

    public override void Enter(Main_Bird bird)
    {
        //bird.rb.velocity = new Vector2(bird.rb.velocity.x, bird.rb.velocity.y);
        bird.rb.gravityScale = bird.gliding_gravity;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Fly");
        Debug.Log("here");
        

    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.is_on_ground)
        {
            bird.height = 0;
            bird.GetFSM().ChangeState(Walk.Instance);
        }
        else if(bird.is_hit_wall)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        else
        {
            bird.gliding_time-= Time.deltaTime;
            if(bird.gliding_time<0)
            {
                bird.GetFSM().ChangeState(Fall.Instance);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    bird.face_direction = -1;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    bird.face_direction = 1;
                    
                }
                if(Mathf.Abs(bird.rb.velocity.x)<= bird.gliding_speed)
                {
                    bird.rb.velocity = new Vector2(bird.face_direction * bird.gliding_speed, bird.rb.velocity.y);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    if (Mathf.Abs(bird.rb.velocity.x) <= bird.gliding_speed)
                        bird.rb.velocity = new Vector2(bird.face_direction*bird.gliding_speed, bird.fly_up_speed);
                    if(!(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D)))
                    {
                        bird.rb.velocity = new Vector2(0, bird.fly_up_speed*1.5f);
                    }
                    bird.gliding_time -= Time.deltaTime* bird.fly_up_extra_stamina_cost;
                    bird.ResetAllTriggers(bird.animator);
                    bird.animator.SetTrigger("Fly");
                }
                if(Input.GetKeyUp(KeyCode.W))
                {
                    bird.rb.velocity = new Vector2(bird.face_direction * bird.gliding_speed, 0);
                    Debug.Log("here2");

                }
                if (Input.GetKey(KeyCode.S))
                {
                    //bird.rb.velocity = new Vector2(bird.rb.velocity.x, -3);
                    //bird.rb.gravityScale = bird.gliding_gravity*2;
                    bird.rb.velocity = new Vector2(bird.face_direction * bird.gliding_speed, -bird.fly_down_speed);
                    bird.gliding_time += Time.deltaTime * bird.fly_down_extra_stamina_save;

                    bird.ResetAllTriggers(bird.animator);
                    bird.animator.SetTrigger("Glide");
                }
                else if (Input.GetKeyUp(KeyCode.S))
                {
                    //bird.rb.velocity = new Vector2(bird.rb.velocity.x, -1);
                    bird.rb.gravityScale = bird.gliding_gravity;
                    bird.set_trail(false);


                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    bird.GetFSM().ChangeState(Fall.Instance);
                }
                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    bird.GetFSM().ChangeState(Air_Dash.Instance);

                }
            }
            
        }
    }

    public override void Exit(Main_Bird bird)
    {
        bird.rb.gravityScale = 1f;
        

    }
}

public class Fall : State<Main_Bird>
{
    public static Fall Instance { get; private set; }
    static Fall()
    {
        Instance = new Fall();
    }

    public override void Enter(Main_Bird bird)
    {
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Fall");
        bird.rb.gravityScale = 2;
        

    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.gliding_time<bird.gliding_time_max)
        {
            bird.gliding_time += bird.fall_stamina_recovery * Time.deltaTime;
        }
        if(bird.is_on_ground)
        {
            Debug.Log("hitground");
            if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
            {
                bird.GetFSM().ChangeState(Walk.Instance);
            }
            else
            {
                bird.GetFSM().ChangeState(Idle.Instance);
            }
        }
        else
        {
            if(!bird.is_hit_wall)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    bird.face_direction = -1;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    bird.face_direction = 1;
                    
                }
                if (Mathf.Abs(bird.rb.velocity.x) <= bird.fall_x_speed)
                    bird.rb.velocity = new Vector2(bird.face_direction*bird.fall_x_speed, bird.rb.velocity.y);
                if (Input.GetKey(KeyCode.Space) && bird.gliding_time > 0.5f)
                {
                    bird.GetFSM().ChangeState(Gliding.Instance);
                }
                if(bird.gliding_time>0&&Input.GetKeyDown(KeyCode.LeftShift))
                {
                    bird.GetFSM().ChangeState(Air_Dash.Instance);

                }
            }
            else
            {
                if(bird.hit_point_is_right==1)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        bird.face_direction = -1;
                        bird.rb.velocity = new Vector2(-bird.fall_x_speed, bird.rb.velocity.y);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        bird.face_direction = 1;
                        bird.rb.velocity = new Vector2(bird.fall_x_speed, bird.rb.velocity.y);
                    }
                }
            }
        }
        
    }

    public override void Exit(Main_Bird bird)
    {
        bird.ResetAllTriggers(bird.animator);
        bird.rb.gravityScale = 1;
        

    }
}

public class Hurt : State<Main_Bird>
{
    public static Hurt Instance { get; private set; }
    static Hurt()
    {
        Instance = new Hurt();
    }
    float time_cool_down;
    float timer;
    public override void Enter(Main_Bird bird)
    {
        bird.rb.gravityScale = 1f;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Idle");
        time_cool_down = 1f;
        timer = 0;
        bird.MyInpulse.GenerateImpulse();
    }

    public override void Execute(Main_Bird bird)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            bird.GetFSM().ChangeState(Gliding.Instance);
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            bird.GetFSM().ChangeState(Air_Dash.Instance);
        }
        if (timer<time_cool_down)
        {
            timer += Time.deltaTime;
        }
        else
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
    }

    public override void Exit(Main_Bird bird)
    {
        timer = 0;
    }
}

public class Save_Game : State<Main_Bird>
{
    public static Save_Game Instance { get; private set; }
    float timer;
    float height;

    static Save_Game()
    {
        Instance = new Save_Game();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
    }

    public override void Execute(Main_Bird bird)
    {
        timer += Time.deltaTime;
        if(Input.GetKeyUp(KeyCode.F))
        {
            bird.GetFSM().ChangeState(Walk.Instance);
        }
        /*else if(timer>bird.hold_to_save_time)
        {
            bird.save_point_position = bird.transform.position;
            bird.respawn_point.transform.position = bird.transform.position;
            bird.GetFSM().ChangeState(Walk.Instance);

            bird.magic_to_save -= bird.magic_cost_to_save;
            bird.magic_to_save_slider.value -= bird.magic_cost_to_save;

        }*/
    }

    public override void Exit(Main_Bird bird)
    {
        timer = 0;
    }
}

public class Death : State<Main_Bird>
{
    public static Death Instance { get; private set; }
    float timer;
    float height;

    static Death()
    {
        Instance = new Death();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
        bird.text.text = "DEAD";
        bird.ControlDyingSound(true);
    }

    public override void Execute(Main_Bird bird)
    {
        timer += Time.deltaTime;
        
        if (timer>0.2f)
        {
            bird.transform.position = bird.save_point_position;
            bird.health = bird.max_health;
            bird.health_slider.fillAmount = bird.health/bird.max_health;
            bird.GetFSM().ChangeState(Idle.Instance);
        }
    }

    public override void Exit(Main_Bird bird)
    {

    }
}

