using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpongeCleaning : MonoBehaviour
{
    bool Is_In;
    public int max_clean_count;
    int already_cleaned;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PollitionAir")
        {
            if(already_cleaned<max_clean_count)
            {
                Is_In = true;
                print("enter");
                Destroy(collision.gameObject);
                already_cleaned += 1;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
