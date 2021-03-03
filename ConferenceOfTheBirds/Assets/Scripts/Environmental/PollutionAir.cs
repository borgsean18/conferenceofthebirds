using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionAir : MonoBehaviour
{
    bool is_in;
    float timer;
    Main_Bird bird;
    public float damage_per_sec;
    // Start is called before the first frame update
    void Start()
    {
        bird = GameObject.FindGameObjectWithTag("Player").GetComponent<Main_Bird>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            is_in = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            is_in = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(is_in)
        {
            if (timer < 0.2)
            {
                timer += Time.deltaTime;
            }
            else
            {
                bird.get_hurt(damage_per_sec / 5);
                timer = 0;
            }
        }
    }
}
