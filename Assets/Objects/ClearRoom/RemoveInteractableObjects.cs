using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInteractableObjects : MonoBehaviour
{
    public List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        list = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }
}
