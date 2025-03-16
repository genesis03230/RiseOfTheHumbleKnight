using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillPanel : MonoBehaviour
{
    public GameObject[] skillButtons;
    public Text[] skillButtonLabels;

    private int[] skillUsageCount; // Usos de cada habilidad
    private const int maxHealUses = 3; // Máximo de usos de habilidad de curación

    private PlayerFighter targetFigther;

    void Awake()
    {
        this.Hide();
        skillUsageCount = new int[skillButtons.Length]; // Inicializamos el array de usos de habilidades
    }

    public void ConfigureButton(int index, string skillName)
    {
        this.skillButtons[index].SetActive(true);
        this.skillButtonLabels[index].text = skillName;
    }

    public void OnSkillButtonClick(int index)
    {
        if (skillButtonLabels[index].text == "Curacion") // Si la habilidad es de curación
        {
            if (skillUsageCount[index] < maxHealUses)
            {
                skillUsageCount[index]++;
                this.DisableButton(index, false); // Deshabilitar el botón de curación inmediatamente
                this.targetFigther.ExecuteSkill(index);

                if (skillUsageCount[index] >= maxHealUses)
                {
                    DisableButton(index, true);
                }
            }
        }
        else
        {
            this.targetFigther.ExecuteSkill(index);
        }
    }

    private void DisableButton(int index, bool isPermanent) // Deshabilita el botón de la habilidad
    {
        Button button = skillButtons[index].GetComponent<Button>();
        button.interactable = false;
        ColorBlock colors = button.colors;
        colors.disabledColor = isPermanent ? Color.red : new Color(0.784f, 0.784f, 0.784f, 0.5f); // Cambia esto al color que prefieras
        button.colors = colors;
    }

    public void ShowForPlayer(PlayerFighter newTarget)
    {
        this.gameObject.SetActive(true);

        this.targetFigther = newTarget;

        // Verificar el contador de uso de la habilidad de curación y deshabilitar el botón si es necesario
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (skillButtonLabels[i].text == "Curacion" && skillUsageCount[i] >= maxHealUses)
            {
                DisableButton(i, true); // Deshabilitar el botón de curación permanentemente
            }
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);

        foreach (var btn in this.skillButtons)
        {
            btn.SetActive(false);
        }
    }


    //SE AGREGA CODIGO NUEVO


    public void DisableAllButtonsExcept(int index)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i != index)
            {
                skillButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void EnableAllButtons()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            // Verificar el contador de uso de la habilidad de curación y deshabilitar el botón si es necesario
            if (skillButtonLabels[i].text == "Curacion" && skillUsageCount[i] >= maxHealUses)
            {
                DisableButton(i, true); // Deshabilitar el botón de curación permanentemente
            }
            else
            {
                skillButtons[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void EnableButton(int index) // Habilita el botón de la habilidad
    {
        Button button = skillButtons[index].GetComponent<Button>();
        button.interactable = true;
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // Cambia esto al color que prefieras
        button.colors = colors;
    }

}
