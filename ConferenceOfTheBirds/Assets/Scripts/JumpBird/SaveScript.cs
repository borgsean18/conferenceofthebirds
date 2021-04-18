using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    Transform Respawn_Position;
    // Start is called before the first frame update
    void Start()
    {
        Respawn_Position = transform.GetChild(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            print("trigger");
            collision.GetComponent<Main_Bird>().save_point_position = Respawn_Position.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
