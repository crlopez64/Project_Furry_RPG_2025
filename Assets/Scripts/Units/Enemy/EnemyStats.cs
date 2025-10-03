using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Enemy's stats.
/// </summary>
public class EnemyStats : UnitStats
{
    private int experienceReward;

    public int setExperienceReward;

    private void Awake()
    {
        experienceReward = setExperienceReward;
        statusAilments = new List<StatusAilment>();
    }
    private void Start()
    {
        statusAilments = new List<StatusAilment>();
    }

    /// <summary>
    /// Return the amount of Experience this enemy gives upon defeat.
    /// </summary>
    /// <returns></returns>
    public int GetExperienceReward()
    {
        return experienceReward;
    }
}
