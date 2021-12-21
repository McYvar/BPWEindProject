using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    public float seconds;

    private void Update()
    {
        if (seconds <= 0) Destroy(gameObject);
        seconds -= Time.deltaTime;
    }
}
