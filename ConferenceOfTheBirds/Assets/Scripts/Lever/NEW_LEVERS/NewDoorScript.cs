using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public List<GameObject> levers;

    private List<NewLeverLogic> leverScripts;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        leverScripts = new List<NewLeverLogic>();

        sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < levers.Count; i++)
        {
            leverScripts.Add(levers[i].GetComponent<NewLeverLogic>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAllLevers()
    {
        int leverCounter = 0;
        for (int i = 0; i < leverScripts.Count; i++)
        {
            if (leverScripts[i].isActive)
            {
                leverCounter++;
            }
        }

        if (leverCounter == levers.Count)
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
                GameManagerScript.current.InteractionButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sr.enabled)
        {
            if (collision.tag == "Player")
            {
                if(GameManagerScript.current.InteractionButton.activeSelf)
                    GameManagerScript.current.InteractionButton.SetActive(false);
            }
        }
    }
}
