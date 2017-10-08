using UnityEngine;
using System.Collections.Generic;

public class TableSetup : MonoBehaviour
{
    public GameObject pathViewShower;
	public LayerMask blockerLayerMask; // blocker layer
	public Vector2 worldSize; // size of the world - width x height
	public float nodeWidth; // how much space each node occupies
    public List<Node> startToEndPath;

    private Node[,] nodeArr;
    private Color[] colorcontainer;

    private float nodeLength;
    private int worldSizeX;
    private int worldSizeY;

    // Awake is like a constructor, it is used for initialization, it is called when the script instance is being loaded
    private void Awake()
    {
		nodeLength = nodeWidth * 2;
		worldSizeX = Mathf.RoundToInt(worldSize.x / nodeLength); // number of nodes in x axis
		worldSizeY = Mathf.RoundToInt(worldSize.y / nodeLength);

        colorcontainer = new Color[worldSizeX * worldSizeY];

        MakeNodeTable();

        ApplyTexture();
    }

    private void Update()
    {
        MakeNodeTable();
        ApplyTexture();
    }

    // Creates the node table
    private void MakeNodeTable()
    {
		nodeArr = new Node[worldSizeX, worldSizeY]; // Initiallize our grid

        // calculate the position of the worlds bottom left coordinate
        Vector3 worldBottomLeft = transform.position - new Vector3(worldSize.x / 2, 0, worldSize.y / 2);

        // Loop through all the world blocks 
		for (int x = 0; x < worldSizeX; x ++)
        {
			for (int y = 0; y < worldSizeY; y ++)
            {
                // The coordinate of the current node
                Vector3 position = worldBottomLeft + new Vector3(x * nodeLength + nodeWidth, 0, y * nodeLength + nodeWidth);

                // true, if we do not collide with anything in the blocker layer mask
                //      checks for a collision in "worldPoint" coordinate, with radius "nodeWidth", and layerMask, "blockerLayerMask"
                bool notBlocked = !(Physics.CheckSphere(position, nodeWidth, blockerLayerMask));

				nodeArr[x, y] = new Node(notBlocked, position, x, y);
			}
		}
	}

    // Return a list of nodes of the current node, which are its neighbors
	public List<Node> GetNodeNeighbours(Node node)
    {
		List<Node> neighbours = new List<Node>();

        // Loop around the current node (except the middle node)
		for (int x = -1; x <= 1; x++)
        {
			for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

				int currNodeX = node.nodeX + x;
				int currNodeY = node.nodeY + y;

                // If the node number is inside the boundaries of its array (world), then add it to our neighbor list
				if (currNodeX >= 0 && currNodeX < worldSizeX && currNodeY >= 0 && currNodeY < worldSizeY)
                {
					neighbours.Add(nodeArr[currNodeX, currNodeY]);
				}
			}
		}

		return neighbours;
	}
	
    // Returns a node depending on the world position
	public Node NodeFromPosition(Vector3 worldPosition)
    {
        // Convert a world position into a percantage, which says how far away the node is from bottom left
		float percentX = Mathf.Clamp01((worldPosition.x + worldSize.x / 2) / worldSize.x);
		float percentY = Mathf.Clamp01((worldPosition.z + worldSize.y / 2) / worldSize.y);

        // Get the node (x, y) coordinate, which is the actual node position in the grid array
		int xPos = Mathf.RoundToInt((worldSizeX - 1) * percentX); 
		int yPos = Mathf.RoundToInt((worldSizeY - 1) * percentY);

		return nodeArr[xPos, yPos];
	}

    // Sets the texture colors depending on object blocking, and shortest path finding
    private void CalculateTextureColors()
    {
        for (int y = 0; y < worldSizeY; y++)
        {
            for (int x = 0; x < worldSizeX; x++)
            {
                if (nodeArr[x, y].notBlocked == false)
                {
                    colorcontainer[y * worldSizeX + x] = new Color32(220, 20, 60, 100); 
                }
                else
                {
                    colorcontainer[y * worldSizeX + x] = new Color32(73, 163, 75, 100);
                }

                if (startToEndPath != null)
                {
                    // If the node is in the path
                    for (int i = 0; i < startToEndPath.Count; i++)
                    {
                        if (startToEndPath[i].nodeX == x && startToEndPath[i].nodeY == y)
                        { 
                            colorcontainer[y * worldSizeX + x] = Color.black; // Color the path with black
                        }
                    }
                }
            }
        }
    }

    // Applies the texture to the gameObject
    private void ApplyTexture()
    {
        CalculateTextureColors();

        Texture2D texture = new Texture2D(worldSizeX, worldSizeY);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorcontainer);

        texture.Apply();

        Renderer rend = pathViewShower.GetComponent<Renderer>();
        rend.sharedMaterial.mainTexture = texture;
    }
}