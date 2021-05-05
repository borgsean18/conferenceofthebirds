using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    Material m;
    BoxCollider2D[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<SpriteRenderer>().material;
        colliders = GetComponents<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Main_Bird temp = collision.GetComponent<Main_Bird>();
            print("touchbreakablewall");
            if (temp.GetFSM().CurrentState() == Air_Dash.Instance)
            {
                foreach (BoxCollider2D c in colliders)
                {
                    c.enabled = false;
                }
                StartCoroutine(Fade());
            }
        }
    }
    public IEnumerator Fade()
    {
        float a = 0;
        
        while(a<0.8)
        {
            a += Time.deltaTime;
            m.SetFloat("_Edge",a);
            yield return null;
        }
        print("shoulddestroy");
        Destroy(transform.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
