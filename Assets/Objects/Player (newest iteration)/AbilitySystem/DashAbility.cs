using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float speed;
    public override void Activate(GameObject obj)
    {
        base.Activate(obj);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Player player = obj.GetComponent<Player>();
        GameObject orientation = player.orientation;
        rb.AddForce(orientation.transform.forward * speed, ForceMode.VelocityChange);
    }
}
