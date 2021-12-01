using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingObject : MonoBehaviour
{

    private RemoveInteractableObjects script;

    private void Start()
    {
        StartCoroutine(addToList());
    }

    private void OnTriggerStay(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.PressObject();
    }


    private void OnTriggerExit(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.UnpressObject();
    }


    private void OnDestroy()
    {
        GameObject[] button = GameObject.FindGameObjectsWithTag("InteractableFloorButton");
        for (int i = 0; i < button.Length; i++)
        {
            IPressable component = button[i].GetComponent<IPressable>();
            if (component != null) component.UnpressObject();
            script.list.Remove(this.gameObject);
        }
    }

    IEnumerator addToList()
    {
        yield return new WaitForEndOfFrame();
        script = GameObject.Find("ClearRoom").GetComponent<RemoveInteractableObjects>();
        script.list.Add(this.gameObject);
    }
}
