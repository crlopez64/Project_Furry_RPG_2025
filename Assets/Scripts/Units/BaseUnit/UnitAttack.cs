using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of Attacks and Move Lists.
/// </summary>
public class UnitAttack : MonoBehaviour
{
    private BattleManager battleManager;
    private Animator animator;
    protected UnitStats unitStats;
    protected MoveList attackList;
    protected MoveList skillsList;
    protected Vector3 originalPosition;

    public virtual void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        animator = GetComponent<Animator>();
        unitStats = GetComponent<UnitStats>();
    }

    /// <summary>
    /// Begin attack animation.
    /// </summary>
    public void AnimationBeginAttack()
    {
        Debug.LogWarning("Animator begin Attack!!");
        animator.SetTrigger("BeginAttackAnim");
        animator.SetInteger("AttackID", battleManager.GetBaseItemAnimationID());
    }

    /// <summary>
    /// Activate the next Action Command
    /// </summary>
    public void ActivateActionCommand()
    {
        Debug.Log("Animator activate next Action Command");
        battleManager.PrepareNextActionCommand();
    }

    /// <summary>
    /// Dequeue the next AttackStep and activate its appropriate Action Command.
    /// </summary>
    public void ActivateNextAttackStep()
    {
        Debug.Log("Animator activate next attack step");
        animator.SetTrigger("NextAttackStep");
    }

    public void ActivateReturnToOriginalPositionAfterAttack()
    {
        battleManager.ExecuteTurnMoveToReturnLocation();
    }

    /// <summary>
    /// Animate getting hit.
    /// </summary>
    public void ActivateGettingHit()
    {
        animator.SetTrigger("Hit0");
    }

    /// <summary>
    /// Return the original position this Unit was at prior to moving.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }

    /// <summary>
    /// Set this Unit's new original position to rest at.
    /// Chance to add difference between old Original Position and new Original Position.
    /// </summary>
    /// <param name="newOrigionalPosition"></param>
    public void SetNewOrigionalPosition(Vector3 newOrigionalPosition)
    {
        if (originalPosition == null)
        {
            originalPosition = newOrigionalPosition;
            return;
        }
        int getRandom = Random.Range(0, 10);
        if (getRandom < 4)
        {
            newOrigionalPosition = new Vector3(Random.Range(originalPosition.x, newOrigionalPosition.x),
                Random.Range(originalPosition.y, newOrigionalPosition.y),
                newOrigionalPosition.z);
        }
        originalPosition = newOrigionalPosition;
    }

    /// <summary>
    /// Get the current Attack List.
    /// </summary>
    /// <returns></returns>
    public List<Attack> GetCurrentAttackList()
    {
        return GetAttackList(attackList.GetMoveList());
    }

    /// <summary>
    /// Get the current Skills List.
    /// </summary>
    /// <returns></returns>
    public List<Attack> GetCurrentSkillsList()
    {
        return GetAttackList(skillsList.GetMoveList());
    }

    /// <summary>
    /// Make the list
    /// </summary>
    /// <param name="masterList"></param>
    /// <returns></returns>
    private List<Attack> GetAttackList(List<Attack> masterList)
    {
        List<Attack> list = new List<Attack>();
        foreach (Attack attack in masterList)
        {
            if (attack.UnlockedAttack(unitStats.GetStatLevel()))
            {
                list.Add(attack);
            }
        }
        return list;
    }
}
