using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    public int coinsOwned = 0;
    public AudioClip coinSound;
    public AudioClip healthSound;
    public AudioClip staminaSound;

    private AudioSource asrc;

    // Start is called before the first frame update
    void Start()
    {
        asrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            coinsOwned++;
            Destroy(collision.gameObject);
            asrc.PlayOneShot(coinSound);
        }
        if (collision.gameObject.tag == "Health")
        {
            asrc.PlayOneShot(healthSound);
        }
        if (collision.gameObject.tag == "Stamina")
        {
            asrc.PlayOneShot(staminaSound);
        }
    }
}
