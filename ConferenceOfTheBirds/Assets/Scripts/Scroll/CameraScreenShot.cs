using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenShot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Texture2D CaptureScreenshot2(Rect rect)
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
    void Update()
    {
        
    }
}
