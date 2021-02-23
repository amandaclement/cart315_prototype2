using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightAngle : MonoBehaviour
{
    private GameObject mainCam;
    //Vector3 mainCamAngle;
    private float mainCamAngle;

    public float rotationSpeed;

    public float posX;
    public float posY;
    public float posZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Mouse.current.position.ReadValue() // New input system

        //Vector3 detAngle;
        //detAngle = Input.mousePosition;

        //bool TopHalf()
        //{
        //    return Input.mousePosition.y > Screen.height / 2.0f;
        //}
        //bool RightHalf()
        //{
        //    return Input.mousePosition.position.x > Screen.width / 2.0f;
        //}

        //if (Input.mousePosition.y > Screen.height / 2.0f)
        //{
        //    Debug.Log("mouse is in top half");
        //}

        //if (Input.mousePosition.x > Screen.width / 2.0f)
        //{
        //    Debug.Log("mouse is in right half");
        //}


        //mainCam = GameObject.FindWithTag("MainCamera");
        mainCamAngle = mainCam.transform.localEulerAngles.x;
        Debug.Log(mainCamAngle);

        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(posX - mainCamAngle, posY, posZ);
        this.transform.localRotation = rot;

    }
}
