using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
