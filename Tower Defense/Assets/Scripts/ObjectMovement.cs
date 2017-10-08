using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private bool canMoveObject = false;
    private GameObject moveObject;
    private Vector3 hitLocation;
    private Vector3 prevPoint;
    private float moveSpeed = 0.003f;

	private void Update ()
    {
        // When the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) 
        {
            DetectObjectClick();
        }

        // If we released the left mouse button, we can no longer move the object
        if (Input.GetMouseButtonUp(0))
        {
            canMoveObject = false;
        }

        // If we can move the object
        if (canMoveObject == true)
        {
            MoveObject();
        }
    }

    // Checks whether the mouse click hit a blocker
    private void DetectObjectClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // The ray from screen space to world space
        RaycastHit hit; // Information about what the ray hit

        // Raycast from mouse position and store the hit information in "hit"
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hit a blocker object
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Blocker") ||
                hit.transform.gameObject.tag == "StartPoint" ||
                hit.transform.gameObject.tag == "EndPoint")
            {
                canMoveObject = true; // Now we can move the object that the ray hit
                moveObject = hit.transform.gameObject; // Store information about the object that was clicked
                hitLocation = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y); // Store the mouse hit location

                Debug.Log(hit.transform.gameObject.name);
            }
        }
    }

    // Move the clicked object relative to mouse movement
    private void MoveObject()
    {
        if (Mathf.Abs(prevPoint.x - Input.mousePosition.x) > 0.1 || Mathf.Abs(prevPoint.z - Input.mousePosition.y) > 0.1)
        {
            moveObject.transform.position = moveObject.transform.position - moveSpeed * (hitLocation - new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y));
        }

        prevPoint = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
    } 
}
