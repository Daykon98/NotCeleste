using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomController : MonoBehaviour
{
    [SerializeField] private GameObject roomLeft = null;
    [SerializeField] private GameObject roomRight = null;
    [SerializeField] private GameObject roomUp = null;
    [SerializeField] private GameObject roomDown = null;

    public float screenSizeX = 640;
    public float screenSizeY = 360;
    public float initialPositionX = 0;
    public float initialPositionY = 0;

    //private roomController roomLeftController;
    //private roomController roomRightController;
    //private roomController roomUpController;
    //private roomController roomDownController;

    private void Start()
    {
        
    }

    public GameObject getNextRoom(CustomEnums.directions direction)
    {
        switch (direction)
        {
            case CustomEnums.directions.down:
                return roomDown;
            case CustomEnums.directions.left:
                return roomLeft;
            case CustomEnums.directions.up:
                return roomUp;
            default:
                return roomRight;

        }

    }


}
