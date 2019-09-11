using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    private moveSmoothly moveCamera;

    private void Start()
    {
        moveCamera = GetComponent<moveSmoothly>();   
    }

    public void move(float movementX, float movementY, float seconds)
    {
        moveCamera.animationSeconds = seconds;
        moveCamera.moveObjectiveX = movementX;
        moveCamera.moveObjectiveY = movementY;
        moveCamera.UpdatePath();
        moveCamera.move();
    }
}

    