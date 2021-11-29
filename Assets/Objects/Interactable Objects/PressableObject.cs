using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableObject : MonoBehaviour, IPressable
{
    private Vector3 originalPosition;
    private Vector3 originalScale;

    public GameObject button;

    public bool pressed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = button.transform.position;
        originalScale = button.transform.localScale;
        pressed = false;
    }

    public void buttonPressed()
    {
        button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + (button.transform.localPosition.y / 2), button.transform.localPosition.z);
        pressed = true;
    }


    public void buttonUnpressed()
    {
        button.transform.position = originalPosition;
        button.transform.localScale = originalScale;
        pressed = false;
    }

}
