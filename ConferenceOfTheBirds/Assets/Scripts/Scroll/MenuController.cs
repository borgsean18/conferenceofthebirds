using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    Scene Game_Scene;
    bool is_in_game;
    Texture2D screenshot;
    Main_Bird bird;
    Vector3 temp_position;
    Vector3 bird_position;
    float bird_health;
    float bird_stamina;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        is_in_game = true;
        bird = GameObject.FindGameObjectWithTag("Player").GetComponent<Main_Bird>();
        temp_position = bird.save_point_position;
    }
    Texture2D CaptureScreenshot2(Rect rect)
    {
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Ωÿ∆¡¡À“ª’≈Õº∆¨: £˚0£˝", filename));
        return screenShot;
    }
    public void Enter_game()
    {
        StartCoroutine(enter());
    }
    IEnumerator enter()
    {
        yield return null;

        Main_Bird bird = GameObject.FindGameObjectWithTag("Player").GetComponent<Main_Bird>();

        bird.health = bird_health;
        bird.gliding_time = bird_stamina;
        bird.save_point_position = temp_position;
        bird.transform.position = bird_position;
    }
    // Update is called once per frame
    IEnumerator change_tex()
    {
        yield return new WaitForSeconds(1);

    }
    IEnumerator do_sc()
    {
        yield return new WaitForEndOfFrame();
        is_in_game = false;
        screenshot = CaptureScreenshot2(new Rect(Screen.width * 0f, Screen.height * 0f, Screen.width * 1f, Screen.height * 1f));
        SceneManager.LoadScene("Mainmenu");
        StartCoroutine(change_pic());
    }
    IEnumerator change_pic()
    {
        yield return null;
        Image a = GameObject.FindGameObjectWithTag("ScrolPic").GetComponent<Image>();
        a.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), Vector2.zero);
        Image temp=GameObject.FindGameObjectWithTag("SCRect").GetComponent<Image>();
        a.rectTransform.position = temp.rectTransform.position;
        a.rectTransform.rect.Set(temp.rectTransform.rect.x, temp.rectTransform.rect.y, width: temp.rectTransform.rect.width, height: temp.rectTransform.rect.height);

    }
    void Update()
    {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
            bird = GameObject.FindGameObjectWithTag("Player").GetComponent<Main_Bird>();

            temp_position = bird.save_point_position;
            bird_position = bird.transform.position;
            bird_health = bird.health;
            bird_stamina = bird.gliding_time;
            StartCoroutine(do_sc());
            }
    }
}
