using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    //Internal References
    private Transform trans;
    private Rigidbody rb;

    //Ship Attributes
    [SerializeField] private float movementSpeed = 4;
    [SerializeField] private float turnSpeed = 4;

    void Start()
    {
        //Sets internal references
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //If a player is controlling the ship, check for input and act accordingly.
        if (GameManager.singleton.shipIsBeingControlled)
        {
            controlShip();
        }
    }

    void controlShip()
    {
        float step = movementSpeed * Time.deltaTime;
        float x = 0;

        turnShip(step);

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            x += 1;
        }

        trans.Translate(new Vector3(x * step, 0, 0));
    }

    void turnShip(float step)
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            trans.Rotate(new Vector3(0, -turnSpeed * step, 0));
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            trans.Rotate(new Vector3(0, turnSpeed * step, 0));
        }
    }
}
