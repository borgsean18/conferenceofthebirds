using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : MonoBehaviour
{
    public bool isOpen = false;
    public List<GameObject> levers;

    private List<NewLeverLogic> leverScripts;

    // Start is called before the first frame update
    void Start()
    {
        leverScripts = new List<NewLeverLogic>();

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
            isOpen = true;
    }
}
