using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Global Variables and References
    [HideInInspector] public bool shipIsBeingControlled = false; //True when any player is controlling the ship.
    public GameObject currentShip; //Inspector stored reference to the current ship. (*Needs to be changed to be automatic somehow);

    public static GameManager singleton = null;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
