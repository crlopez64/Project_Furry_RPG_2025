using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Enemy's stats.
/// </summary>
public class EnemyStats : UnitStats
{
    private int baseExperienceReward;

    public int setExperienceReward;

    private void Awake()
    {
        //TODO: Get final baseExperience based on the Enemy's level and other factors.
        baseExperienceReward = setExperienceReward;
        statusAilments = new List<StatusAilment>();
    }

    /// <summary>
    /// Return the amount of Experience this enemy gives upon defeat.
    /// </summary>
    /// <returns></returns>
    public int GetExperienceReward()
    {
        return baseExperienceReward;
    }
}
