using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTex : MonoBehaviour
{
    public RenderTexture RT;
    
    // Start is called before the first frame update
    void Start()
    {
        RT.width = Screen.width;
        RT.height = Screen.height;

        //if (GetComponent<Camera>().targetTexture != null)
        //{
        //    GetComponent<Camera>().targetTexture.Release();
        //}

        //RT = new RenderTexture(Screen.width, Screen.height, 24);
    }

}
