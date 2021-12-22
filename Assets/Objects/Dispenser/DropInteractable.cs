using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInteractable : MonoBehaviour
{
    #region Variables and such
    public GameObject button;
    public GameObject objectToInstatiate;
    private IPressable buttonInterface;
    private bool tempBool;
    private GameObject clone;
    #endregion


    #region Start and Update
    private void Start()
    {
        buttonInterface = this.button.GetComponent<IPressable>();
        tempBool = true;
    }


    private void Update()
    {
        // If the in "Inspector" assigned object with the IPressable interfaces is active then a drop is instantiated
        if (buttonInterface != null)
        {
            bool pressed = buttonInterface.activateObject;
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
    #endregion


    // Subroutine to create and object and if one exists already destroy that one to make sure theres only one in the room
    private void InstantiateObject()
    {
        if (clone != null) Destroy(clone);
        clone = Instantiate(objectToInstatiate, transform.position, transform.rotation);
    }
}
