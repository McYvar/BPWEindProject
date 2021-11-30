using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.PressObject();
    }


    private void OnTriggerExit(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.UnpressObject();
    }
}
