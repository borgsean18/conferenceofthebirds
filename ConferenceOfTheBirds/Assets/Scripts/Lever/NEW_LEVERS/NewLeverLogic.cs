using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLeverLogic : MonoBehaviour
{
    public bool isActive = false;
    public bool isPlayerClose = false;
    public GameObject door;
    public GameObject doorLight;

    // Start is called before the first frame update
    void Start()
    {
        doorLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                isActive = true;
                doorLight.SetActive(true);
                door.GetComponent<NewDoorScript>().CheckAllLevers();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerClose = true;
            GameManagerScript.current.InteractionButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerClose = false;
            GameManagerScript.current.InteractionButton.SetActive(false);
        }
    }
}
