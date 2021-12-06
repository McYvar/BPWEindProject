using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    PlayerStateManager playerScript;

    private void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerScript.Dead();
        else Destroy(other.gameObject);
    }
}
