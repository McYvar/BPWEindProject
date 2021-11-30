using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTriggerBasedDoors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            leftDoor.transform.localPosition = new Vector3(0, 0, 0);
            rightDoor.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
