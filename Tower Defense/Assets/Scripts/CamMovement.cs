using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private float camSpeed = 45f;
    private float scrollMult = 100f;

    private void Update()
    {
        // If the key "Shift" is pressed, make the movement and rotation speeds faster
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            camSpeed *= 2;
            scrollMult *= 2;
        }

        // If we let go the button "Shift", the movement and rotation speeds will get slower
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            camSpeed /= 2;
            scrollMult /= 2;
        }

        // Move the camera if horizontal or 
        transform.position += Input.GetAxis("Vertical") == 0 ? Vector3.zero : Input.GetAxis("Vertical") > 0 ? Vector3.forward * camSpeed * Time.deltaTime : Vector3.back * camSpeed * Time.deltaTime;
        transform.position += Input.GetAxis("Horizontal") == 0 ? Vector3.zero : Input.GetAxis("Horizontal") > 0 ? transform.rotation * Vector3.right * camSpeed * Time.deltaTime : transform.rotation * Vector3.left * camSpeed * Time.deltaTime;
        transform.position += new Vector3(0, -Input.GetAxis("Mouse ScrollWheel") * camSpeed * scrollMult * Time.deltaTime, 0);

        // Clamp the position values between some points to prevent the user from going to far away from the landscape
        float clampedXPos = Mathf.Clamp(transform.position.x, -50, 50);
        float clampedYPos = Mathf.Clamp(transform.position.y, 30, 120);
        float clampedZPos = Mathf.Clamp(transform.position.z, -75, 50);

        // Set the clamped values of the camera 
        transform.position = new Vector3(clampedXPos, clampedYPos, clampedZPos);  
    }
}