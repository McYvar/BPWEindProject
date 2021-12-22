using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StrafeAblitiy : Ability
{
    public float strafeForce;
    public KeyCode key;

    public override KeyCode GetActiveKey()
    {
        return key;
    }


    public override bool GetKeyDown(GameObject obj)
    {
        // Based on the in "Inspector" given key return a bool if this key is pressed
        if (Input.GetKeyDown(key)) return true;
        else return false;
    }


    public override void Activate(GameObject obj)
    {
        base.Activate(obj);

        // This ability strafes the player in a certain direction by adding a force to that direction
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        GameObject orientation = obj.GetComponent<Player>().orientation;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.AddForce(orientation.transform.right * strafeForce, ForceMode.VelocityChange);
    }

}
