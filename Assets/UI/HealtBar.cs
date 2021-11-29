using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{

    public Slider playerHealth;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        playerHealth.maxValue = health;
        playerHealth.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth(int health)
    {
        playerHealth.value = health;

        fill.color = gradient.Evaluate(playerHealth.normalizedValue);
    }


    public int getHealth()
    {
        return (int) playerHealth.value;
    }

}
