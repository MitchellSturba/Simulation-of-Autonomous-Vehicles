using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.SceneManagement;
using TMPro;

public class UISystem : MonoSingleton<UISystem> {

    public CarController carController;
    public string GoodCarStatusMessage;
    public string BadSCartatusMessage;
    public TextMeshProUGUI Speed_text;
    //public Image MPH_Animation;
    public TextMeshProUGUI Angle_Text;
    public TextMeshProUGUI RecordStatus_Text;
	public TextMeshProUGUI DriveStatus_Text;
	public TextMeshProUGUI SaveStatus_Text;
	public TextMeshProUGUI x_Coordinate;
	public TextMeshProUGUI y_Coordinate;
	public TextMeshProUGUI z_Coordinate;
	public TextMeshProUGUI Braking_Status;
	public TextMeshProUGUI Throttle_Text;
    public GameObject RecordingPause; 
	public GameObject RecordDisabled;
	public bool isTraining = false;

    private bool recording;
    private float topSpeed;
	private bool saveRecording;


    // Use this for initialization
    void Start() {
		Debug.Log (isTraining);
        topSpeed = carController.MaxSpeed;
        recording = false;
        RecordingPause.SetActive(false);
		RecordStatus_Text.text = "RECORD";
		DriveStatus_Text.text = "";
		SaveStatus_Text.text = "";
		SetAngleValue(0);
        SetMPHValue(0);
		if (!isTraining) {
			DriveStatus_Text.text = "Mode: Autonomous";
			RecordDisabled.SetActive (false);
			RecordStatus_Text.text = "";
		} 
    }

    public void SetAngleValue(float value)
    {
        Angle_Text.text = value.ToString("N2") + "°";
    }

    public void SetMPHValue(float value)
    {
        Speed_text.text = value.ToString("N2") + " km/h";
        //Do something with value for fill amounts
        //MPH_Animation.fillAmount = value/topSpeed;
    }

    public void SetXYZCoordinates(float x, float y, float z) {
        x_Coordinate.text = x.ToString("N2");
        y_Coordinate.text = y.ToString("N2");
        z_Coordinate.text = z.ToString("N2");
    }

    public void SetBrakingStatus(float brakingValue) {
        if(brakingValue == 0) {
            Braking_Status.text = "Not Braking";
        }
        else {
            Braking_Status.text = "Braking";
        }
    }

    public void SetThrottleValue(float acceleration) {
        Throttle_Text.text = acceleration.ToString("N2");
    }

    public void ToggleRecording()
    {
		// Don't record in autonomous mode
		if (!isTraining) {
			return;
		}

        if (!recording)
        {
			if (carController.checkSaveLocation()) 
			{
				recording = true;
				RecordingPause.SetActive (true);
				RecordStatus_Text.text = "RECORDING";
				carController.IsRecording = true;
			}
        }
        else
        {
			saveRecording = true;
			carController.IsRecording = false;
        }
    }
	
    void UpdateCarValues()
    {
        SetMPHValue(carController.CurrentSpeed);
        SetAngleValue(carController.CurrentSteerAngle);
        SetXYZCoordinates(carController.transform.position.x, carController.transform.position.y, carController.transform.position.z);
        SetThrottleValue(carController.AccelInput);
        SetBrakingStatus(carController.BrakeInput);
    }

	// Update is called once per frame
	void Update () {

        // Easier than pressing the actual button :-)
        // Should make recording training data more pleasant.

		if (carController.getSaveStatus ()) {
			SaveStatus_Text.text = "Capturing Data: " + (int)(100 * carController.getSavePercent ()) + "%";
			//Debug.Log ("save percent is: " + carController.getSavePercent ());
		} 
		else if(saveRecording) 
		{
			SaveStatus_Text.text = "";
			recording = false;
			RecordingPause.SetActive(false);
			RecordStatus_Text.text = "RECORD";
			saveRecording = false;
		}

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRecording();
        }

		if (!isTraining) 
		{
			if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) 
			{
				DriveStatus_Text.color = Color.red;
				DriveStatus_Text.text = "Mode: Manual";
			} 
			else 
			{
				DriveStatus_Text.color = Color.white;
				DriveStatus_Text.text = "Mode: Autonomous";
			}
		}

	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Do Menu Here
            SceneManager.LoadScene("MenuScene");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Do Console Here
        }

        UpdateCarValues();
    }
}
