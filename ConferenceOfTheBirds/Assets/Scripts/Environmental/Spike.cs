using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float damage;
    public float offset_force_x;
    public float offset_force_y;
    public float hurt_cool_down;
    GameObject player;
    float timer;
    bool is_enter;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            print("fucku");
            collision.GetComponent<Main_Bird>().get_hurt(damage);
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(offset_force_x, offset_force_y));
            is_enter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        is_enter = false;
    }
    private void Update()
    {
        if(is_enter)
        {
            timer += Time.deltaTime;
            if(timer>hurt_cool_down)
            {
                player.GetComponent<Main_Bird>().get_hurt(damage);
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(offset_force_x, offset_force_y));
                timer = 0;
            }
        }
    }
}
