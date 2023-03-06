using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckInteractController : MonoBehaviour
{
    public int truckStackCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stackable"))
        {
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    other.transform.position = new Vector3(transform.position.x + i * 3f, transform.position.y, transform.position.z + k * 3f); ;
                }
            }
        }
    }
}
