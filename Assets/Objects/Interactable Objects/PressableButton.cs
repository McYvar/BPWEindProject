using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButton : MonoBehaviour, IPressable
{
    public GameObject button;

    public bool activateObject { get; set; }

    public float timeTillRelease;

    private bool buttonPress;
    public bool stayActive { get; set; }
    public bool remainActive;
    public bool switchMode;

    private void Start()
    {
        activateObject = false;
        buttonPress = false;
        stayActive = remainActive;
    }


    // Subroutine by IPressable, like the floor button, moves down a little when pressed, but it can stay active or will be released after some time
    public void PressObject()
    {
        if (!buttonPress)
        {
            button.transform.localPosition = new Vector3(0, -transform.localScale.y / 20, 0);
            if (switchMode && activateObject) activateObject = false;
            else if (switchMode && !activateObject) activateObject = true;
            else activateObject = true;
            buttonPress = true;
            if (stayActive) return;
            StartCoroutine(TimeTillButtonRelease());
        }
    }


    public void UnpressObject()
    {
    }


    // Subroutine to release the button after some time
    IEnumerator TimeTillButtonRelease()
    {
        yield return new WaitForSeconds(timeTillRelease);
        button.transform.localPosition = new Vector3(0, 0, 0);
        if (!switchMode) activateObject = false;
        buttonPress = false;
    }

}
