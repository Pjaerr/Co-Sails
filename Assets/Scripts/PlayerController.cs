using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool playerIsBeingControlled = true;
    private Transform trans;
    [SerializeField] private float movementSpeed = 4;

    //Mouse Look
    private Transform CameraTrans;

    [SerializeField] private float mouseSensitivity = 100.0f; //The speed at which the camera looks around.
    [SerializeField] private float clampAngle = 80.0f; //The angle at which you are stopped from looking up/down.

    private float rotY = 0.0f; //Rotation around the y axis
    private float rotX = 0.0f; //Rotation around the x axis

    void Start()
    {
        trans = GetComponent<Transform>();

        CameraTrans = trans.GetChild(0);

        Vector3 rot = CameraTrans.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        mouseLook();
        if (playerIsBeingControlled)
        {
            movePlayer();
        }
    }

    void movePlayer()
    {
        float step = movementSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            trans.position = trans.position + CameraTrans.forward * step;
        }
    }

    void mouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        CameraTrans.rotation = localRotation;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "ShipControlZone" && Input.GetKeyDown(KeyCode.F))
        {
            takeControlOfShip(!playerIsBeingControlled);
        }
    }

    void takeControlOfShip(bool takeControl)
    {
        //Do a check to see if noone else is controlling ship first.
        playerIsBeingControlled = takeControl;
        GameManager.singleton.shipIsBeingControlled = !takeControl;

    }
}
