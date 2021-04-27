using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{
    public int wind_x_direction;// right is 1, left is -1, not left-right direction is 0
    public int wind_y_direction;// up is 1, down is -1, not up-down direction is 0
    public float time_cool_down;
    public float wind_x_force;
    public float wind_y_force;
    float timer=0;
    bool player_is_in;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            //print("hello");
            player_is_in = true;
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(wind_x_force * wind_x_direction, wind_y_force * wind_y_direction));
            timer = 0;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_is_in = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > time_cool_down)
        {
            timer = 0;
            if (player_is_in)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().AddForce(new Vector2(wind_x_force * wind_x_direction, wind_y_force * wind_y_direction));
            }
        }

    }
}
