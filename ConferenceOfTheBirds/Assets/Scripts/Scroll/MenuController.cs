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
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        is_in_game = true;
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
        yield return new WaitForSeconds(1);
        Image a = GameObject.FindGameObjectWithTag("ScrolPic").GetComponent<Image>();
        a.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), Vector2.zero);
        a.SetNativeSize();
    }
    void Update()
    {
        if(is_in_game)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(do_sc());
            }
        }
    }
}
