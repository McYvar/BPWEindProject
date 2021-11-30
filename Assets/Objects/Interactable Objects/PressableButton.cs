using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableButton : MonoBehaviour, IPressable
{
    public GameObject button;

    public int amountOfObjectsOnButton { get; set; }

    public bool pressed { get; set; }


    private void Start()
    {
        pressed = false;
    }


    public void PressObject()
    {
        if (!pressed)
        {
            pressed = true;
            StartCoroutine(TimeTillButtonRelease());
            button.transform.localPosition = new Vector3(0, -transform.localScale.y / 20, 0);
        }
    }

    public void UnpressObject()
    {
    }

    IEnumerator TimeTillButtonRelease()
    {
        yield return new WaitForSeconds(3f);
        pressed = false;
        button.transform.localPosition = new Vector3(0, 0, 0);
    }
}
