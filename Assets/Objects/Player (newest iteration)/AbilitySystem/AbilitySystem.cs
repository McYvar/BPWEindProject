using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public List<Ability> abilityList;

    private void Update()
    {
        for (int i = 0; i < abilityList.Count; i++)
        {
            Ability ability = abilityList[i];
            if (!ability.OnCooldown())
            {
                if (!ability.active && ability.GetKeyDown(gameObject)) ability.active = true;
                if (ability.active)
                {
                    ability.Activate(gameObject);
                    ability.active = false;
                    if (!ability.reusable) abilityList.Remove(ability);
                }
            }
        }
    }

}
