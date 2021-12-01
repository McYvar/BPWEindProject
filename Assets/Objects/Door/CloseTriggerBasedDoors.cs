using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTriggerBasedDoors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public float doorSpeed;

    private void Start()
    {
        doorSpeed *= -1;
    }


    private void LateUpdate()
    {
        leftDoor.transform.localPosition += Vector3.forward * doorSpeed * Time.deltaTime;
        leftDoor.transform.localPosition = new Vector3(leftDoor.transform.localPosition.x, leftDoor.transform.localPosition.y, Mathf.Clamp(leftDoor.transform.localPosition.z, 0, 1.75f));

        rightDoor.transform.localPosition += Vector3.back * doorSpeed * Time.deltaTime;
        rightDoor.transform.localPosition = new Vector3(rightDoor.transform.localPosition.x, rightDoor.transform.localPosition.y, Mathf.Clamp(rightDoor.transform.localPosition.z, -1.75f, 0));
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSpeed = Mathf.Abs(doorSpeed);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSpeed *= -1;
        }
    }

}
