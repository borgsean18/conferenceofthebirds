using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilitiesScript : MonoBehaviour
{
    //Abilities
    public enum abilities
    {
        none,
        powerDash,
    }
    public abilities currentAbility = abilities.none;

    private float power = 0;

    //Animate vibrate
    private bool canVibrate = true;
    private float x;

    //Singleton
    public static CharacterAbilitiesScript current;

    private void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentAbility)
        {
            case abilities.none:
                //do Nothing
                break;
            case abilities.powerDash:
                PowerDash();
                break;
        }
    }
        

    private void PowerDash()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovementStateMachineScript.current.currentState = MovementStateMachineScript.state.none;
            x = transform.position.x;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Vibrate());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //shoot into that direction
            //...
            power = power * 10;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(power, 0));
            MovementStateMachineScript.current.currentState = MovementStateMachineScript.state.walking;
            power = 0;
        }
    }

    private IEnumerator Vibrate()
    {
        if (canVibrate)
        {
            if (power < 800)
            {
                power += 5f;
            }
            canVibrate = false;
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y);
            yield return new WaitForSeconds(0.05f);
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y);
            yield return new WaitForSeconds(0.05f);
            canVibrate = true;
        }
    }
}