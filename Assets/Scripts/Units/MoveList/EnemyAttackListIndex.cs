using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Index to correctly provide an Enemy a movelist in the Battle Scene.
/// </summary>
public class EnemyAttackListIndex
{
    /// <summary>
    /// Return an Enemy's Attack List to provide back to EnemyStats.
    /// </summary>
    /// <param name="enemyName"></param>
    /// <returns></returns>
    public static MoveList GetEnemyAttackList(string enemyName)
    {
        switch (enemyName)
        {
            case "Sample_Enemy":
                return new SampleEnemyAttack();
            default:
                Debug.LogError("ERROR: Could not find Attack List for: " + enemyName);
                return null;
        }
    }

}
