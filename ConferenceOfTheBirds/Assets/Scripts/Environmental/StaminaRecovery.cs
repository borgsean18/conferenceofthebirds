using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRecovery : MonoBehaviour
{
    public float stamina_recovered;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Main_Bird>().gliding_time += stamina_recovered;
            collision.GetComponent<Main_Bird>().stamina_meter.value = collision.GetComponent<Main_Bird>().gliding_time;
            if (collision.GetComponent<Main_Bird>().gliding_time > collision.GetComponent<Main_Bird>().gliding_time_max)
                collision.GetComponent<Main_Bird>().gliding_time = collision.GetComponent<Main_Bird>().gliding_time_max;
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
