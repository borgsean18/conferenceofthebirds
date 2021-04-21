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
    private bool inProximity = false;
    public bool CanTalkToThisPerson = true;

    //GameObject
    public GameObject personDialogueBox;
    public GameObject personTextObject;
    public GameObject PlayerDialogueBox;
    public GameObject PlayerTextObject;

    public GameObject PersonB;
    public Text PersonBText;

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
        //Components
        CharacterTextObjText = personTextObject.GetComponent<Text>();
        PlayerTextObjText = PlayerTextObject.GetComponent<Text>();
        personDialogueBoxRT = personDialogueBox.GetComponent<RectTransform>();
        playerDialogueBoxRT = PlayerDialogueBox.GetComponent<RectTransform>();
        personDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + 1.5f));
        //playerDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(CharacterControllingScript.current.transform.position.x, transform.position.y + 1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (inProximity && Input.GetKeyDown(KeyCode.Space) && canTalk && CanTalkToThisPerson)
        {
            StartCoroutine(HaveConversationMethod());
        }

        personDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + personOffset));
        //playerDialogueBoxRT.position = Camera.main.WorldToScreenPoint(new Vector2(CharacterControllingScript.current.transform.position.x, CharacterControllingScript.current.transform.position.y + playerOffset));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inProximity = true;

            //if (canTalk)
            //    UIManagerScript.current.ActivateInteractableObjNotification(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inProximity = false;

            //if (UIManagerScript.current.InteractableObjNotification.activeSelf)
            //    UIManagerScript.current.DeActivateInteractableObjNotification();
        }
    }

    private IEnumerator HaveConversationMethod()
    {
        CanTalkToThisPerson = false;
        canTalk = false;
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

            if (DialogeWithThisPerson[dialogueCounter].StartsWith("S"))
            {
                PersonB.SetActive(true);
                PersonBText.text = DialogeWithThisPerson[dialogueCounter].Substring(2);
            }
            else if (PersonB != null)
            {
                PersonB.SetActive(false);
            }

            yield return waitForKeyPress(KeyCode.Space);
        }

        //UIManagerScript.current.DeActivateInteractableObjNotification();
        StartCoroutine(TurnOffDialogueBoxesTimer());
        //CharacterControllingScript.current.canWalk = true;
        convoEnded = true;
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
