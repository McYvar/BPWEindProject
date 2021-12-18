using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StrafeAblitiy : Ability
{
    public float strafeForce;
    public KeyCode key;

    public override bool GetKeyDown(GameObject obj)
    {
        if (Input.GetKeyDown(key)) return true;
        else return false;
    }


    public override void Activate(GameObject obj)
    {
        base.Activate(obj);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        GameObject orientation = obj.GetComponent<Player>().orientation;
        rb.AddForce(orientation.transform.right * strafeForce, ForceMode.VelocityChange);
    }

}
