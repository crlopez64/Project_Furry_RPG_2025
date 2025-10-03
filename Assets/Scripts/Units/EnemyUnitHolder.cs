using System;
using UnityEditor.Animations;
using UnityEngine;

/// <summary>
/// Script in charge of being an Enemy Placeholder at battle.
/// </summary>
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(UnitMove))]
public class EnemyUnitHolder : MonoBehaviour
{
    //private RuntimeAnimatorController controller;
    private EnemyStats enemyStats;

    private void Awake()
    {
        //controller = GetComponentInParent<RuntimeAnimatorController>();
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Stats!!");
        }
    }
    
    /// <summary>
    /// Create the Enemy and their stats.
    /// </summary>
    public void CreateEnemy(string enemyName, string enemyCode)
    {
        // On Component Spawn, that will create the move list for that Enemy
        enemyStats.SetUnitName(enemyName);
        string attackComponentName = new string("Enemy" +  enemyCode + "Attack");
        Component getComponent = gameObject.AddComponent(Type.GetType(attackComponentName));
        if (getComponent == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Attack of type: " + attackComponentName +
                "\nLocate in /Scripts/Unit/Enemy/EnemyAttackSpecific/");
        }
    }
}
