using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpaceObject : MonoBehaviour, IPressable
{
    public bool activateObject { get; set; }
    public bool stayActive { get; set; }
    public bool remainActive;

    private void Start()
    {
        activateObject = false;
        stayActive = remainActive;
    }


    public void PressObject()
    {
        activateObject = true;
    }


    public void UnpressObject()
    {
        if (!stayActive) activateObject = false;
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
