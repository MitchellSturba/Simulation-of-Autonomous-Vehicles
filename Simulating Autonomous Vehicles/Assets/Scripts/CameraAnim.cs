using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    Animator CamAnimator;
    AnimatorClipInfo[] info;

    // Start is called before the first frame update
    void Start()
    {
        CamAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) CamAnimator.SetTrigger("First_Person");
        if (Input.GetKey(KeyCode.Alpha2)) CamAnimator.SetTrigger("Wheel_View");
        if (Input.GetKey(KeyCode.Alpha3)) CamAnimator.SetTrigger("Third_Person");
        if (Input.GetKey(KeyCode.Alpha4)) CamAnimator.SetTrigger("Top_Down");

    }
}
