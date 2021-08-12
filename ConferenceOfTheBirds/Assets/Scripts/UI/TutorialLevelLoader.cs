using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevelLoader : MonoBehaviour
{
    public Animator transition;

    public static TutorialLevelLoader current;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator FadeToNextScene()
    {
        yield return new WaitForSeconds(5f);

        transition.SetTrigger("start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}