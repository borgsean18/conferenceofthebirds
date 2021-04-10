using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoving : MonoBehaviour
{
    public float speed;
    public float wait_time;
    public Transform Start_Point;
    public Transform End_Point;
    Vector3 direction;
    bool to_end;
    bool is_moving;
    Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
        direction = (End_Point.position - Start_Point.position).normalized;
        distance = End_Point.position - Start_Point.position;
        Start_Point.position = transform.position;
        End_Point.position = transform.position + distance;
        to_end = true;
        is_moving = true;
    }
    public IEnumerator Wait(float t)
    {
        is_moving = false;
        yield return new WaitForSeconds(t);
        is_moving = true;
    }
    // Update is called once per frame
    void Update()
    {

        if (is_moving)
        {
            if (Mathf.Abs((transform.position - Start_Point.position).magnitude) < 0.1f)
            {
                to_end = true;
                StartCoroutine(Wait(wait_time));
            }
            else if (Mathf.Abs((transform.position - End_Point.position).magnitude) < 0.1f)
            {
                to_end = false;
                StartCoroutine(Wait(wait_time));

            }
            if (to_end)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                transform.position += direction * -speed * Time.deltaTime;
            }
        }
    }
}
