using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableFloorButton : MonoBehaviour, IPressable
{
    private Vector3 originalPositionButton;

    public GameObject button;

    public bool activateObject { get; set; }
    public bool stayActive { get; set; }
    public bool remainActive;


    private void Start()
    {
        activateObject = false;
        stayActive = remainActive;
        if (button != null) originalPositionButton = button.transform.localPosition;
    }


    private void Update()
    {
        // If the button is activated it moves down a little in game
        if (button != null)
        {
            if (activateObject)
            {
                button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localScale.y * 0.1f, button.transform.localPosition.z);
            }
            else
            {
                button.transform.localPosition = originalPositionButton;
            }
        }
    }


    // Subroutine by IPressable, if the object is pressed it activates a mechanism
    public void PressObject()
    {
        activateObject = true;
    }


    // Then after some time it unpresses...
    public void UnpressObject()
    {
        if (!stayActive) activateObject = false;
    }

}
