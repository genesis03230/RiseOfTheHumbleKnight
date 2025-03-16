using UnityEngine;
using System.Collections;

public class EnemyFighter : Fighter
{
    public NpcController npcController; // Referencia al NPCController
    public AudioSource death;

    void Awake()
    {
        this.stats = new Stats(200, 200, 30, 40, 60, 10); //NIVEL - VIDA - ATAQUE - DEFENSA - MAGIA - VELOCIDAD
    }

    protected override void Die()
    {
        death.PlayDelayed(0.5f);
        isAlive = false;
        PlayAnimation(defeatAnimationNameTrigger);
    }


    public override void InitTurn()
    {
        StartCoroutine(this.IA());
    }

    IEnumerator IA()
    {
        yield return new WaitForSeconds(1f);

        Skill skill = this.skills[Random.Range(0, this.skills.Length)];
        skill.SetEmitter(this);

        if (skill.needsManualTargeting)
        {
            Fighter[] targets = this.GetSkillTargets(skill);

            Fighter target = targets[Random.Range(0, targets.Length)];

            skill.AddReceiver(target);
        }
        else
        {
            this.AutoConfigureSkillTargeting(skill);
        }

        this.combatManager.OnFighterSkill(skill);
    }
}
