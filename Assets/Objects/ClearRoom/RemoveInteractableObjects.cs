using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInteractableObjects : MonoBehaviour
{
    // First I create a list in wich certain objects in the room will be stored
    public List<GameObject> list;

    void Start()
    {
        list = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        // At the end of every room all the objects in the room added in this list will be removed
        for(int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }
}
