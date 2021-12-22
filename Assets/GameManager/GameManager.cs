using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created a game manager because every scene has to start with certain conditions
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // Game is gravity based at a certain point so always at loading a level reset gravity to normal
        Physics.gravity = Vector3.down * 20f;
    }


    private void Start()
    {
        // Lock and hide the cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
