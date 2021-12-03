using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpaceObject : MonoBehaviour, IPressable
{
    public bool pressed { get; set; }
    public bool stayActive { get; set; }
    public bool remainActive;

    private void Start()
    {
        pressed = false;
        stayActive = remainActive;
    }


    public void PressObject()
    {
        pressed = true;
    }


    public void UnpressObject()
    {
        if (!stayActive) pressed = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerSphere"))
        {
            PressObject();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerSphere"))
        {
            UnpressObject();
        }
    }

}
