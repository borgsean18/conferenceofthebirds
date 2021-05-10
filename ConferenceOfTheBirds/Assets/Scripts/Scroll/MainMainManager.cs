using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMainManager : MonoBehaviour
{
    public Animator anim1;
    public Animator anim2;
    // Start is called before the first frame update
    public void EnterGame()
    {
        anim1.SetFloat("Speed", -1.8f);
        anim1.Play("ScrollFoldAnim");
        anim2.SetFloat("Speed", -1.8f);
        anim2.Play("MainMenuAnimation");
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadSceneAsync("Scroll");

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Settings()
    {

    }
}
