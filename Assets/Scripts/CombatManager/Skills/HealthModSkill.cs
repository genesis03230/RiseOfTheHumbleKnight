using UnityEngine;

public enum HealthModType
{
    STAT_BASED, FIXED, PERCENTAGE
}

public class HealthModSkill : Skill
{
    public FloatingText floatingText;

    [Header("Health Mod")]
    public float amount;

    public HealthModType modType;

    [Range(0f, 1f)]
    public float critChance = 0;

    [Header("Miss Chance")]
    [Range(0f, 1f)]
    public float missChance = 0; // Nueva variable para controlar la probabilidad de fallo


    protected override void OnRun(Fighter receiver)
    {
        // Animar al emisor de la habilidad
        //this.Animate(this.emitter, this.emitterAnimationName, false);

        float dice = Random.Range(0f, 1f);

        // Verificar si el golpe falla
        if (dice <= this.missChance)
        {
            floatingText.ActivateObject(false);
            this.messages.Enqueue("MISS");
            return;
        }

        float amount = this.GetModification(receiver);

        // Verificar si el golpe es critico
        if (dice <= this.critChance)
        {
            floatingText.ActivateObject(true);
            amount *= 2f;
            this.messages.Enqueue("Golpe CRITICO!");
        }

        receiver.ModifyHealth(amount);

        // Verificar si el receptor está derrotado
        if (!receiver.isAlive)
        {
            this.messages.Enqueue($"{receiver.idName} ha sido derrotado!");
            return;
        }

        // Animar al receptor de la habilidad
       // this.Animate(receiver, this.receiverAnimationName, true);
    }

    public float GetModification(Fighter receiver)
    {
        switch (this.modType)
        {
            case HealthModType.STAT_BASED:
                Stats emitterStats = this.emitter.GetCurrentStats();
                Stats receiverStats = receiver.GetCurrentStats();

                // Fórmula: https://bulbapedia.bulbagarden.net/wiki/Damage
                float rawDamage = (((2 * emitterStats.level) / 5) + 2) * this.amount * (emitterStats.attack / receiverStats.deffense);

                return (rawDamage / 50) + 2;
            case HealthModType.FIXED:
                return this.amount;
            case HealthModType.PERCENTAGE:
                Stats rStats = receiver.GetCurrentStats();

                return rStats.maxHealth * this.amount;
        }

        throw new System.InvalidOperationException("HealthModSkill::GetDamage. Unreachable!");
    }
}