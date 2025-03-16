using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
    public Text nameLabel;
    public Text levelLabel;

    public Slider healthSlider;
    public Image healthSliderBar;
    public Text healthLabel;

    public void SetStats(string name, Stats stats)
    {
        nameLabel.text = name;
        levelLabel.text = "Lvl. " + stats.level;
        SetHealth(stats.health, stats.maxHealth);
    }

    /*public void SetHealth(float health, float maxHealth)
    {
        this.healthLabel.text = $"{Mathf.RoundToInt(health)} / {Mathf.RoundToInt(maxHealth)}";
        float percentage = health / maxHealth;

        this.healthSlider.value = percentage;

        if (percentage < 0.33f)
        {
            this.healthSliderBar.color = Color.red;
        }
    }*/

    public void SetHealth(float health, float maxHealth)
    {
        int roundedHealth = Mathf.RoundToInt(health);
        int roundedMaxHealth = Mathf.RoundToInt(maxHealth);

        healthSlider.value = (float)roundedHealth / roundedMaxHealth;
        healthLabel.text = roundedHealth + " / " + roundedMaxHealth;

        // Actualizar el color de la barra de salud
        if (healthSlider.value > 0.5f)
        {
            healthSliderBar.color = Color.green;
        }
        else if (healthSlider.value > 0.3f)
        {
            healthSliderBar.color = Color.yellow;
        }
        else
        {
            healthSliderBar.color = Color.red;
        }
    }
}
