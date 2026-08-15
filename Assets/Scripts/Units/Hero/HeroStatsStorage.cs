using System.Collections.Generic;
using static UnitStats;

/// <summary>
/// Script in charge of holding Hero Stats when moving one scene to another.
/// </summary>
public class HeroStatsStorage
{
    protected List<StatusAilment> statusAilments;
    protected string unitName;
    protected byte statLevel = 1;
    protected int currentHealth = 0;
    protected int currentMana = 0;
    protected int statMaxHealth = 0;
    protected int statMaxMana = 0;
    protected int statAttackPhysical = 0;
    protected int statDefensePhysical = 0;
    protected int statAttackSpecial = 0;
    protected int statDefenseSpecial = 0;
    protected int statSpeed = 0;
    protected int statLuck = 0;
    protected int baseMaxHealth = 0;
    protected int baseMaxMana = 0;
    protected int baseStatAttackPhysical = 0;
    protected int baseStatDefensePhysical = 0;
    protected int baseStatAttackSpecial = 0;
    protected int baseStatDefenseSpecial = 0;
    protected int baseStatSpeed = 0;

    /// <summary>
    /// Constructor via HeroStats.
    /// </summary>
    /// <param name="heroStats"></param>
    public HeroStatsStorage(HeroStats heroStats)
    {
        unitName = heroStats.GetUnitName();
        statLevel = heroStats.GetStatLevel();
        currentMana = heroStats.GetCurrentMana();
        currentHealth = heroStats.GetCurrentHealth();
        // Final Stats
        statMaxMana = heroStats.GetMaxMana();
        statMaxHealth = heroStats.GetMaxHealth();
        statAttackPhysical = heroStats.GetStatValue(StatType.ATTACK_PHYSICAL);
        statAttackSpecial = heroStats.GetStatValue(StatType.ATTACK_SPECIAL);
        statDefensePhysical = heroStats.GetStatValue(StatType.DEFENSE_PHYSICAL);
        statDefenseSpecial = heroStats.GetStatValue(StatType.DEFENSE_SPECIAL);
        statSpeed = heroStats.GetStatValue(StatType.SPEED);
        statLuck = heroStats.GetStatValue(StatType.LUCK);
        // Base Stats
        baseMaxHealth = heroStats.GetBaseStatValue(StatType.MAX_HEALTH);
        baseMaxMana = heroStats.GetBaseStatValue(StatType.MAX_MANA);
        baseStatAttackPhysical = heroStats.GetBaseStatValue(StatType.ATTACK_PHYSICAL);
        baseStatDefensePhysical = heroStats.GetBaseStatValue(StatType.DEFENSE_PHYSICAL);
        baseStatAttackSpecial = heroStats.GetBaseStatValue(StatType.ATTACK_SPECIAL);
        baseStatDefenseSpecial = heroStats.GetBaseStatValue(StatType.DEFENSE_SPECIAL);
        baseStatSpeed = heroStats.GetBaseStatValue(StatType.SPEED);
        // Status Ailments
        statusAilments = heroStats.GetStatusAilments();
    }

    /// <summary>
    /// Return Unit's name.
    /// </summary>
    /// <returns></returns>
    public string GetUnitName()
    {
        return unitName;
    }
    
    /// <summary>
    /// Return Unit's level.
    /// </summary>
    /// <returns></returns>
    public byte GetStatLevel()
    {
        return statLevel;
    }

    /// <summary>
    /// Return the raw current health value.
    /// </summary>
    /// <returns></returns>
    public int GetStatCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Return the raw current mana value.
    /// </summary>
    /// <returns></returns>
    public int GetStatCurrentMana()
    {
        return currentMana;
    }

    /// <summary>
    /// Return base stats in order: Max HP, Max Mana, ATK, DEF, SP.ATK, SP.DEF, SPD
    /// </summary>
    /// <returns></returns>
    public int[] GetBaseStats()
    {
        return new int[] { baseMaxHealth, baseMaxMana, baseStatAttackPhysical, baseStatDefensePhysical, baseStatAttackSpecial, baseStatDefenseSpecial, baseStatSpeed };
    }

    /// <summary>
    /// Return front end stats in order: Max HP, Max Mana, ATK, DEF, SP.ATK, SP.DEF, SPD
    /// </summary>
    /// <returns></returns>
    public int[] GetFrontEndStats()
    {
        return new int[] { statMaxHealth, statMaxMana, statAttackPhysical, statDefensePhysical, statAttackSpecial, statDefenseSpecial, statSpeed };
    }
}
