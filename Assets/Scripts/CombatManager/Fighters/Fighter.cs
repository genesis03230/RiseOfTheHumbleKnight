using UnityEngine;
using System.Collections.Generic;

public abstract class Fighter : MonoBehaviour
{
    public Team team;

    public string idName;
    public StatusPanel statusPanel;

    public CombatManager combatManager;

    public List<StatusMod> statusMods;

    protected Stats stats;

    protected Skill[] skills;

    public StatusCondition statusCondition;

    public bool isAlive { get; set; } = true;


    [Header("Animations")]
    public string defeatAnimationNameTrigger; // Nombre de la animación de derrota
    public string victoryAnimationName; // Nombre de la animación de victoria

    protected virtual void Start()
    {
        // Inicializar el panel de estado con las estadísticas actuales
        statusPanel.SetStats(idName, stats);

        this.skills = this.GetComponentsInChildren<Skill>();

        this.statusMods = new List<StatusMod>();
    }

    protected void AutoConfigureSkillTargeting(Skill skill)
    {
        skill.SetEmitter(this);

        switch (skill.targeting)
        {
            case SkillTargeting.AUTO:
                skill.AddReceiver(this);
                break;
            case SkillTargeting.ALL_ALLIES:
                Fighter[] allies = this.combatManager.GetAllyTeam();
                foreach (var receiver in allies)
                {
                    skill.AddReceiver(receiver);
                }

                break;
            case SkillTargeting.ALL_OPPONENTS:
                Fighter[] enemies = this.combatManager.GetOpposingTeam();
                foreach (var receiver in enemies)
                {
                    skill.AddReceiver(receiver);
                }
                break;

            case SkillTargeting.SINGLE_ALLY:
            case SkillTargeting.SINGLE_OPPONENT:
                throw new System.InvalidOperationException("Unimplemented! This skill needs manual targeting.");
        }
    }

    protected Fighter[] GetSkillTargets(Skill skill)
    {
        switch (skill.targeting)
        {
            case SkillTargeting.AUTO:
            case SkillTargeting.ALL_ALLIES:
            case SkillTargeting.ALL_OPPONENTS:
                throw new System.InvalidOperationException("Unimplemented! This skill doesn't need manual targeting.");

            case SkillTargeting.SINGLE_ALLY:
                return this.combatManager.GetAllyTeam();
            case SkillTargeting.SINGLE_OPPONENT:
                return this.combatManager.GetOpposingTeam();
        }

        // Esto no debería ejecutarse nunca pero hay que ponerlo para hacer al compilador feliz.
        throw new System.InvalidOperationException("Fighter::GetSkillTargets. Unreachable!");
    }

    /*{
        this.statusPanel.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }*/

    public void ModifyHealth(float amount)
    {
        stats.health = Mathf.Clamp(stats.health + amount, 0, stats.maxHealth);
        statusPanel.SetHealth(stats.health, stats.maxHealth);

        if (stats.health <= 0)
        {
            Die();
        }
    }



    public Stats GetCurrentStats()
    {
        return stats;
        /*Stats modedStats = this.stats;

        foreach (var mod in this.statusMods)
        {
            modedStats = mod.Apply(modedStats);
        }

        return modedStats;*/
    }

    public StatusCondition GetCurrentStatusCondition()
    {
        return statusCondition;    
        /*if (this.statusCondition != null && this.statusCondition.hasExpired)
        {
            Destroy(this.statusCondition.gameObject);
            this.statusCondition = null;
        }

        return this.statusCondition;*/
    }

    protected abstract void Die();


    public void PlayAnimation(string animationName)
    {
        Animator animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(animationName);
        }
        else
        {
            Debug.LogWarning("No se encontró el componente Animator."); // Mensaje de advertencia
        }
    }

    public abstract void InitTurn();
}
