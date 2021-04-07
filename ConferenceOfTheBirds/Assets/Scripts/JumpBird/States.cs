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
    float timer;
    bool ready_to_jump;
    int jump_has_direction;
    public static Idle Instance { get; private set; }
    static Idle()
    {
        Instance = new Idle();
    }

    public override void Enter(Main_Bird bird)
    {
        bird.Reset_walk_collider_offset();
        ready_to_jump = false;
        jump_has_direction = 0;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Idle");
        bird.rb.velocity = new Vector2(0, 0);
    }

    public override void Execute(Main_Bird bird)
    {
        if(!bird.is_on_ground)
        {
            bird.GetFSM().ChangeState(Fall.Instance);
        }
        else if(!ready_to_jump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ready_to_jump = true;
                timer = 0;
            }
            else if(Input.GetKeyDown(KeyCode.A))
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
        else
        {
            timer += Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.A))
            {
                bird.face_direction = -1;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                bird.face_direction = 1;
            }
            if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.A))
            {
                jump_has_direction = 1;
            }
            else
            {
                jump_has_direction = 0;

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (timer < bird.min_charge_jump_holding_time)
                {
                    bird.rb.velocity += new Vector2(jump_has_direction*bird.walk_speed/2, bird.jump_speed);
                    bird.GetFSM().ChangeState(Jump.Instance);
                }
                else if (timer >= bird.min_charge_jump_holding_time && timer < bird.max_charge_jump_holding_time)
                {
                    float temp = timer / bird.max_charge_jump_holding_time;
                    bird.rb.velocity += new Vector2(jump_has_direction*bird.face_direction * bird.walk_speed * temp, bird.charge_jump_speed * temp);
                    bird.GetFSM().ChangeState(Jump.Instance);
                }
                else if (timer >= bird.max_charge_jump_holding_time)
                {
                    bird.rb.velocity += new Vector2(jump_has_direction*bird.face_direction * bird.walk_speed, bird.charge_jump_speed);
                    bird.GetFSM().ChangeState(Jump.Instance);

                }
            }
            else if(timer >= bird.max_charge_jump_holding_time)
            {
                bird.rb.velocity += new Vector2(jump_has_direction * bird.face_direction * bird.walk_speed, bird.charge_jump_speed);
                bird.GetFSM().ChangeState(Jump.Instance);
            }
        }
        

    }

    public override void Exit(Main_Bird bird)
    {
        ready_to_jump = false;
        jump_has_direction = 0;
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
        bird.Set_walk_collider_offset();
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
            bird.rb.velocity += new Vector2(bird.face_direction * bird.walk_speed/2, bird.jump_speed);
            bird.GetFSM().ChangeState(Jump.Instance);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            bird.face_direction = 1;
            bird.rb.velocity = new Vector2(bird.walk_speed, bird.rb.velocity.y);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bird.rb.velocity += new Vector2(bird.walk_speed / 2, bird.jump_speed);
            }
        }
        else if(Input.GetKey(KeyCode.A))
        {
            bird.face_direction = -1;
            bird.rb.velocity = new Vector2(-bird.walk_speed, bird.rb.velocity.y);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bird.rb.velocity += new Vector2(-bird.walk_speed / 2, bird.jump_speed);
            }
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
    static Jump()
    {
        Instance = new Jump();
    }

    public override void Enter(Main_Bird bird)
    {
        height = 0;
        double_fly = false;
        bird.ResetAllTriggers(bird.animator);
        bird.animator.SetTrigger("Ground_Fly");
        Debug.Log("ground_fly");
    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.rb.velocity.y>0)
        {
            height += bird.rb.velocity.y;
            if (Input.GetKeyDown(KeyCode.Space)&&!double_fly)
            {
                double_fly = true;
                bird.height = height;
                bird.rb.velocity = new Vector2(bird.face_direction * (Mathf.Abs(bird.rb.velocity.x) + bird.double_jump_x_speed), bird.rb.velocity.y + bird.double_jump_y_speed);
            }
        }
        else
        {
            bird.GetFSM().ChangeState(Fall.Instance);
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
    float timer;
    float height;

    static Air_Dash()
    {
        Instance = new Air_Dash();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
    }

    public override void Execute(Main_Bird bird)
    {
        
    }

    public override void Exit(Main_Bird bird)
    {
    }
}
public class Gliding : State<Main_Bird>
{
    public static Gliding Instance { get; private set; }
    float timer;
    float current_height;
    static Gliding()
    {
        Instance = new Gliding();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
        current_height = bird.height;
        bird.rb.velocity = new Vector2(bird.rb.velocity.x, bird.rb.velocity.y);
        bird.rb.gravityScale = bird.gliding_gravity;

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
                if (Input.GetKey(KeyCode.A))
                {
                    bird.face_direction = -1;
                    bird.rb.velocity = new Vector2(-bird.gliding_speed, bird.rb.velocity.y);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    bird.face_direction = 1;
                    bird.rb.velocity = new Vector2(bird.gliding_speed, bird.rb.velocity.y);
                }
                if(Input.GetKey(KeyCode.W))
                {
                    bird.rb.velocity = new Vector2(bird.face_direction*bird.gliding_speed, bird.fly_up_speed);
                    bird.gliding_time -= Time.deltaTime* bird.fly_up_extra_stamina_cost;
                    bird.ResetAllTriggers(bird.animator);
                    bird.animator.SetTrigger("Fly");
                }
                if(Input.GetKeyUp(KeyCode.W))
                {
                    //bird.rb.velocity -= new Vector2(0, bird.flying_up_speed);
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

                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    bird.GetFSM().ChangeState(Fall.Instance);
                }
            }
            
        }
        current_height += bird.rb.velocity.y;
    }

    public override void Exit(Main_Bird bird)
    {
        bird.rb.gravityScale = 1f;
        timer = 0;
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
    }

    public override void Execute(Main_Bird bird)
    {
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
                if (Input.GetKey(KeyCode.A))
                {
                    bird.face_direction = -1;
                    bird.rb.velocity = new Vector2(-bird.fall_x_speed, bird.rb.velocity.y);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    bird.face_direction = 1;
                    bird.rb.velocity = new Vector2(bird.fall_x_speed, bird.rb.velocity.y);
                }
                if (Input.GetKey(KeyCode.Space) && bird.gliding_time > 0)
                {
                    bird.rb.velocity = new Vector2(bird.face_direction * bird.gliding_speed, bird.rb.velocity.y);
                    bird.GetFSM().ChangeState(Gliding.Instance);
                }
                else if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //bird.rb.velocity = new Vector2(bird.face_direction * 8, 3);
                }
            }
            
        }
        
    }

    public override void Exit(Main_Bird bird)
    {
        bird.ResetAllTriggers(bird.animator);
    }
}

public class Hurt : State<Main_Bird>
{
    public static Hurt Instance { get; private set; }
    static Hurt()
    {
        Instance = new Hurt();
    }

    public override void Enter(Main_Bird bird)
    {
        bird.rb.gravityScale = 1f;
    }

    public override void Execute(Main_Bird bird)
    {
       bird.GetFSM().ChangeState(Fall.Instance);
    }

    public override void Exit(Main_Bird bird)
    {

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
    }

    public override void Execute(Main_Bird bird)
    {
        timer += Time.deltaTime;
        
        if (timer>0.2f)
        {
            bird.transform.position = bird.save_point_position;
            bird.health = 100;
            bird.health_slider.value = bird.health;
            bird.GetFSM().ChangeState(Idle.Instance);
        }
    }

    public override void Exit(Main_Bird bird)
    {
    }
}
