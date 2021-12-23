using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public string abilityName;
    public KeyCode key;
    public float cooldown;
    public bool reusable;

    private string abilityDisplayName;
    private float abilityCooldown;
    private bool active;

    public virtual bool GetKeyDown(GameObject obj)
    {
        // Based on the in "Inspector" given key return a bool if this key is pressed
        if (Input.GetKeyDown(key)) return true;
        else return false;
    }

    public virtual void Activate(GameObject obj) 
    {
        abilityCooldown = cooldown;
    }

    public KeyCode GetActiveKey()
    {
        return key;
    }


    public bool OnCooldown()
    {
        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
            return true;
        }
        else
        {
            return false;
        }
    }


    public float GetAbilityCooldown()
    {
        return abilityCooldown;
    }


    public void SetAbililtyCooldown(float cooldown)
    {
        abilityCooldown = cooldown;
    }


    public void SetAbilityDisplayName(string name)
    {
        abilityDisplayName = name;
    }


    public string GetAbilityDisplayName()
    {
        return abilityDisplayName;
    }


    public bool GetActive()
    {
        return active;
    }


    public void SetActive(bool condition)
    {
        active = condition;
    }

}
