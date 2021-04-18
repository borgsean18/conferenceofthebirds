using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public float health_recovered;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            collision.GetComponent<Main_Bird>().health += health_recovered;
            collision.GetComponent<Main_Bird>().health_slider.value = collision.GetComponent<Main_Bird>().health;
            if(collision.GetComponent<Main_Bird>().health> collision.GetComponent<Main_Bird>().max_health)
                collision.GetComponent<Main_Bird>().health = collision.GetComponent<Main_Bird>().max_health;
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
