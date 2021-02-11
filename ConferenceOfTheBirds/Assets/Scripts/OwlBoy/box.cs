using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject door;

    private bool openDoor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (openDoor)
        {
            OpenDoor();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "pressureplate")
        {
            openDoor = true;
        }
    }

    private void OpenDoor()
    {
        door.transform.position += Vector3.up * 10f * Time.deltaTime;
    }
 //I am a box, I contain things. Sometimes people fill me with their memories and put me in the attic. It is not that bad here,
 //I met a spider once, he's name is Greg. Greg built his nest inside me. Together we look at the old photo albums of this family.
 //We know all about them, all their secrets...
}
