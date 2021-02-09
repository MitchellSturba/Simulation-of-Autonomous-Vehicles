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
    public GameObject BrakeLights;

    //Stores the info for the raycasts
    RaycastHit forwardHit;
    RaycastHit leftHit;
    RaycastHit rightHit;
    RaycastHit rearHit;
    RaycastHit frontLeftHit;
    RaycastHit frontRightHit;


    //car variables
    public float carspeed;
    public float AccelerateSpeed = 2f;
    public float rotatespeed;
    public float cameraDistance;
    bool aboutToCrash = false;
    bool reachedFinish = false;
    bool carIsTurning = false;

    //used to accelerate and decelerate the vehicle.
    bool gasPedal = false;
    float currentSpeed = 0;

    void Start() {
        
        gasPedal = true;

    }

    private void Update()
    {
        
        // debug code to help determine the distance of the raycasts 
        Debug.Log("FORWARD distance: " + forwardHit.distance);
        Debug.Log("REAR Hit distance: " + rearHit.distance);
        Debug.Log("LEFT Hit distance: " + leftHit.distance);
        Debug.Log("RIGHT Hit distance: " + rightHit.distance);
        Debug.Log("FRONT-LEFT Hit distance: " + frontLeftHit.distance);
        Debug.Log("FRONT-RIGHT Hit distance: " + frontRightHit.distance);
        Debug.Log("carIsTurning is " + carIsTurning);

        // If the car is about to crash and/or slow down, turn the brake lights on and then turn them off again after
        if (forwardHit.distance <= 1) {
            aboutToCrash = true;
            // if the car is about to crash and has not completely stopped, turn the brake lights on
            if(forwardHit.distance != 0) {
                BrakeLights.SetActive(true);
            }
            // if the forward hit is 0 and has completely stopped, turn the brake lights off
            else {
                aboutToCrash = false;
                BrakeLights.SetActive(false);
            } 
        }
        // as long as the car is not about to crash, keep the brake lights off
        else {
            aboutToCrash = false;
            BrakeLights.SetActive(false);
        }

        //move the car forward every frame so long as it's not about to crash OR if the forward distance is 0 (the raycast hits nothing in front which means it can keep going straight)
        if (!aboutToCrash || forwardHit.distance == 0)
        {
            //Used to accelerate the car.
            if (currentSpeed < carspeed) currentSpeed += Time.deltaTime * AccelerateSpeed;
            transform.Translate(0, 0, currentSpeed * Time.deltaTime);
        }
            
        // if the car has reached the finish line, stop it from moving
        if(reachedFinish == true) {
            transform.Translate(0, 0, 0);
            BrakeLights.SetActive(true);
            Debug.Log("Car has stopped moving after reaching finish line!");
        }

        //Debug.Log("Carspeed is: " + carspeed);

        // this outer if statement controls when the car stops moving. 
        // if the bool flag "reachedFinish" has been set to true in the if statement in FixedUpdate, then don't allow for the car to keep moving.
        // to revert this change, simply remove the if statement that starts directly below, and take out the two inner if statements.
        if(reachedFinish == false) {
            
            //if left distance is greater than right distance, rotate the car LEFT
            if (leftHit.distance > rightHit.distance + 5 && rightHit.distance != 0 && frontLeftHit.distance > frontRightHit.distance + 3 || (forwardHit.distance < 4 && forwardHit.distance != 0))
            {
                if(leftHit.distance > 25) {
                    carIsTurning = true;
                    if(forwardHit.distance < 10) {
                        transform.Rotate(0,-rotatespeed * Time.deltaTime, 0);
                        BrakeLights.SetActive(true);
                        // debug code to know when the car is turning left
                        Debug.Log("Turning LEFT");
                        currentSpeed = 4.5F;
                    }
                }
                else {
                    carIsTurning = false;
                }
                if(carIsTurning == false) {
                    transform.Rotate(0,-rotatespeed * Time.deltaTime, 0);
                    BrakeLights.SetActive(true);
                    // debug code to know when the car is turning left
                    Debug.Log("Turning LEFT");
                    currentSpeed = 4.5F;

                }
            }
            // extra if-case so that when the raycast extends to infinity (aka, hits nothing), it still turns LEFT
            if(leftHit.distance == 0 || frontLeftHit.distance == 0) {
                transform.Rotate(0,-rotatespeed * Time.deltaTime, 0);
                BrakeLights.SetActive(true);
                // debug code to know when the car is turning left
                Debug.Log("Turning LEFT");
                currentSpeed = 4.5F;
            }
            //if right distance is greater than left distance, rotate the car RIGHT
            if (rightHit.distance > leftHit.distance - 3 && leftHit.distance != 0 && frontRightHit.distance > frontLeftHit.distance - 3)
            {
                if(frontRightHit.distance > 25) {
                    carIsTurning = true;
                    if(forwardHit.distance < 10) {
                        transform.Rotate(0, (rotatespeed + 10) * Time.deltaTime, 0);
                        BrakeLights.SetActive(true);
                        // debug code to know when the car is turning right
                        Debug.Log("Turning RIGHT");
                        currentSpeed = 4.5F;
                    }
                }
                else {
                    carIsTurning = false;
                }
                transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
                BrakeLights.SetActive(true);
                // debug code to know when the car is turning right
                Debug.Log("Turning RIGHT");
                currentSpeed = 4.5F;
            }
            // extra if-case so that when the raycast extends to infinity (aka, hits nothing), it still turns RIGHT
            if(rightHit.distance == 0 || frontRightHit.distance == 0) {
                transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
                BrakeLights.SetActive(true);
                // debug code to know when the car is turning right
                Debug.Log("Turning RIGHT");
                currentSpeed = 4.5F;
            }
            //Debug.Log("currentSpeed is: " + currentSpeed);
            
        }
        // reverse the car if necessary
       // if(forwardHit.distance < 0.4) {
        //    transform.Translate(0, 0, -(carspeed * Time.deltaTime));
        //}

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
            //Debug.Log("it hit: " + rightHit.collider);
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
