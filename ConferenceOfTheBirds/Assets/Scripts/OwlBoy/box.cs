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
}
