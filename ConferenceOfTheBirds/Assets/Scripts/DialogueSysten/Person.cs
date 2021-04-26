using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    //Variables
    public string dialogue;
    private bool canTalk = true;

    //GameObjects
    public GameObject Dialogue;
    public GameObject TextObject;

    //Components
    private RectTransform dialogueBoxRT;

    // Start is called before the first frame update
    void Start()
    {
        //GameObjects
        TextObject.GetComponent<Text>().text = dialogue;

        //Components
        dialogueBoxRT = Dialogue.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        dialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + 4f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canTalk)
        {
            StartCoroutine(SayDialogue());
        }
    }

    private IEnumerator SayDialogue()
    {
        //Say Dialogue
        canTalk = false;
        Dialogue.SetActive(true);
        yield return new WaitForSeconds(3f);
        //Hide Dialogue
        Dialogue.SetActive(false);
    }
}
