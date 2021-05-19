using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollectCoins : MonoBehaviour
{
    public int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coins == 3)
        {
            StartCoroutine(TutorialLevelLoader.current.FadeToNextScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coins++;
        }
    }
}
