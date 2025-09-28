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
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        //controller = GetComponentInParent<RuntimeAnimatorController>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAttack = GetComponent<EnemyAttack>();
        if (enemyStats == null)
        {
            Debug.LogError("ERROR: Could not find Enemy Stats!!");
        }
    }
    
    /// <summary>
    /// Create the Enemy and their stats.
    /// </summary>
    /// <param name="nameOfEnemyToCreate"></param>
    public void CreateEnemy(string nameOfEnemyToCreate)
    {
        Debug.Log("Update animator and enemy stats to enemy");
        enemyAttack.AddEnemyMoveList(nameOfEnemyToCreate);
    }
}
