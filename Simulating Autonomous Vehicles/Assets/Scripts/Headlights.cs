using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// **********NEW**************
// this simple script is for the headlights to be able to turn ON and OFF.
public class Headlights : MonoBehaviour
{

    private Light Headlight;
    public bool lightsAreOn = true;
    // Start is called before the first frame update
    void Start()
    {
        Headlight = GetComponent<Light>();

        //Turns lights on/off depending on if the box is checked
        if (lightsAreOn) Headlight.enabled = true;
        if (!lightsAreOn) Headlight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the user presses the '6' key, turn the front headlights OFF.
        if (Input.GetKey(KeyCode.Alpha6)) {
            Headlight.enabled = false;
        }
        // if the user presses the '7' key, turn the front headlights ON.
        if (Input.GetKey(KeyCode.Alpha7)) {
            Headlight.enabled = true;
        }
    }
}
