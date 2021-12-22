using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    #region Variables and stuff
    public float bopHeight;
    public float bounceSpeed;
    public float rotationSpeed;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private float chanceDirectionTimer;

    public Ability ability;
    #endregion


    #region Start and Update
    private void Start()
    {
        // Give the ability animation a random start rotation (its not a real animation but still...)
        rotationX = Random.Range(-5, 5);
        rotationY = Random.Range(-5, 5);
        rotationZ = Random.Range(-5, 5);
        chanceDirectionTimer = 3;
    }


    private void Update()
    {
        // Animation for the object of the ability
        transform.Rotate(new Vector3(rotationX * rotationSpeed, rotationY * rotationSpeed, rotationZ * rotationSpeed));
        transform.position = transform.position + new Vector3(0, Mathf.Cos(Time.time * bounceSpeed) * bopHeight * Time.deltaTime, 0);

        if (chanceDirectionTimer <= 0)
        {
            // After some time chance the rotation direction of the animation
            rotationX = Random.Range(-5, 5);
            rotationY = Random.Range(-5, 5);
            rotationZ = Random.Range(-5, 5);
            chanceDirectionTimer = 3;
        }
        chanceDirectionTimer -= Time.deltaTime;
    }
    #endregion


    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<Ability> abilityList = FindObjectOfType<AbilitySystem>().abilityList;
            abilityList.Add(ability);
            Destroy(gameObject);
        }
    }
    #endregion

}
