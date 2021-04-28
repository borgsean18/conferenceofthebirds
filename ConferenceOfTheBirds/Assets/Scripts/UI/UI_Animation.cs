using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Animation : MonoBehaviour
{
    public Animator animator3;
    // Start is called before the first frame update
    void Start()
    {
        //animator3 = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Start_UI_Anim()
    {
        animator3.SetTrigger("1");
    }
    public void End_UI_Anim()
    {
        animator3.SetTrigger("2");
    }
}
