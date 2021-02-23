using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBonus : MonoBehaviour
{
    public float magic_point;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            collision.GetComponent<Main_Bird>().magic_to_save += magic_point;
            collision.GetComponent<Main_Bird>().magic_to_save_slider.value = collision.GetComponent<Main_Bird>().magic_to_save;
            collision.GetComponent<Main_Bird>().magic_to_save = collision.GetComponent<Main_Bird>().magic_to_save % collision.GetComponent<Main_Bird>().max_magic_to_save;
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
