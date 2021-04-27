using System.Collections;
using System.Collections.Generic;
using Lean;
using UnityEngine;

public class TrainingToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject RecordButton;
    public UISystem uisystem;
    public GameObject AutonomousTextField;
    public GameObject ManualTextField;
    bool on = false;
    private void Start()
    {
        //Since the toggle starts off by default
        RecordButton.SetActive(false);
        uisystem.isTraining = false;
    }

    public void Toggle()
    {
        on = !on;

        if (on)
        {
            RecordButton.SetActive(true);
            ManualTextField.SetActive(true);
            AutonomousTextField.SetActive(false);
            uisystem.isTraining = true;
        }
        else
        {
            RecordButton.SetActive(false);
            ManualTextField.SetActive(false);
            AutonomousTextField.SetActive(true);
            uisystem.isTraining = false;
        }
    }

}
