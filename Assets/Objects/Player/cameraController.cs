using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
