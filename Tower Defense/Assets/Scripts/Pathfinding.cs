using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    // Transform class contains object position, rotation, and scale
    public Transform start; // Reference to the start transform (need for position)
    public Transform end;

    [Space(5)]

    public GameObject pathModeObj;
    public GameObject normalModeObj;

	private TableSetup table;
    private bool pathMode = true;

    private void Awake()
    {
		table = GetComponent<TableSetup>(); // Reference to the Grid class
    }

    // Update is called once at every frame 
    private void Update()
    {
        FindShortestPath(start.position, end.position);

        // Trigger between the path mode and the normal mode
        if (Input.GetKeyDown(KeyCode.T))
        {
            pathModeObj.SetActive(!pathMode);
            normalModeObj.SetActive(pathMode);

            pathMode = !pathMode;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // The A* algorithm implementation for finding the shortest path from "startPos" to "endPos" position
    private void FindShortestPath(Vector3 startPos, Vector3 endPos)
    {
        // Using the position of the start and end points, obtain its nodes
		Node startNode = table.NodeFromPosition(startPos); 
		Node endNode = table.NodeFromPosition(endPos);

		List<Node> openList = new List<Node>(); // Not expanded nodes
		HashSet<Node> closedList = new HashSet<Node>(); // Expanded nodes (visited nodes)
		openList.Add(startNode);

		while (openList.Count > 0)
        {
			Node currNode = openList[0];

            // Get the smallest f cost node
            for (int i = 1; i < openList.Count; i++)
            {   
				if (openList[i].fCost <= currNode.fCost)
                {
					if (openList[i].hCost < currNode.hCost)
                    {
						currNode = openList[i];
                    }
				}
			}

			openList.Remove(currNode);
			closedList.Add(currNode);

            // If we found our path, go from the "endNode" to the stating node (adding them to a list) and return
			if (currNode == endNode)
            {
				FindStartToEndPath(startNode, endNode);

				return;
			}

            // Loop through each of the neighbor of the current node
			foreach (Node neighbour in table.GetNodeNeighbours(currNode))
            {
                // If the neightbor node is blocked, or it was already chosen for the path, skip to the next neighbor
				if (!neighbour.notBlocked || closedList.Contains(neighbour))
                {
					continue;
				}

                // The g cost of the neighbor is the 'g cost of current node' + 'distance from the node to neighbor'
				int neighborGCost = currNode.gCost + CalcDistance(currNode, neighbour); 

                // If the new g cost is cheaper, or if the open list does not contain the neighbor, update neighbor node information
				if (neighborGCost < neighbour.gCost || !openList.Contains(neighbour))
                {
					neighbour.gCost = neighborGCost;
					neighbour.hCost = CalcDistance(neighbour, endNode);
					neighbour.parent = currNode; // Set the parent of the neighbor node to the current node

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
				}
			}
		}
	}

    // Gets the distance cost between two nodes (h cost)
    // Costs are 1 or sqrt(2), multiplied by 10, diagonal movement is 14, left, right, up, down is 10
    private int CalcDistance(Node nodeA, Node nodeB)
    {
        int width = Mathf.Abs(nodeA.nodeX - nodeB.nodeX);
        int height = Mathf.Abs(nodeA.nodeY - nodeB.nodeY);

        if (width > height)
        {
            // "height" diagonal moves, and "width - height", nondiagonal moves
            return 14 * height + 10 * (width - height); 
        }
        else
        {
            return 14 * width + 10 * (height - width);
        }
	}

    // Follow from the end to the start node to get the path
    private void FindStartToEndPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // Add all the nodes, from the end to the start node
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Add(startNode);

        path.Reverse(); 

        table.startToEndPath = path;

        // Do not update the path of the bot if it made more than 10 moves
        if (PathFollower.step < 2)
        {
            PathFollower.path = path;
        }
    }
}
