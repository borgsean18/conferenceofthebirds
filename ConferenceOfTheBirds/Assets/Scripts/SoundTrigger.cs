using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{

    public AudioSource playSound;

    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playSound.Play();
            print("playing");
            Destroy(gameObject);
        }
    }

}
