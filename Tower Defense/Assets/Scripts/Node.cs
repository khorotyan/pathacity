using UnityEngine;

public class Node
{
	public bool notBlocked; // indicates whether the node is an obstacle or not
	public Vector3 position; // the location of the node
	public int nodeX; // grid x position
	public int nodeY;

	public int gCost;
	public int hCost;
	public Node parent;
	
	public Node(bool notBlocked, Vector3 position, int nodeX, int nodeY)
    {
		this.notBlocked = notBlocked;
		this.position = position;
		this.nodeX = nodeX;
		this.nodeY = nodeY;
	}

    // Returns the f cost of the node
	public int fCost
    {
		get
        {
			return gCost + hCost;
		}
	}
}
