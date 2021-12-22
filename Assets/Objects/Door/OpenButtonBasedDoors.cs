using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButtonBasedDoors : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject button;

    public float doorSpeed;
    IPressable buttonInterface;

    private void Start()
    {
        buttonInterface = button.GetComponent<IPressable>();
    }


    private void Update()
    {
        // If the in "Inspector" assigned object with the IPressable interfaces is active then the doors will open
        float doorSpeed = this.doorSpeed;

        if (buttonInterface != null)
        {
            bool isPressed = buttonInterface.activateObject;
            if (isPressed)
            {
                doorSpeed *= -1;
            }
        }

        leftDoor.transform.localPosition += Vector3.back * doorSpeed * Time.deltaTime;
        leftDoor.transform.localPosition = new Vector3(leftDoor.transform.localPosition.x, leftDoor.transform.localPosition.y, Mathf.Clamp(leftDoor.transform.localPosition.z, 0, 1.75f));

        rightDoor.transform.localPosition += Vector3.forward * doorSpeed * Time.deltaTime;
        rightDoor.transform.localPosition = new Vector3(rightDoor.transform.localPosition.x, rightDoor.transform.localPosition.y, Mathf.Clamp(rightDoor.transform.localPosition.z, -1.75f, 0));
    }

}
