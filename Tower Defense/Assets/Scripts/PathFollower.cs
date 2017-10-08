using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public GameObject follower;
    public Transform startPoint;

    public static List<Node> path = new List<Node>();
    public static int step = 0;

    private GameObject spawned;
    private float waitTime = 0.2f;
    private float currTimer;
    private bool stepOver = false;
    private bool canMove = false;

    private void Awake()
    {
        // Create a bot at the start of the game
        spawned = Instantiate(follower, startPoint.position, startPoint.rotation);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawned.transform.position = path[0].position;
            step = 0;
            canMove = false;
        }

        if (canMove == true)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            canMove = !canMove;
            spawned.transform.position = path[0].position;
            step = 0;
        }
    }

    // Move the bot
    private void Move()
    {
        // If the path is not null, or we can make another step
        if (path.Count != 0 && step < path.Count - 1)
        {
            if (stepOver == false)
            {
                stepOver = true;
            }
            else // Interpolate the position of the bot between the steps 
            {
                currTimer += 1 * Time.deltaTime;

                spawned.transform.position = Vector3.Lerp(path[step].position, path[step+1].position, currTimer / waitTime);

                // Face the direction of travel
                spawned.transform.rotation = Quaternion.LookRotation(path[step + 1].position - path[step].position);

                if (currTimer >= waitTime)
                {
                    currTimer = 0;
                    step++;
                    stepOver = false;
                }
            }

            // If we reached the end of the path, start over
            if (step >= path.Count - 1)
            {
                canMove = false;
                step = 0;
            }
        }
    }
}
