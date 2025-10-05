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
        statMaxMana = heroStats.GetMaxMana();
        statMaxHealth = heroStats.GetMaxHealth();
        statAttackPhysical = heroStats.GetAttackPhysicalLevel();
        statAttackSpecial = heroStats.GetAttackSpecialLevel();
        statDefensePhysical = heroStats.GetDefensePhysicalLevel();
        statDefenseSpecial = heroStats.GetDefenseSpecialLevel();
        statSpeed = heroStats.GetStatSpeed();
        statLuck = heroStats.GetStatLuck();
        statusAilments = heroStats.GetStatusAilments();
    }

    /// <summary>
    /// Constructor manually inputting values.
    /// </summary>
    public HeroStatsStorage(string unitName, byte statLevel, int currentHealth, int currentMana,
        int statMaxHealth, int statMaxMana, int statAttackPhysical, int statDefensePhysical,
        int statAttackSpecial, int statDefenseSpecial, int statSpeed, int statLuck, List<StatusAilment> statusAilments)
    {
        this.unitName = unitName;
        this.statLevel = statLevel;
        this.currentHealth = currentHealth;
        this.currentMana = currentMana;
        this.statMaxHealth = statMaxHealth;
        this.statMaxMana = statMaxMana;
        this.statAttackPhysical = statAttackPhysical;
        this.statAttackSpecial = statAttackSpecial;
        this.statDefensePhysical = statDefensePhysical;
        this.statDefenseSpecial = statDefenseSpecial;
        this.statSpeed = statSpeed;
        this.statLuck = statLuck;
        this.statusAilments = statusAilments;
    }

    public string GetUnitName()
    {
        return unitName;
    }
    public byte GetStatLevel()
    {
        return statLevel;
    }

    public int GetStatCurrentHealth()
    {
        return currentHealth;
    }

    public int GetStatCurrentMana()
    {
        return currentMana;
    }

    public int GetStatMaxHealth()
    {
        return statMaxHealth;
    }

    public int GetStatMaxMana()
    {
        return statMaxMana;
    }

    public int GetStatAttackPhysical()
    {
        return statAttackPhysical;
    }

    public int GetStatAttackSpecial()
    {
        return statAttackSpecial;
    }
    public int GetStatDefensePhysical()
    {
        return statDefensePhysical;
    }
    public int GetStatDefenseSpecial()
    {
        return statDefenseSpecial;
    }
    
    public int GetStatSpeed()
    {
        return statSpeed;
    }

    public int GetStatLuck()
    {
        return statLuck;
    }
}
