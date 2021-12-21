using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public float bopHeight;
    public float bounceSpeed;
    public float rotationSpeed;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private float chanceDirectionTimer;

    public Ability ability;

    private void Start()
    {
        rotationX = Random.Range(-5, 5);
        rotationY = Random.Range(-5, 5);
        rotationZ = Random.Range(-5, 5);
        chanceDirectionTimer = 3;
    }


    private void Update()
    {
        transform.Rotate(new Vector3(rotationX * rotationSpeed, rotationY * rotationSpeed, rotationZ * rotationSpeed));
        transform.position = transform.position + new Vector3(0, Mathf.Cos(Time.time * bounceSpeed) * bopHeight * Time.deltaTime, 0);

        if (chanceDirectionTimer <= 0)
        {
            rotationX = Random.Range(-5, 5);
            rotationY = Random.Range(-5, 5);
            rotationZ = Random.Range(-5, 5);
            chanceDirectionTimer = 3;
        }
        chanceDirectionTimer -= Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<Ability> abilityList = FindObjectOfType<AbilitySystem>().abilityList;
            abilityList.Add(ability);
            Destroy(gameObject);
        }
    }

}
