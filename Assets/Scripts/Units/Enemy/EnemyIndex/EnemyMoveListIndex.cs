using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Index to correctly provide an Enemy a movelist in the Battle Scene.
/// </summary>
public class EnemyMoveListIndex
{
    /// <summary>
    /// Return an Enemy's Move List to provide back to EnemyStats.
    /// </summary>
    /// <param name="enemyName"></param>
    /// <returns></returns>
    public static MoveList GetEnemyMoveList(string enemyName)
    {
        switch (enemyName)
        {
            case "Sample Enemy":
                return new SampleEnemyMoveList();
            default:
                Debug.LogError("ERROR: Could not find Move List for: " + enemyName);
                return null;
        }
    }

}
