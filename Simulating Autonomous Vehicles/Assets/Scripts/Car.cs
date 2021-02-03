using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    //Positions of the cameras
    public Transform FrontCamera;
    public Transform LeftCamera;
    public Transform RightCamera;
    public Transform RearCamera;
    public Transform FrontLeftCamera;
    public Transform FrontRightCamera;

    //Stores the info for the raycasts
    RaycastHit forwardHit;
    RaycastHit leftHit;
    RaycastHit rightHit;
    RaycastHit rearHit;
    RaycastHit frontLeftHit;
    RaycastHit frontRightHit;


    //car variables
    public float carspeed;
    public float rotatespeed;
    public float cameraDistance;
    bool aboutToCrash = false;
    bool reachedFinish = false;

    private void Update()
    {

        Debug.Log(forwardHit.distance);
        if (forwardHit.distance <= 1) aboutToCrash = true;
        else aboutToCrash = false;

        //move the car forward every frame so long as it's not about to crash
        if (!aboutToCrash) transform.Translate(0, 0, carspeed * Time.deltaTime);
        
        // **********NEW**************
        // if the car has reached the finish line, stop it from moving
        if(reachedFinish == true) {
            transform.Translate(0, 0, 0);
            Debug.Log("Car has stopped moving after reaching finish line!");
        }
        // **********NEW**************
        // this outer if statement controls when the car stops moving. 
        // if the bool flag "reachedFinish" has been set to true in the if statement in FixedUpdate, then don't allow for the car to keep moving.
        // to revert this change, simply remove the if statement that starts on line 50 and ends on line on 61, and take out the two inner if statements.
        if(reachedFinish == false) {
            //if left distance is greater than right distance, rotate the car left
            if (leftHit.distance > rightHit.distance + 4)
            {
                transform.Rotate(0,-rotatespeed * Time.deltaTime, 0);
            }
            //if right distance is greater than left distance, rotate the car right
            if (rightHit.distance > leftHit.distance + 4)
            {
                transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        //Getting directions of cameras;
        Vector3 fwd = FrontCamera.transform.TransformDirection(Vector3.forward);
        Vector3 left = LeftCamera.transform.TransformDirection(Vector3.left);
        Vector3 right = RightCamera.transform.TransformDirection(Vector3.right);
        Vector3 back = RearCamera.transform.TransformDirection(Vector3.back);

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(FrontCamera.transform.position, fwd, out forwardHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(FrontCamera.transform.position, fwd * forwardHit.distance, Color.red);

            // **********NEW**************
            // this simply checks to see if the car has reached the "finish line" and should stop the car when it reaches it
            // to prevent the console from being spammed after it reaches the finish line, add something like '&& !reachedFinish' to the end of every if statement in FixedUpdate.
            if(forwardHit.collider.gameObject.tag == "finishLine") {
                Debug.Log("The car has hit the finishLine!");
                reachedFinish = true;
            }
            Debug.Log("There is something in front of the car!");
        }
        //Left
        if (Physics.Raycast(LeftCamera.transform.position, left, out leftHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(LeftCamera.transform.position, left * leftHit.distance, Color.red);
            Debug.Log("There is something to the left of the car!");
        }
        //Right
        if (Physics.Raycast(RightCamera.transform.position, right, out rightHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(RightCamera.transform.position, right * rightHit.distance, Color.red);
            Debug.Log("There is something to the right of the car!");
        }
        //Behind
        if (Physics.Raycast(RearCamera.transform.position, back, out rearHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(RearCamera.transform.position, back * rearHit.distance, Color.red);
            Debug.Log("There is something behind the car!");
        }
        //FrontLeft
        if (Physics.Raycast(FrontLeftCamera.transform.position, FrontLeftCamera.transform.TransformDirection(Vector3.left), out frontLeftHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(FrontLeftCamera.transform.position, FrontLeftCamera.transform.TransformDirection(Vector3.left) * frontLeftHit.distance, Color.red);
        }
        //FrontRight
        if (Physics.Raycast(FrontRightCamera.transform.position, FrontRightCamera.transform.TransformDirection(Vector3.right), out frontRightHit, cameraDistance, layerMask))
        {
            Debug.DrawRay(FrontRightCamera.transform.position, FrontRightCamera.transform.TransformDirection(Vector3.right) * frontRightHit.distance, Color.red);
        }
    }


}
