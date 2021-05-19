using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkablePeople : MonoBehaviour
{
    //Variables
    public List<string> DialogeWithThisPerson;
    public bool canTalk = true;
    public bool convoEnded = false;
    public bool inProximity = false;
    public bool CanTalkToThisPerson = true;

    //GameObject
    public GameObject personDialogueBox;
    public GameObject personTextObject;
    public GameObject PlayerDialogueBox;
    public GameObject PlayerTextObject;
    private GameObject Player;

    //Components
    private Text CharacterTextObjText;
    private Text PlayerTextObjText;
    private RectTransform personDialogueBoxRT;
    private RectTransform playerDialogueBoxRT;
    public float personOffset = 3f;
    public float playerOffset = 4f;

    // Start is called before the first frame update
    void Start()
    {
        //Game Objects
        Player = GameObject.FindGameObjectWithTag("Player");

        //Components
        CharacterTextObjText = personTextObject.GetComponent<Text>();
        PlayerTextObjText = PlayerTextObject.GetComponent<Text>();
        personDialogueBoxRT = personDialogueBox.GetComponent<RectTransform>();
        playerDialogueBoxRT = PlayerDialogueBox.GetComponent<RectTransform>();
        personDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + 1f, transform.position.y + 1.5f));
        playerDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(Player.transform.position.x - 1f, transform.position.y + 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (inProximity && Input.GetKeyDown(KeyCode.I) && canTalk && CanTalkToThisPerson)
        {
            StartCoroutine(HaveConversationMethod());
        }

        personDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + 1f, transform.position.y + personOffset));
        playerDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(Player.transform.position.x - 1f, Player.transform.position.y + playerOffset));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inProximity = true;
            
            if (!convoEnded)
                GameManagerScript.current.InteractionButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inProximity = false;
            GameManagerScript.current.InteractionButton.SetActive(false);
        }
    }

    private IEnumerator HaveConversationMethod()
    {
        CanTalkToThisPerson = false;
        canTalk = false;
        Player.GetComponent<Main_Bird>().CanMove = false;
        //CharacterControllingScript.current.canWalk = false;
        //CharacterControllingScript.current.IdleState();

        for (int dialogueCounter = 0; dialogueCounter < DialogeWithThisPerson.Count; dialogueCounter++)
        {
            if (DialogeWithThisPerson[dialogueCounter].StartsWith("O"))
            {
                personDialogueBox.SetActive(true);
                CharacterTextObjText.text = DialogeWithThisPerson[dialogueCounter].Substring(2);
            }
            else
            {
                personDialogueBox.SetActive(false);
            }

            if (DialogeWithThisPerson[dialogueCounter].StartsWith("I"))
            {
                PlayerDialogueBox.SetActive(true);
                PlayerTextObjText.text = DialogeWithThisPerson[dialogueCounter].Substring(2);
            }
            else
            {
                PlayerDialogueBox.SetActive(false);
            }

            yield return waitForKeyPress(KeyCode.I);
        }

        StartCoroutine(TurnOffDialogueBoxesTimer());
        //CharacterControllingScript.current.canWalk = true;
        convoEnded = true;
        GameManagerScript.current.InteractionButton.SetActive(false);
        Player.GetComponent<Main_Bird>().CanMove = true;

        if (GameManagerScript.current.isEpilogue)
        {
            GameManagerScript.current.endCanvas.SetActive(true);
        }
    }

    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done) // essentially a "while true", but with a bool to break out naturally
        {
            if (Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        // now this function returns
    }

    private IEnumerator TurnOffDialogueBoxesTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (personDialogueBox.activeSelf)
            personDialogueBox.SetActive(false);
        if (PlayerDialogueBox.activeSelf)
            PlayerDialogueBox.SetActive(false);
    }
}
