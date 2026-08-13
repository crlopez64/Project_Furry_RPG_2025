using UnityEngine;

/// <summary>
/// Script in charge of Enemy Attacks.
/// </summary>
public class EnemyAttack : UnitAttack
{

    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Retrieve the appropriate Attack List.
    /// </summary>
    public virtual void AddEnemyMoveList(string enemyName)
    {
        attackList = EnemyMoveListIndex.GetEnemyMoveList(enemyName);
    }

    /// <summary>
    /// Add the base stats for the enemy.
    /// </summary>
    /// <param name="enemyName"></param>
    /// <param name="statLevel"></param>
    public void AddEnemyBaseStats(string enemyName, byte statLevel)
    {
        EnemyStats enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Stats!!");
            return;
        }
        enemyStats.SetUnitName(enemyName);
        enemyStats.SetBaseStats(statLevel, EnemyBaseStatsIndex.GetEnemyBaseStats(enemyName));
    }

    /// <summary>
    /// Determine what attack the enemy will do next.
    /// </summary>
    public virtual void EnemyDetermineNextAttack()
    {
        Debug.LogError("TODO!! Figure out AI to determine enemy AI on next attack to use");
    }
}
