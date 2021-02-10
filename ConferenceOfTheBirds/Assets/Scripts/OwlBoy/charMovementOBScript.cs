using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charMovementOBScript : MonoBehaviour
{
    //singleton
    public static charMovementOBScript current;


    [Header("Stats")]
    //stats
    public float normalSpeed = 2f;
    private float speed = 0;
    public int jumps = 1;
    public bool flyMode = false;

    [Header("Equipment")]
    public GameObject eqippedItem;
    private GameObject equipmentParent;
    private int selectedItem = 0;

    [Header("Scene Objects")]
    //Camera
    public GameObject mainCameraGameObj;
    private Camera mainCameraComponent;

    //Triggers
    private bool enteredFlymode = false;

    //Components
    private Rigidbody2D rb;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCameraComponent = mainCameraGameObj.GetComponent<Camera>();
        mainCameraComponent.backgroundColor = Color.blue;

        //stats
        speed = normalSpeed;

        //Equipment
        equipmentParent = transform.GetChild(1).gameObject;
        for (int i = 0; i < equipmentParent.transform.childCount; i++)
        {
            equipmentParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        Fly();

        enteredFlymode = false;

        ChangeEquippedItem();
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && floorCheckScript.current.isOnFloor)
        {
            jumps--;
            rb.AddForce(transform.up * 250f);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !floorCheckScript.current.isOnFloor && flyMode == false)
        {
            flyMode = true;
            enteredFlymode = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    private void Fly()
    {
        if (flyMode)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space) && enteredFlymode == false)
            {
                flyMode = false;
                rb.gravityScale = 1;
            }
        }
    }

    private void ChangeEquippedItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            equipmentParent.transform.GetChild(selectedItem).gameObject.SetActive(false);
            selectedItem++;
            if (selectedItem >= equipmentParent.transform.childCount)
            {
                selectedItem = 0;
            }
            equipmentParent.transform.GetChild(selectedItem).gameObject.SetActive(true);
            eqippedItem = equipmentParent.transform.GetChild(selectedItem).gameObject;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            equipmentParent.transform.GetChild(selectedItem).gameObject.SetActive(false);
            selectedItem--;
            if (selectedItem < 0)
            {
                selectedItem = equipmentParent.transform.childCount - 1;
            }
            equipmentParent.transform.GetChild(selectedItem).gameObject.SetActive(true);
            eqippedItem = equipmentParent.transform.GetChild(selectedItem).gameObject;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < equipmentParent.transform.childCount; i++)
            {
                equipmentParent.transform.GetChild(i).gameObject.SetActive(false);
                eqippedItem = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColdPatch")
        {
            speed = 1.5f;
            mainCameraComponent.backgroundColor = Color.Lerp(Color.blue, Color.cyan, 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColdPatch")
        {
            speed = normalSpeed;
            mainCameraComponent.backgroundColor = Color.Lerp(Color.cyan, Color.blue, 3f);
        }
    }
}
