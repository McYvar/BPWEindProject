using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableObject : MonoBehaviour, IPressable
{
    private Vector3 originalPositionButton;

    public GameObject button;

    public int amountOfObjectsOnButton { get; set; }

    public bool pressed { get; set; }

    private void Start()
    {
        amountOfObjectsOnButton = 0;
        originalPositionButton = button.transform.localPosition;
        pressed = false;
    }


    private void Update()
    {
        if (amountOfObjectsOnButton > 0)
        {
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localScale.y, button.transform.localPosition.z);
            pressed = true;
        }
        else
        {
            button.transform.localPosition = originalPositionButton;
            pressed = false;
        }
    }

    public void AddObject()
    {
        amountOfObjectsOnButton++;
    }

    public void RemoveObject()
    {
        amountOfObjectsOnButton--;
    }

}
