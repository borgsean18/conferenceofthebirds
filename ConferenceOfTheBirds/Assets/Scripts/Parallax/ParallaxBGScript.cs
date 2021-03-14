using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBGScript : MonoBehaviour
{
    [SerializeField] private float parallaxEffectMultiplier;

    public GameObject camera;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = camera.transform;
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxEffectMultiplier;
        lastCameraPosition = cameraTransform.position;
    }
}
