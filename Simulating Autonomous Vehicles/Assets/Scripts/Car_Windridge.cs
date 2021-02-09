using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Windridge : MonoBehaviour
{
    [HideInInspector]
    public float carSpeed;
    public float maxSpeed;
    public float Acceleration = 2f;
    public bool gasPressed = true;
    public bool braking = false;
    float currentSpeed = 0;

    //cordinates from the gps



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gasPressed)
        {
            if (carSpeed < maxSpeed)
            {
                carSpeed += Acceleration * Time.deltaTime;
            }
            else
            {
                Acceleration = 0;
            }
            transform.Translate(0, 0, carSpeed * Time.deltaTime);
        }

        //In order to control the car manually
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(0,-25 * Time.deltaTime,0);
        }
        //In order to control the car manually
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 25 * Time.deltaTime, 0);
        }
        // allow the car to brake by holding the S key (slows down on key down, resumes speed on key UP)
        if(Input.GetKeyDown(KeyCode.S))
        {
            carSpeed = 2.3F;
            braking = true;
            transform.Translate(0, 0, carSpeed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.S)) {
            braking = false;
            carSpeed = 5;
            transform.Translate(0, 0, carSpeed * Time.deltaTime);
        }
    }
}
