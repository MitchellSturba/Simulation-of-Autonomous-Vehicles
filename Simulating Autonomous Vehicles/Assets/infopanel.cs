using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class infopanel : MonoBehaviour
{

    public Car_Windridge CarScript;
    public Headlights HeadlightsScript;
    public TextMeshProUGUI Driving_Mode;
    public TextMeshProUGUI Gas_Pressed;
    public TextMeshProUGUI Braking;
    public TextMeshProUGUI Headlights;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Acceleration;
    public TextMeshProUGUI X;
    public TextMeshProUGUI Y;
    public TextMeshProUGUI Z;


    private void LateUpdate()
    {
        //Setting variables
        float carspeed = CarScript.carSpeed;
        float acceleration = CarScript.Acceleration;
        bool isOn = HeadlightsScript.lightsAreOn;

        //setting text
        Speed.text = (carspeed*18)/5 + " km/h";
        Acceleration.text = acceleration + " km/h";
        X.text = CarScript.transform.position.x.ToString("F2");
        Y.text = CarScript.transform.position.y.ToString("F2");
        Z.text = CarScript.transform.position.z.ToString("F2");
        if(isOn == true) Headlights.text = "ON";
        if(isOn == false) Headlights.text = "OFF";
        Gas_Pressed.text = CarScript.gasPressed.ToString();
        Braking.text = CarScript.braking.ToString();
    }
}
