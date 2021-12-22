using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public bool reusable;
    public bool active;
    public float cooldown;

    public string abilityName;
    public string abilityDisplayName;

    private float abilityCooldown;

    public virtual KeyCode GetActiveKey() { return KeyCode.None; }

    public virtual bool GetKeyDown(GameObject obj) { return false; }


    public virtual void Activate(GameObject obj) 
    {
        abilityCooldown = cooldown;
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

}
