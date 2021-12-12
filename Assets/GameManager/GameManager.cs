using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player playerScript;

    private void Awake()
    {
        Physics.gravity = Vector3.down * 9.81f;
        playerScript = GameManager.FindObjectOfType<Player>();

        playerScript?.OnAwake();
    }


    private void Start()
    {
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
