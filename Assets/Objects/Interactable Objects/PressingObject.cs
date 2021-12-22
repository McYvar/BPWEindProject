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
        // If the object stays inside the trigger and the object is of Interface IPressable then...
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.PressObject();
    }


    private void OnTriggerExit(Collider collider)
    {
        // If the object exits the trigger and the object is of Interface IPressable then...
        IPressable component = collider.GetComponent<IPressable>();
        if (component != null) component.UnpressObject();
    }


    private void OnDestroy()
    {
        // If this gameObject gets destroyed then it will be removed from a certain list that keeps track of interactable objects in the room
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
        // If the object is added to the scene it gets added to a list
        yield return new WaitForEndOfFrame();
        script.list.Add(this.gameObject);
    }

}
