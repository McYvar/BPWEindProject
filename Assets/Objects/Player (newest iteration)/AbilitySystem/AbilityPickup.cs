using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability ability;

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
