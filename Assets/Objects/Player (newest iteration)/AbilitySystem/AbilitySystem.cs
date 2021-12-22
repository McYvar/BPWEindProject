using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilitySystem : MonoBehaviour
{
    public TMP_Text text;

    private string startText;

    // Creating a List to loop over the abilities added to this list
    public List<Ability> abilityList;

    private void Start()
    {
        startText = "Abilities: ";
    }

    private void Update()
    {
        text.text = startText;
        for (int i = 0; i < abilityList.Count; i++)
        {
            Ability ability = abilityList[i];
            if (ability == null) break;
            ability.abilityDisplayName = ability.abilityName + " (" + ability.GetActiveKey() + ")";
            text.text += "\n" + ability.abilityDisplayName;
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
        
        if (abilityList.Count == 0)
        {
            text.text = "Abilities: None";
        }
    }

}
