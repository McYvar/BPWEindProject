using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tiny class that defines the damage dealth if it hits or the object with Interface IDamagable hits this gameObject
public class impact : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable component = collision.gameObject.GetComponent<IDamagable>();
        if (component != null)
        {
            component.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
