using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[HideInInspector]
	public PlayerMovement PlayerMovement;
	//private int mouseSensitivity = 100;
	[HideInInspector]
	public Transform playerBody;
    
	float xRotation = 0f;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
	    float mouseX = Input.GetAxis("Mouse X") * PlayerMovement.mouseSensitivity * Time.deltaTime;
	    float mouseY = Input.GetAxis("Mouse Y") * PlayerMovement.mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
