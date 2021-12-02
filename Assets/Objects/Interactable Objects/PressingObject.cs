using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingObject : MonoBehaviour
{

    private RemoveInteractableObjects script;
    PressableFloorButton[] button;

    private void Awake()
    {
        script = GameObject.FindObjectOfType<RemoveInteractableObjects>();
        button = GameObject.FindObjectsOfType<PressableFloorButton>();
    }

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
        for (int i = 0; i < button.Length; i++)
        {
            if (button[i] != null)
            {
                IPressable component = button[i].GetComponent<IPressable>();
                if (component != null) component.UnpressObject();
                script.list.Remove(this.gameObject);
            }
        }
    }

    IEnumerator addToList()
    {
        yield return new WaitForEndOfFrame();
        script.list.Add(this.gameObject);
    }
}
