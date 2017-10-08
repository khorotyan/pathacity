using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Destroy the car if it reaches the end of the map
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable")
        {
            if (AnimateObjects.moveables.Contains(other.gameObject))
            {
                AnimateObjects.moveables.Remove(other.gameObject);
            }

            if (AnimateObjects.moveables2.Contains(other.gameObject))
            {
                AnimateObjects.moveables2.Remove(other.gameObject);
            }

            Destroy(other.gameObject);
        }
    }
}
