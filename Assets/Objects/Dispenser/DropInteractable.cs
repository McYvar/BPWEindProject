using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInteractable : MonoBehaviour
{
    public GameObject button;
    public GameObject objectToInstatiate;
    private IPressable buttonInterface;
    private bool tempBool;
    private GameObject clone;

    private void Start()
    {
        buttonInterface = this.button.GetComponent<IPressable>();
        tempBool = true;
    }


    private void Update()
    {
        if (buttonInterface != null)
        {
            bool pressed = buttonInterface.pressed;
            if (pressed && tempBool)
            {
                tempBool = false;
                InstantiateObject();
            }

            if (!pressed)
            {
                tempBool = true;
            }
        }
    }


    private void InstantiateObject()
    {
        if (clone != null) Destroy(clone);
        clone = Instantiate(objectToInstatiate, transform.position, transform.rotation);
    }
}
