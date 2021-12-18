using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExtraJumpAbility : Ability
{
    public float jumpForce;
    public KeyCode key;

    public override bool GetKeyDown(GameObject obj)
    {
        Player player = obj.GetComponent<Player>();
        if (!player.onGround)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        return false;
    }


    public override void Activate(GameObject obj)
    {
        base.Activate(obj);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Player player = obj.GetComponent<Player>();

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector2.up * jumpForce * player.flip, ForceMode.VelocityChange);
    }

}
