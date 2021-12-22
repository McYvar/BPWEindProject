using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tiny class that loads a scene when entering the triggerspace
public class NextSceneTrigger : MonoBehaviour
{
    public int Scene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(Scene);
    }
}
