using UnityEngine;

public class EnemyFile : EnemyAttack
{
    public override void AddEnemyMoveList()
    {
        if (attackList != null)
        {
            return;
        }
        attackList = EnemyAttackListIndex.GetEnemyAttackList("Sample_Enemy");
    }

    /// <summary>
    /// Determine what attack the enemy will do next.
    /// </summary>
    public override void EnemyDetermineNextAttack()
    {
        Debug.LogError("TODO!! Figure out AI to determine enemy AI on next attack to use");
        //Get Attack Anim

    }


}
