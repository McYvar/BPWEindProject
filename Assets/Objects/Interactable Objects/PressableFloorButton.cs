using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableFloorButton : MonoBehaviour, IPressable
{
    private Vector3 originalPositionButton;

    public GameObject button;

    public bool pressed { get; set; }
    public bool stayActive { get; set; }
    public bool remainActive;


    private void Start()
    {
        pressed = false;
        stayActive = remainActive;
        if (button != null)
            originalPositionButton = button.transform.localPosition;
    }


    private void Update()
    {
        if (button != null)
        {
            if (pressed)
            {
                button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localScale.y * 0.1f, button.transform.localPosition.z);
            }
            else
            {
                button.transform.localPosition = originalPositionButton;
            }
        }
    }


    public void PressObject()
    {
        pressed = true;
    }


    public void UnpressObject()
    {
        if (!stayActive) pressed = false;
    }
}
