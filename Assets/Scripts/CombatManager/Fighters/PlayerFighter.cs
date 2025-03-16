using System.Collections;
using UnityEngine;

public class PlayerFighter : Fighter
{
    public SlowMotionController slowMotionController;
    public AudioSource death;

    [Header("UI")]
    public PlayerSkillPanel skillPanel;
    public EnemiesPanel enemiesPanel;
    

    private Skill skillToBeExecuted;
   

    void Awake()
    {
        this.stats = new Stats(100, 100, 25, 50, 40, 20); //NIVEL - VIDA - ATAQUE - DEFENSA - MAGIA - VELOCIDAD
    }

    protected override void Die()
    {
        StartCoroutine(GameOver());
        death.Play();
        isAlive = false;
        PlayAnimation(defeatAnimationNameTrigger);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.4f);
        slowMotionController.ActivateSlowMotion();
    }


    public override void InitTurn()
    {
        this.skillPanel.ShowForPlayer(this);

        for (int i = 0; i < this.skills.Length; i++)
        {
            this.skillPanel.ConfigureButton(i, this.skills[i].skillName);
        }
    }

    /// ================================================
    /// <summary>
    /// Se llama desde EnemiesPanel.
    /// </summary>
    /// <param name="index"></param>
    public void ExecuteSkill(int index)
    {
        this.skillToBeExecuted = this.skills[index];
        this.skillToBeExecuted.SetEmitter(this);

        // Deshabilitar todos los botones excepto el seleccionado
        this.skillPanel.DisableAllButtonsExcept(index);

        if (this.skillToBeExecuted.needsManualTargeting)
        {
            Fighter[] receivers = this.GetSkillTargets(this.skillToBeExecuted);
            this.enemiesPanel.Show(this, receivers);
        }
        else
        {
            this.AutoConfigureSkillTargeting(this.skillToBeExecuted);

            this.combatManager.OnFighterSkill(this.skillToBeExecuted);
            this.skillPanel.Hide();
        }
    }

    public void SetTargetAndAttack(Fighter enemyFigther)
    {
        this.skillToBeExecuted.AddReceiver(enemyFigther);

        this.combatManager.OnFighterSkill(this.skillToBeExecuted);

        this.skillPanel.Hide();
        this.enemiesPanel.Hide();

        // Habilitar todos los botones después de ejecutar la habilidad
        this.skillPanel.EnableAllButtons();
    }
}
