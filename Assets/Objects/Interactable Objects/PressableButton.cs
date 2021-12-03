using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButton : MonoBehaviour, IPressable
{
    public GameObject button;

    public bool pressed { get; set; }

    public float timeTillRelease;

    public bool stayActive { get; set; }
    public bool remainActive;

    private void Start()
    {
        pressed = false;
        stayActive = remainActive;
    }


    public void PressObject()
    {
        if (!pressed)
        {
            pressed = true;
            if (!stayActive)
            {
                StartCoroutine(TimeTillButtonRelease());
            }
            button.transform.localPosition = new Vector3(0, -transform.localScale.y / 20, 0);
        }
    }


    public void UnpressObject()
    {
    }


    IEnumerator TimeTillButtonRelease()
    {
        yield return new WaitForSeconds(timeTillRelease);
        pressed = false;
        button.transform.localPosition = new Vector3(0, 0, 0);
    }

}
