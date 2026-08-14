using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

/// <summary>
/// Script in charge of Enemy Attacks.
/// </summary>
[RequireComponent (typeof(EnemyStats))]
public class EnemyAttack : UnitAttack
{
    
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Add the base stats for the enemy.
    /// </summary>
    /// <param name="enemyName"></param>
    /// <param name="statLevel"></param>
    public void SetEnemyMoveList(string enemyName, byte statLevel)
    {
        EnemyStats enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Stats!!");
            return;
        }
        attackList = EnemyMoveListIndex.GetEnemyMoveList(enemyName);
        hasAttackList = attackList.GetMoveList().Count > 0;
    }

    /// <summary>
    /// Determine what attack the enemy will do next.
    /// </summary>
    public virtual void EnemyDetermineNextAttack()
    {
        Debug.LogError("TODO!! Figure out AI to determine enemy AI on next attack to use");
    }
}
