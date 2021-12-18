using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public List<Ability> abilityList;

    private void Update()
    {
        foreach (Ability ability in abilityList)
        {
            if (!ability.OnCooldown())
            {
                if (!ability.active && ability.GetKeyDown()) ability.active = true;
                if (ability.active)
                {
                    ability.Activate(gameObject);
                    ability.active = false;
                }
            }
        }
    }

}
