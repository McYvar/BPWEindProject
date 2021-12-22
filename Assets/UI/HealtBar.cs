using UnityEngine;
using UnityEngine.UI;

// HEALTBAR MOSTLY BASED ON A VIDEO MADE BY A YOUTUBER NAMED BRACKEYS
public class HealtBar : MonoBehaviour
{
    public Slider playerHealth;
    public Gradient gradient;
    public Image fill;

    // Subroutine for setting the maximum health
    public void SetMaxHealth(int health)
    {
        playerHealth.maxValue = health;
        playerHealth.value = health;

        fill.color = gradient.Evaluate(1f);
    }


    // Subroutine to set the health to a given amount
    public void setHealth(int health)
    {
        playerHealth.value = health;

        fill.color = gradient.Evaluate(playerHealth.normalizedValue);
    }


    // Subroutine to get the health
    public int getHealth()
    {
        return (int) playerHealth.value;
    }

}
