using System;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Script in charge of being an Enemy Placeholder at battle.
/// </summary>
[RequireComponent(typeof(UnitMove))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyUnitHolder : MonoBehaviour
{
    //private RuntimeAnimatorController controller;
    private EnemyStats enemyStats;
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        //controller = GetComponentInParent<RuntimeAnimatorController>();
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Stats!!");
        }
        enemyAttack = GetComponent<EnemyAttack>();
        if (enemyAttack == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Attack!!");
        }
    }
    
    /// <summary>
    /// Create the Enemy and their stats.
    /// </summary>
    public void CreateEnemy(string enemyName)
    {
        Debug.Log("Creating Enemy: " + enemyName);
        enemyStats.SetUnitName(enemyName);
        enemyStats.SetBaseStatsAndFrontEndStats(32, EnemyBaseStatsIndex.GetEnemyBaseStats(enemyName));
        enemyStats.HealthAndManaFullRestore();
        enemyAttack.SetEnemyMoveList(enemyName, 32);
    }
}
