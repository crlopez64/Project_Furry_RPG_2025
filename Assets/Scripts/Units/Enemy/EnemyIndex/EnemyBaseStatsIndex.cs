using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Index to correctly provide an Enemy a base stats in the Battle Scene.
/// </summary>
public class EnemyBaseStatsIndex
{
    /// <summary>
    /// Return a list of an Enemy's Base Stats to provide back to EnemyStats.
    /// In Order: HP, MANA, ATK, DEF, SP.ATK, SP.DEF, SPD
    /// </summary>
    /// <param name="enemyName"></param>
    /// <returns></returns>
    public static List<int> GetEnemyBaseStats(string enemyName)
    {
        switch (enemyName)
        {
            case "Sample Enemy":
                return new List<int>(){ 100, 20, 45, 37, 30, 31, 25 };
            default:
                Debug.LogError("ERROR: Could not find Base Stats for: " + enemyName);
                return null;
        }
    }
}
