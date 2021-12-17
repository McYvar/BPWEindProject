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
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * Mathf.Cos(Time.time * bounceSpeed) * bopHeight * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamagable thing = other.GetComponent<IDamagable>();
        if (thing != null) thing.takeDamage(-20);

        if (other.CompareTag("Player")) Destroy(this.gameObject);
    }
}
