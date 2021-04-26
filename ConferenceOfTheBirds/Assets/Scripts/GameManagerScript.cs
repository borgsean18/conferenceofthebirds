using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    //UI
    [Header("UI")]
    public GameObject NotificationsCanvas;
    public GameObject levelCompleteText;

    //Singleton
    public static GameManagerScript current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //NotificationsCanvas.SetActive(false);
    }

    public void EndLevel()
    {
        //do end level logic
        print("level Over");
        NotificationsCanvas.SetActive(true);
        StartCoroutine(GoToNextScene(2f));
    }

    private IEnumerator GoToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        //go to next scene
    }
}
