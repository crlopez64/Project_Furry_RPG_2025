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
    /// <param name="enemyName"></param>
    public void AddEnemyMoveList(string enemyName)
    {
        if (attackList != null)
        {
            return;
        }
        attackList = EnemyAttackListIndex.GetEnemyAttackList(enemyName);
    }
}
