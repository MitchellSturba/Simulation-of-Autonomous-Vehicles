using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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
    RaycastHit leftDownHit;


    //car variables
    public float carspeed;
    public float AccelerateSpeed = 2f;
    public float rotatespeed;
    public float cameraDistance;
    float sensorRange;  // change how far the sensor reaches based on how fast the car is currently going.
    bool aboutToCrash = false;
    bool reachedFinish = false;
    bool carIsTurning = false;
    bool hasSeenSceneBefore = false;    // this is for when the program compares the current scene's txt file to its collection

    //used to accelerate and decelerate the vehicle.
    bool gasPedal = false;
    float currentSpeed = 0;

    Scene sceneName;
    string currentScene;
    string fileName;

    void Start() {
        
        gasPedal = true;

        // save the object ids to a txt file that is named after the current scene
        sceneName = SceneManager.GetActiveScene();
        currentScene = sceneName.name;
        fileName = currentScene + ".txt";

        // reset the objects id/names info file before running the program
        StreamWriter writer = new StreamWriter(fileName, false);
        writer.Close();

        sensorRange = cameraDistance;   // initialize the sensorRange to be the same as the cameraDistance variable
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
            sensorRange = 50;
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
            sensorRange = 100;
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
                        sensorRange = 60;
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
                    sensorRange = 60;
                }
            }
            // extra if-case so that when the raycast extends to infinity (aka, hits nothing), it still turns LEFT
            if(leftHit.distance == 0 || frontLeftHit.distance == 0) {
                transform.Rotate(0,-rotatespeed * Time.deltaTime, 0);
                BrakeLights.SetActive(true);
                // debug code to know when the car is turning left
                Debug.Log("Turning LEFT");
                currentSpeed = 4.5F;
                sensorRange = 60;
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
                        sensorRange = 60;
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
                sensorRange = 60;
            }
            // extra if-case so that when the raycast extends to infinity (aka, hits nothing), it still turns RIGHT
            if(rightHit.distance == 0 || frontRightHit.distance == 0) {
                transform.Rotate(0, rotatespeed * Time.deltaTime, 0);
                BrakeLights.SetActive(true);
                // debug code to know when the car is turning right
                Debug.Log("Turning RIGHT");
                currentSpeed = 4.5F;
                sensorRange = 60;
            }
            //Debug.Log("currentSpeed is: " + currentSpeed);
            
        }
        // reverse the car if necessary
       // if(forwardHit.distance < 0.4) {
        //    transform.Translate(0, 0, -(carspeed * Time.deltaTime));
        //}
        Debug.Log("SensorRange is: " + sensorRange);
    }

    private void FixedUpdate()
    {
        //Getting directions of cameras;
        Vector3 fwd = FrontCamera.transform.TransformDirection(Vector3.forward);
        Vector3 left = LeftCamera.transform.TransformDirection(Vector3.left);
        Vector3 leftDown = LeftCamera.transform.TransformDirection(new Vector3(-1, -1, 0));
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
        if (Physics.Raycast(LeftCamera.transform.position, left, out leftHit, sensorRange, layerMask))
        {
            Debug.DrawRay(LeftCamera.transform.position, left * leftHit.distance, Color.red);
            Debug.Log("There is something to the left of the car!");
            //Color color = leftHit.transform.gameObject.GetComponent<Renderer>().material.color;
            //Debug.Log("Object color: " + color);
            //Debug.Log("Object id: " + leftHit.transform.gameObject.GetInstanceID());
            //Debug.Log("Object id name: " + leftHit.transform.gameObject.name);

            // save the object ids to a txt file that is named after the current scene
            sceneName = SceneManager.GetActiveScene();
            currentScene = sceneName.name;
            fileName = currentScene + ".txt";

            // This will save the object ids and names of what the car sees ("hits") into a text file
            // could be helpful when trying to determine if the car has already seen its surroundings (by comparing the text file with a new list of the current environment)
            if(leftHit.transform.gameObject.GetComponent<UniqueId>() != null) {
                using(StreamWriter writer = new StreamWriter(fileName, true)) {
                    writer.WriteLine("object id LEFT: " + leftHit.transform.gameObject.GetComponent<UniqueId>().uniqueId);
                    //writer.WriteLine("object id: " + leftHit.transform.gameObject.GetInstanceID());
                    writer.WriteLine("object name LEFT: " + leftHit.transform.gameObject.name);
                }
            }
        }
        //Right
        if (Physics.Raycast(RightCamera.transform.position, right, out rightHit, sensorRange, layerMask))
        {
            Debug.DrawRay(RightCamera.transform.position, right * rightHit.distance, Color.red);
            Debug.Log("There is something to the right of the car!");
            //Debug.Log("it hit: " + rightHit.collider);

            // save the object ids to a txt file that is named after the current scene
            sceneName = SceneManager.GetActiveScene();
            currentScene = sceneName.name;
            fileName = currentScene + ".txt";

            if(rightHit.transform.gameObject.GetComponent<UniqueId>() != null) {
                using(StreamWriter writer = new StreamWriter(fileName, true)) {
                    writer.WriteLine("object id RIGHT: " + rightHit.transform.gameObject.GetComponent<UniqueId>().uniqueId);
                    //writer.WriteLine("object id: " + leftHit.transform.gameObject.GetInstanceID());
                    writer.WriteLine("object name RIGHT: " + rightHit.transform.gameObject.name);
                }
            }
        }
        //Behind
        if (Physics.Raycast(RearCamera.transform.position, back, out rearHit, sensorRange, layerMask))
        {
            Debug.DrawRay(RearCamera.transform.position, back * rearHit.distance, Color.red);
            Debug.Log("There is something behind the car!");
        }
        //FrontLeft
        if (Physics.Raycast(FrontLeftCamera.transform.position, FrontLeftCamera.transform.TransformDirection(Vector3.left), out frontLeftHit, sensorRange, layerMask))
        {
            Debug.DrawRay(FrontLeftCamera.transform.position, FrontLeftCamera.transform.TransformDirection(Vector3.left) * frontLeftHit.distance, Color.red);
        }
        //FrontRight
        if (Physics.Raycast(FrontRightCamera.transform.position, FrontRightCamera.transform.TransformDirection(Vector3.right), out frontRightHit, sensorRange, layerMask))
        {
            Debug.DrawRay(FrontRightCamera.transform.position, FrontRightCamera.transform.TransformDirection(Vector3.right) * frontRightHit.distance, Color.red);
        }
        // frontDown
        if (Physics.Raycast(LeftCamera.transform.position, leftDown, out leftDownHit, 5, layerMask))
        {
            Debug.DrawRay(LeftCamera.transform.position, leftDown * leftDownHit.distance, Color.blue);
            //Color color = leftDownHit.transform.gameObject.GetComponent<Renderer>().material.color;
            //Debug.Log("Down Left Object color: " + color);
        }
    }

    // this is where (temporarily) the program will check the txt file that was just created/updated with the files in the "collection"
    private void OnDestroy() {
        Debug.Log("The game has ended.");

        // get the name of the current scene to open the corresponding text file to read and compare
        sceneName = SceneManager.GetActiveScene();
        currentScene = sceneName.name;
        fileName = currentScene + ".txt";

        // keep track of how long the files are
        int currentFileLength = 0;
        int comparisonFileLength = 0;
        // total number of lines that are equal
        int numberOfEqualLines = 0;
        // this is what will determine if the scene has been seen or not (since it will loop through multiple scenes for comparison)
        int finalVerdict = 0;
        
        // create a list that holds all of the scene names in txt formats to be compared with
        List<string> fileNamesList = new List<string>();
        fileNamesList.Add("Mini City.txt");
        fileNamesList.Add("LoopTrack.txt");
        fileNamesList.Add("TestMini City.txt");
        //fileNamesList.add("Windridge City Demo Scene.txt");

        // loop through each file that exists (but do not include itself otherwise it will always say it has seen the scene before)
        foreach (string name in fileNamesList) {
            // only compare the files if it is not comparing with itself
            if (name.Equals(fileName) == false) {
                // this will open the text file that was just generated and all of the other files
                using (StreamReader reader = new StreamReader(fileName)) {
                    using(StreamReader readerComparing = new StreamReader(name)) {
                        string lineCurrent;
                        string lineComparison;
                        Debug.Log("Comparing " + fileName + " and " + name);
                        
                        // loop until either file has ended (maybe try to fix so that it keeps going until the longer file is done)
                        //while(lineCurrent = reader.ReadLine() != null || lineComparison = readerComparing.readLine() != null)
                        while(((lineCurrent = reader.ReadLine()) != null) && ((lineComparison = readerComparing.ReadLine()) != null)) {
                            currentFileLength++;
                            comparisonFileLength++;
                            
                            // if the lines are equal
                            if(lineCurrent == lineComparison) {
                                numberOfEqualLines++;
                                //Debug.Log("Line was matched");
                            }
                            // if the final number of equal lines is not zero
                            if(numberOfEqualLines != 0) {
                                // mark the variable as true which means the scene has been seen before
                                hasSeenSceneBefore = true;
                            }
                            else {
                                hasSeenSceneBefore = false;
                            }
                        }
                        // if this variable is false, the car has not been in this scene before
                        if(hasSeenSceneBefore == false) {
                            Debug.Log("Current File Length: " + currentFileLength);
                            Debug.Log("Comparison File Length: " + comparisonFileLength);
                            Debug.Log("This is a different/new scene that the car has NOT seen yet.");
                            Debug.Log("Total equal lines: " + numberOfEqualLines);
                        }
                        // otherwise, the car has seen this scene before
                        else {
                            finalVerdict++;
                            Debug.Log("Current File Length: " + currentFileLength);
                            Debug.Log("Comparison File Length: " + comparisonFileLength);
                            Debug.Log("The car HAS seen this scene before in its collection.");
                            Debug.Log("Total equal lines: " + numberOfEqualLines);
                        }
                    }
                }
            }
            // reset the values for the next iteration of the loop
            hasSeenSceneBefore = false;
            numberOfEqualLines = 0;
            currentFileLength = 0;
            comparisonFileLength = 0;
        }
        if(finalVerdict != 0) {
            Debug.Log("Final Verdict: the car HAS seen this scene before");
        }
        else {
            Debug.Log("Final Verdict: the car has NOT seen this scene before");
        }

    }


}
