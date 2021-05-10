using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMainManager : MonoBehaviour
{
    public Animator anim1;
    public Animator anim2;
    public Vector3 bird_position;
    public Vector3 save_position;
    public float health;
    public float gliding_time;
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
        SceneManager.LoadScene("Parallax");
        GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>().Enter_game();

    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Settings()
    {

    }
}
