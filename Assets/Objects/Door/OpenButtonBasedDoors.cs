using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButtonBasedDoors : MonoBehaviour
{

    public GameObject button;

    Vector3 originalPosition;


    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        IPressable buttonInterface = button.GetComponent<IPressable>();
        if (buttonInterface != null)
        {
            bool isPressed = buttonInterface.pressed;

            if (isPressed)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localScale.z * 1.5f);
            }
            else
            {
                transform.localPosition = originalPosition;
            }
        }
    }
}
