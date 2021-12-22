using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float rotationSpeed;
    public float bopHeight;
    public float bounceSpeed;

    private void Update()
    {
        // Makes a tiny animation based on rotation and position
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * Mathf.Cos((Time.time * bounceSpeed)) * bopHeight * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // If the object has interface IDamagable then it gets 20 healt and destroys this gameObject
        IDamagable thing = other.GetComponent<IDamagable>();
        if (thing != null) {
            thing.takeDamage(-20);
            Destroy(gameObject);
        }
    }

}
