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


    // Subroutine by IPressable to activate on entering the trigger
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
        // If an object with the tag or player enters then...
        if (other.CompareTag("TriggerObject") || other.CompareTag("Player"))
        {
            PressObject();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // If an object with the tag or player exits then...
        if (other.CompareTag("TriggerObject") || other.CompareTag("Player"))
        {
            UnpressObject();
        }
    }

}
