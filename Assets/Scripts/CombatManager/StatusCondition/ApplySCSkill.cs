using UnityEngine;

/// <summary>
/// Apply Status Condition Skill
/// </summary>
public class ApplySCSkill : Skill
{
    private StatusCondition condition;

    protected override void OnRun(Fighter receiver)
    {
        if (this.condition == null)
        {
            this.condition = this.GetComponentInChildren<StatusCondition>();

            if (this.condition.gameObject == this.gameObject)
            {
                throw new System.InvalidOperationException(
                    "The StatusCondition should be a child of the skill object because it needs to be cloned"
                );
            }
        }

        if (receiver.GetCurrentStatusCondition())
        {
            //this.messages.Enqueue("Fallo!");
            return;
        }

        // Clonamos la status condition
        GameObject go = Instantiate(this.condition.gameObject);
        go.transform.SetParent(receiver.transform);

        // Asignamos el cambio de estado al receptor
        StatusCondition clonedCondition = go.GetComponent<StatusCondition>();
        clonedCondition.SetReceiver(receiver);
        receiver.statusCondition = clonedCondition;

        // Verificar si el receptor está derrotado
        if (!receiver.isAlive)
        {
            this.messages.Enqueue($"{receiver.idName} ha sido derrotado!");
            return;
        }

        //this.messages.Enqueue(clonedCondition.GetReceptionMessage()); Omitir mensajes para combate agil
    }
}