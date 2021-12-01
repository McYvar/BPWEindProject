using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    public int Scene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(Scene);
    }
}
