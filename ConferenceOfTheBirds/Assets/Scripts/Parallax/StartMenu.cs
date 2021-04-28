using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    bool is_menu;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        animator2.SetTrigger("1");

    }
    public IEnumerator Wait2(float t)
    {
        yield return new WaitForSeconds(t);
        animator1.SetTrigger("2");

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!is_menu)
            {
                is_menu = true;
                GetComponent<Main_Bird>().enabled = false;
                animator1.SetTrigger("1");
                StartCoroutine(Wait(1f));

            }
            else
            {
                is_menu = false;
                GetComponent<Main_Bird>().enabled = true;
                animator2.SetTrigger("2");
                StartCoroutine(Wait2(1f));
            }
        }
        
        
    }

    
}
