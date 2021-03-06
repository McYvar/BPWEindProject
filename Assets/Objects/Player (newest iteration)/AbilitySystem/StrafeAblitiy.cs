using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StrafeAblitiy : Ability
{
    public float strafeForce;

    public override void Activate(GameObject obj)
    {
        base.Activate(obj);

        // This ability strafes the player in a certain direction by adding a force to that direction
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Player player = obj.GetComponent<Player>();
        GameObject orientation = player.orientation;
        rb.AddForce(orientation.transform.right * player.flip * strafeForce, ForceMode.VelocityChange);
    }

}
