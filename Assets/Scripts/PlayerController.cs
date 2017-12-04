using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Internal References
    private Transform trans;
    private Rigidbody rb;

    //Player Attributes
    [SerializeField] private float movementSpeed = 4;

    //Checks
    private bool playerIsBeingControlled = true;
    private bool playerIsOnShip = true;

    //Mouse Look References and Attributes
    private Transform CameraTrans; //The transform of the camera attached to this player.

    [SerializeField] private float mouseSensitivity = 100.0f; //The speed at which the camera looks around.
    [SerializeField] private float clampAngle = 80.0f; //The angle at which you are stopped from looking up/down.

    private float rotY = 0.0f; //Rotation around the y axis
    private float rotX = 0.0f; //Rotation around the x axis

    //External References
    private Transform currentShipTrans; //The current ships transform, grabbed from GameManager


    void Start()
    {
        //Assigning internal references.
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        //Set the cameraTrans reference to the first child of this player.
        CameraTrans = trans.GetChild(0);

        //Grab the current ship transform from the GameManager
        currentShipTrans = GameManager.singleton.currentShip.GetComponent<Transform>();

        //Store the camera's intial rotation inside rot.
        Vector3 rot = CameraTrans.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
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
            //Move the player in the direction the camera is facing, by deltaTime to make it framerate independant.
            trans.position = trans.position + CameraTrans.forward * step;
        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            trans.position = trans.position - CameraTrans.forward * step;
        }

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            trans.position = trans.position + CameraTrans.right * step;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            trans.position = trans.position - CameraTrans.right * step;
        }
    }

    void mouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        /*Set the rotation around the Y axis to that of the mouse's horizontal movement, multiplied
        by the given sensitivity. Do the same for the rotation around the x axis, but using the 
        mouse's vertical movement.*/
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        //Clamp the up/down movement by the given clampAngle.
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation;

        /*If the player is a child of the ship, make sure the camera's rotation changes to that
        of mouse movement, but also when the ship rotates. If it isn't a child of the ship, 
        just react solely to mouse movement.*/
        if (playerIsOnShip)
        {
            localRotation = Quaternion.Euler(rotX, rotY + currentShipTrans.rotation.eulerAngles.y, 0.0f);
        }
        else
        {
            localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        }

        CameraTrans.rotation = localRotation;
    }

    void OnTriggerStay(Collider col)
    {
        //If the player is in the ship control zone and presses 'F'
        if (col.gameObject.tag == "ShipControlZone" && Input.GetKeyDown(KeyCode.F))
        {
            takeControlOfShip(!playerIsBeingControlled);
        }
    }

    void takeControlOfShip(bool takeControl)
    {
        /*If the ship is being controlled, and this player is also being controlled,
        don't attempt to take control of the ship as someone else is already controlling
        the ship.*/
        if (GameManager.singleton.shipIsBeingControlled && playerIsBeingControlled)
        {
            return;
        }

        //Rotate the camera to face 90 degrees when first taking control.
        if (!takeControl)
        {
            rotY = 90;
        }

        playerIsBeingControlled = takeControl;
        GameManager.singleton.shipIsBeingControlled = !takeControl;
    }
}
