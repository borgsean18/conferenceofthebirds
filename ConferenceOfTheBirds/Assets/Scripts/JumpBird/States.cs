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
public class Walk : State<Main_Bird>
{
    public static Walk Instance { get; private set; }
    float timer;
    bool ready_to_jump;
    static Walk()
    {
        Instance = new Walk();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
        ready_to_jump = false;
    }

    public override void Execute(Main_Bird bird)
    {
        if(!ready_to_jump)
        {
            if (Input.GetKey(KeyCode.D))
            {
                bird.sprite.flipX = true;
                bird.face_left = false;
                bird.rb.velocity = new Vector2(bird.walk_speed, 0);
                timer += Time.deltaTime;
                if (timer > 0.2)
                {
                    timer = 0;
                    bird.transform.position += new Vector3(0, 0.05f,0);
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                bird.rb.velocity = new Vector2(0, 0);
                timer = 0;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                bird.sprite.flipX = false;
                bird.face_left = true;

                bird.rb.velocity = new Vector2(-bird.walk_speed, 0);
                timer += Time.deltaTime;
                if (timer > 0.2)
                {
                    timer = 0;
                    bird.transform.position += new Vector3(0, 0.05f, 0);

                }
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                bird.rb.velocity = new Vector2(0, 0);
                timer = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ready_to_jump = true;
            timer = 0;
        }
        else if(Input.GetKey(KeyCode.Space)&& ready_to_jump)
        {
            Debug.Log("chargejump");

            timer += Time.deltaTime;
            if(timer>=0.2f)
            {
                bird.rb.velocity = new Vector2(0, 0);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space) && ready_to_jump)
        {
            if(timer<0.2f)
            {
                bird.rb.velocity += new Vector2(0, bird.jump_speed);
                bird.GetFSM().ChangeState(Jump.Instance);
            }
            else if(timer>=0.2f&&timer<0.5f)
            {
                if(bird.face_left)
                    bird.rb.velocity += new Vector2(-bird.walk_speed * timer * 2, bird.charge_jump_speed*timer*2);
                else
                    bird.rb.velocity += new Vector2(bird.walk_speed * timer * 2, bird.charge_jump_speed * timer * 2);

                bird.GetFSM().ChangeState(Jump.Instance);
            }
            else if(timer>=0.5f)
            {
                if (bird.face_left)
                    bird.rb.velocity += new Vector2(-bird.walk_speed, bird.charge_jump_speed);
                else
                    bird.rb.velocity += new Vector2(bird.walk_speed , bird.charge_jump_speed);

                bird.GetFSM().ChangeState(Jump.Instance);

            }
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
                bird.rb.velocity = new Vector2(bird.rb.velocity.x / Mathf.Abs(bird.rb.velocity.x) * (Mathf.Abs(bird.rb.velocity.x) + 3), bird.rb.velocity.y + 3);
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

public class Charging_Jump : State<Main_Bird>
{
    public static Charging_Jump Instance { get; private set; }
    float timer;
    float height;

    static Charging_Jump()
    {
        Instance = new Charging_Jump();
    }

    public override void Enter(Main_Bird bird)
    {
        timer = 0;
    }

    public override void Execute(Main_Bird bird)
    {
        if (bird.rb.velocity.y > 0)
        {
            height += bird.rb.velocity.y;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            bird.height = height;

            bird.GetFSM().ChangeState(Gliding.Instance);
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
        bird.rb.velocity = new Vector2(bird.rb.velocity.x, -1);
        bird.rb.gravityScale = 0f;
    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.is_on_ground)
        {
            bird.height = 0;
            bird.GetFSM().ChangeState(Walk.Instance);
        }
        else
        {
            if(Input.GetKey(KeyCode.A))
            {
                bird.face_left = true;
                bird.rb.velocity = new Vector2(-bird.walk_speed, bird.rb.velocity.y);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                bird.face_left = false;
                bird.rb.velocity = new Vector2(bird.walk_speed, bird.rb.velocity.y);
            }
            //if(Input.GetKey(KeyCode.W))
            //{
            //    if (current_height < bird.height) ;
            //        //bird.rb.velocity = new Vector2(bird.rb.velocity.x, 0.5f);
            //}
            //if(Input.GetKeyUp(KeyCode.W))
            //{
            //    bird.rb.velocity = new Vector2(bird.rb.velocity.x, -1);
            //}
            if(Input.GetKey(KeyCode.S))
            {
                bird.rb.velocity = new Vector2(bird.rb.velocity.x, -3);
            }
            else if(Input.GetKeyUp(KeyCode.S))
            {
                bird.rb.velocity = new Vector2(bird.rb.velocity.x, -1);
            }
            else
            {
                bird.GetFSM().ChangeState(Fall.Instance);
            }
        }
        current_height += bird.rb.velocity.y;
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

    }

    public override void Execute(Main_Bird bird)
    {
        if(bird.is_on_ground)
        {
            bird.GetFSM().ChangeState(Walk.Instance);
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            bird.rb.velocity = new Vector2(bird.rb.velocity.x/Mathf.Abs(bird.rb.velocity.x)*bird.gliding_speed, bird.rb.velocity.y);
            bird.GetFSM().ChangeState(Gliding.Instance);
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            bird.rb.velocity = new Vector2(bird.rb.velocity.x / Mathf.Abs(bird.rb.velocity.x) * 8, 3);
        }
    }

    public override void Exit(Main_Bird bird)
    {

    }
}
