using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tiny class that destroys a objects a few seconds after spawning
public class DestroyInSeconds : MonoBehaviour
{
    public float seconds;

    private void Update()
    {
        if (seconds <= 0) Destroy(gameObject);
        seconds -= Time.deltaTime;
    }
}
