using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    #region Variables and stuff
    public float bopHeight;
    public float bounceSpeed;
    public float rotationSpeed;
    public bool respawnOnPickup;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private float changeDirectionTimer;

    public GameObject obj;
    private float respawnTimer = 0;

    public Ability ability;
    #endregion


    #region Start and Update
    private void Start()
    {
        // Give the ability animation a random start rotation (its not a real animation but still...)
        rotationX = Random.Range(-5, 5);
        rotationY = Random.Range(-5, 5);
        rotationZ = Random.Range(-5, 5);
        changeDirectionTimer = 3;
    }


    private void Update()
    {
        // Animation for the object of the ability
        transform.Rotate(new Vector3(rotationX * rotationSpeed, rotationY * rotationSpeed, rotationZ * rotationSpeed));
        transform.position = transform.position + new Vector3(0, Mathf.Cos(Time.time * bounceSpeed) * bopHeight * Time.deltaTime, 0);

        if (changeDirectionTimer <= 0)
        {
            // After some time chance the rotation direction of the animation
            rotationX = Random.Range(-5, 5);
            rotationY = Random.Range(-5, 5);
            rotationZ = Random.Range(-5, 5);
            changeDirectionTimer = 3;
        }
        changeDirectionTimer -= Time.deltaTime;

        if (respawnTimer <= 0)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
            respawnTimer -= Time.deltaTime;
        }
    }
    #endregion


    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<Ability> abilityList = other.gameObject.GetComponent<AbilitySystem>().abilityList;
            if (abilityList.Contains(ability)) return;
            ability.SetAbililtyCooldown(0);
            abilityList.Add(ability);
            if (!respawnOnPickup) Destroy(gameObject);
            else respawnTimer = 3;
        }
    }
    #endregion

}
