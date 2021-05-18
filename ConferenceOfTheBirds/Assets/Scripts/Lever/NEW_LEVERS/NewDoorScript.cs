using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public int activeLevers = 0;
    public int totalLevers = 3;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAllLevers()
    {
        activeLevers++;

        if (activeLevers == totalLevers)
        {
            isOpen = true;
            sr.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sr.enabled)
        {
            if (collision.tag == "Player")
            {
                if (sr.enabled)
                {
                    //load next scene
                    StartCoroutine(LevelLoader.current.FadeToNextScene());
                }
            }
        }
    }
}
