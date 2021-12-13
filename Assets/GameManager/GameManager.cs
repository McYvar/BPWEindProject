using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player playerScript;

    private void Awake()
    {
        Physics.gravity = Vector3.down * 20f;
        playerScript = GameManager.FindObjectOfType<Player>();

        playerScript?.OnAwake();
    }


    private void Start()
    {
        // Lock and hide the cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerScript?.OnStart();
    }


    private void Update()
    {
        playerScript?.OnUpdate();
    }


    private void FixedUpdate()
    {
        playerScript?.OnFixedUpdate();
    }

}
