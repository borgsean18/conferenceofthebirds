using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForVideoToPlay : MonoBehaviour
{
    public float lengthOfVideo;
    public Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds(lengthOfVideo);
        //scene transition
        transition.SetTrigger("start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
