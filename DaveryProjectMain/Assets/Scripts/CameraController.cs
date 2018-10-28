using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform Dave;
    public Vector3 offset;
    public Vector3 lockCameraPositionLeft;
    public Vector3 lockCameraPositionRight;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Dave.position.x >= lockCameraPositionLeft.x && Dave.position.x <= lockCameraPositionRight.x)
        {

            transform.position = new Vector3(Dave.position.x + offset.x, Dave.position.y + offset.y, Dave.position.z + offset.z);
        }
        
	}
}
