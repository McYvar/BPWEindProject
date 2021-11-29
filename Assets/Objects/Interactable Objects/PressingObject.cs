using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        component?.buttonPressed();
    }


    private void OnTriggerExit(Collider collider)
    {
        IPressable component = collider.GetComponent<IPressable>();
        component?.buttonUnpressed();
    }
}
