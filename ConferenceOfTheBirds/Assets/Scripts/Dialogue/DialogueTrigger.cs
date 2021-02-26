using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject DialogueManager;
    private DialogueManagerScript dms;
    public bool canTalk = false;
    public Dialogue dialogue;

    private void Start()
    {
        dms = DialogueManager.GetComponent<DialogueManagerScript>();
    }

    private void Update()
    {
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue()
    {
        dms.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canTalk = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canTalk = false;
    }
}
