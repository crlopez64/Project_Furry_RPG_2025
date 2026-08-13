using System;
using System.Collections.Generic;
using UnityEngine;
using static BaseItem;

/// <summary>
/// A Unit's stats if they can be fought.
/// </summary>
public class UnitStats : MonoBehaviour
{
    protected List<StatusAilment> statusAilments;
    protected string unitName;
    protected byte attackPhysicalLevel = 0;
    protected byte defensePhysicalLevel = 0;
    protected byte attackSpecialLevel = 0;
    protected byte defenseSpecialLevel = 0;
    protected byte luckLevel = 0;
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
    protected int baseStatMaxHealth = 0;
    protected int baseStatMaxMana = 0;
    protected int baseStatAttackPhysical = 0;
    protected int baseStatDefensePhysical = 0;
    protected int baseStatAttackSpecial = 0;
    protected int baseStatDefenseSpecial = 0;
    protected int baseStatSpeed = 0;
    
    /// <summary>
    /// For referencing Stats if needed.
    /// </summary>
    public enum StatType
    {
        MAX_HEALTH,
        MAX_MANA,
        ATTACK_PHYSICAL,
        DEFENSE_PHYSICAL,
        ATTACK_SPECIAL,
        DEFENSE_SPECIAL,
        SPEED,
        LUCK
    }

    /// <summary>
    /// Location of an Item can be equipped to.
    /// </summary>
    public enum EquipSlots : byte
    {
        HEAD,
        TORSO,
        ARMS,
        LOWER,
        UNIQUE_1,
        UNIQUE_2
    }

    /// <summary>
    /// The type of a Unit or Attack.
    /// </summary>
    public enum UnitType : byte
    {
        NORMAL,
        ICE,
        FIRE,
        ELECTRIC
    }
    
    /// <summary>
    /// An effect that can harm or support a Unit.
    /// </summary>
    public enum StatusAilment : byte
    {
        /// <summary>
        /// For purposes of finding if Unit has no Ailments on them.
        /// </summary>
        NONE,
        /// <summary>
        /// Restore small HP at the start of every turn. Reduce POISONED and BURNED damage.
        /// Upon receiving EXHAUSTED, cancel that Ailment and reduce HEARTY by 1.
        /// </summary>
        HEARTY,
        /// <summary>
        /// Skip that Player's turn. Cancelled if Unit is put ASLEEP.
        /// </summary>
        STUNNED,
        /// <summary>
        /// Chance of unit not being able to go that turn.
        /// </summary>
        PARALYZED,
        /// <summary>
        /// Intake small Damage at end of every turn.
        /// </summary>
        POISONED,
        /// <summary>
        /// Intake small Damage at start of every turn. Reduce Attack Level by 1.
        /// </summary>
        BURNED,
        /// <summary>
        /// Skip the Player's turn; restore small HP, add POISONED damage, or reduce BURNED damage on skipped turn.
        /// </summary>
        ASLEEP,
        /// <summary>
        /// Chance of unit attacking 1 other unit indiscriminately for their turn. Cancelled if Unit is put ASLEEP.
        /// </summary>
        CONFUSED,
        /// <summary>
        /// Unit will automatically attack the Unit that enranged them (does not use Skills). Cancelled if Unit is put ASLEEP.
        /// </summary>
        ENRAGED,
        /// <summary>
        /// Unit's Skills are disabled.
        /// </summary>
        FRIGHTENED,
        /// <summary>
        /// Speeds up Damage Roll going down and slows down Damage Roll going up. Cancelled if Unit is put HEARTY.
        /// </summary>
        EXHAUSTED
    }

    /// <summary>
    /// Set this Unit's name.
    /// </summary>
    /// <param name="unitName"></param>
    public void SetUnitName(string unitName)
    {
        if (unitName != null)
        {
            return;
        }
        this.unitName = unitName;
    }

    /// <summary>
    /// Add all Status Ailments from an Item to this Unit. Skips any Status Ailments that are already on this Unit.
    /// Return false if no Status Ailments were added, true if at least 1 Status Ailment was added.
    /// </summary>
    /// <param name="item"></param>
    public bool AddStatusAilments(Item item)
    {
        bool usedForSomething = false;
        foreach (StatusAilment statusAilment in item.GetStatusAilments())
        {
            if (statusAilment == StatusAilment.NONE)
            {
                continue;
            }
            if (statusAilments.Contains(statusAilment))
            {
                continue;
            }
            statusAilments.Add(statusAilment);
            usedForSomething = true;
        }
        return usedForSomething;
    }

    /// <summary>
    /// Remove all Status Ailments from an Item to this Unit. Skips any Status Ailments that are not on this Unit.
    /// Return false if no Status Ailments were removed, true if at least 1 Status Ailment was removed.
    /// </summary>
    /// <param name="item"></param>
    public bool RemoveStatusAilments(Item item)
    {
        bool usedForSomething = false;
        foreach (StatusAilment statusAilment in item.GetStatusAilmentsRecovery())
        {
            if (statusAilment == StatusAilment.NONE)
            {
                continue;
            }
            if (!statusAilments.Contains(statusAilment))
            {
                continue;
            }
            statusAilments.Remove(statusAilment);
            usedForSomething = true;
        }
        return usedForSomething;
    }

    /// <summary>
    /// Set this Unit's stats.
    /// </summary>
    /// <param name="statLevel"></param>
    /// <param name="statMaxHealth"></param>
    /// <param name="statMaxMana"></param>
    /// <param name="statAttackPhysical"></param>
    /// <param name="statDefensePhysical"></param>
    /// <param name="statAttackSpecial"></param>
    /// <param name="statDefenseSpecial"></param>
    /// <param name="statSpeed"></param>
    /// <param name="statLuck"></param>
    public void SetStatValues(byte statLevel, int statMaxHealth, int statMaxMana, int statAttackPhysical, int statDefensePhysical, int statAttackSpecial, int statDefenseSpecial, int statSpeed, int statLuck)
    {
        this.statLevel = statLevel;
        this.statMaxHealth = statMaxHealth;
        this.statMaxMana = statMaxMana;
        this.statAttackPhysical = statAttackPhysical;
        this.statDefensePhysical = statDefensePhysical;
        this.statAttackSpecial = statAttackSpecial;
        this.statDefenseSpecial = statDefenseSpecial;
        this.statSpeed = statSpeed;
        this.statLuck = statLuck;
        currentHealth = statMaxHealth;
        currentMana = statMaxMana;
    }

    /// <summary>
    /// Set this Unit's base stats. Once done, can run algorithm to get front-end stats.
    /// List in order: HP, Mana, Physical Attack, Physical Defense, Special Attack, Special Defense, Speed
    /// </summary>
    /// <param name="statLevel"></param>
    /// <param name="baseStats"></param>
    public void SetBaseStats(byte statLevel, List<int> baseStats)
    {
        this.statLevel = statLevel;
        baseStatMaxHealth = baseStats[0];
        baseStatMaxMana = baseStats[1];
        baseStatAttackPhysical = baseStats[2];
        baseStatDefensePhysical = baseStats[3];
        baseStatAttackSpecial = baseStats[4];
        baseStatDefenseSpecial = baseStats[5];
        baseStatSpeed = baseStats[6];
    }

    /// <summary>
    /// Run the algorithm to get front-end stats from base stats.
    /// </summary>
    public void GetFinalStats()
    {
        if (statLevel == 0)
        {
            Debug.LogWarning("NOTE: StatLevel is 0. Cancelling rest of stats.");
            return;
        }
        statMaxHealth = baseStatMaxHealth;
        statMaxMana = baseStatMaxMana;
        statAttackPhysical = GetStatFromWorkingAlgorithm(StatType.ATTACK_PHYSICAL);
        statDefensePhysical = GetStatFromWorkingAlgorithm(StatType.DEFENSE_PHYSICAL);
        statAttackSpecial = GetStatFromWorkingAlgorithm(StatType.ATTACK_SPECIAL);
        statDefenseSpecial = GetStatFromWorkingAlgorithm(StatType.DEFENSE_SPECIAL);
        statSpeed = GetStatFromWorkingAlgorithm(StatType.SPEED);
        statLuck = GetStatFromWorkingAlgorithm(StatType.LUCK);
    }

    /// <summary>
    /// Get this Unit's name.
    /// </summary>
    /// <returns></returns>
    public string GetUnitName()
    {
        return unitName;
    }

    /// <summary>
    /// Get this Unit's maximum health.
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return statMaxHealth;
    }

    /// <summary>
    /// Get this Unit's maximum mana.
    /// </summary>
    /// <returns></returns>
    public int GetMaxMana()
    {
        return statMaxMana;
    }
    
    /// <summary>
    /// Get this Unit's current health.
    /// </summary>
    /// <returns></returns>
    public virtual int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Get this Unit's current mana.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentMana()
    {
        return currentMana;
    }

    /// <summary>
    /// Return Physical Attack level.
    /// </summary>
    /// <returns></returns>
    public byte GetAttackPhysicalLevel()
    {
        return attackSpecialLevel;
    }

    /// <summary>
    /// Return Special Attack level.
    /// </summary>
    /// <returns></returns>
    public byte GetAttackSpecialLevel()
    {
        return attackSpecialLevel;
    }

    /// <summary>
    /// Return Physical Defense level.
    /// </summary>
    /// <returns></returns>
    public byte GetDefensePhysicalLevel()
    {
        return defenseSpecialLevel;
    }

    /// <summary>
    /// Return Special Defense level.
    /// </summary>
    /// <returns></returns>
    public byte GetDefenseSpecialLevel()
    {
        return defenseSpecialLevel;
    }

    /// <summary>
    /// Return Luck level.
    /// </summary>
    /// <returns></returns>
    public byte GetLuckLevel()
    {
        return luckLevel;
    }

    /// <summary>
    /// Return this Unit's stat value based off of the StatType given.
    /// </summary>
    /// <param name="statType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public virtual int GetStatValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.ATTACK_PHYSICAL:
                return statAttackPhysical;
            case StatType.DEFENSE_PHYSICAL:
                return statDefensePhysical;
            case StatType.ATTACK_SPECIAL:
                return statAttackSpecial;
            case StatType.DEFENSE_SPECIAL:
                return statDefenseSpecial;
            case StatType.SPEED:
                return statSpeed;
            case StatType.LUCK:
                return statLuck;
            default:
                throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
        }
    }

    /// <summary>
    /// Return this Unit's level.
    /// </summary>
    /// <returns></returns>
    public byte GetStatLevel()
    {
        return statLevel;
    }

    /// <summary>
    /// Has this Unit's HP reached zero?
    /// </summary>
    /// <returns></returns>
    public bool IsDefeated()
    {
        return currentHealth <= 0;
    }

    /// <summary>
    /// Return a list of status ailments this Unit is experiencing.
    /// </summary>
    /// <returns></returns>
    public List<StatusAilment> GetStatusAilments()
    {
        return statusAilments;
    }

    /// <summary>
    /// Restore this Unit's HP to the maximum health they have.
    /// </summary>
    public virtual void HealthRestore()
    {
        currentHealth = statMaxHealth;
    }

    /// <summary>
    /// Restore this Unit's HP by a static amount.
    /// </summary>
    /// <param name="restoreValue"></param>
    public virtual void HealthRestore(int restoreValue)
    {
        currentHealth += restoreValue;
        if (currentHealth >= statMaxHealth)
        {
            currentHealth = statMaxHealth;
        }
    }

    /// <summary>
    /// Restore this Unit's HP by at least the health value given.
    /// </summary>
    /// <param name="restoreValue"></param>
    /// <param name="statLuck"></param>
    public void HealthRestore(int restoreValue, int statLuck)
    {
        // TODO: be able to restore additional health to gain via statLuck
        currentHealth += restoreValue;
        if (currentHealth >= statMaxHealth)
        {
            currentHealth = statMaxHealth;
        }
    }

    /// <summary>
    /// Add all Stat Changes from an Equippable Item to this Unit. Skips any Stat Changes that are not applicable to this Unit.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddEquipmentStatChanges(Item item)
    {
        if (!item.IsEquipment())
        {
            return false;
        }
        Dictionary<StatEffectType, int>.KeyCollection keys = item.GetStatValues().Keys;
        HeroStats hero = GetComponent<HeroStats>();
        bool usedForSomething = false;
        foreach (StatEffectType key in keys)
        {
            switch (key)
            {
                case StatEffectType.POWER:
                    // Equipment should not change Power to Attacks
                    continue;
                case StatEffectType.HEALTH_MAX:
                    hero.AddEquipStatValue(StatType.MAX_HEALTH, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.MANA_MAX:
                    hero.AddEquipStatValue(StatType.MAX_MANA, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.HEALTH_CURRENT:
                    // Equipment should not change current health
                    continue;
                case StatEffectType.MANA_CURRENT:
                    // Equipment should not change current mana
                    continue;
                case StatEffectType.ATTACK_PHYSICAL:
                    hero.AddEquipStatValue(StatType.ATTACK_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.ATTACK_SPECIAL:
                    hero.AddEquipStatValue(StatType.ATTACK_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.DEFENSE_PHYSICAL:
                    hero.AddEquipStatValue(StatType.DEFENSE_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.DEFENSE_SPECIAL:
                    hero.AddEquipStatValue(StatType.DEFENSE_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.LUCK:
                    hero.AddEquipStatValue(StatType.LUCK, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
            }
        }
        return usedForSomething;
    }

    /// <summary>
    /// Remove all Stat Changes from an Equippable Item to this Unit. Skips any Stat Changes that are not applicable to this Unit.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ReduceEquipmentStatChanges(Item item)
    {
        if (!item.IsEquipment())
        {
            return false;
        }
        Dictionary<StatEffectType, int>.KeyCollection keys = item.GetStatValues().Keys;
        HeroStats hero = GetComponent<HeroStats>();
        bool usedForSomething = false;
        foreach (StatEffectType key in keys)
        {
            switch (key)
            {
                case StatEffectType.POWER:
                    // Equipment should not change Power to Attacks
                    continue;
                case StatEffectType.HEALTH_MAX:
                    hero.ReduceEquipStatValue(StatType.MAX_HEALTH, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.MANA_MAX:
                    hero.ReduceEquipStatValue(StatType.MAX_MANA, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.HEALTH_CURRENT:
                    // Equipment should not change current health
                    continue;
                case StatEffectType.MANA_CURRENT:
                    // Equipment should not change current mana
                    continue;
                case StatEffectType.ATTACK_PHYSICAL:
                    hero.ReduceEquipStatValue(StatType.ATTACK_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.ATTACK_SPECIAL:
                    hero.ReduceEquipStatValue(StatType.ATTACK_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.DEFENSE_PHYSICAL:
                    hero.ReduceEquipStatValue(StatType.DEFENSE_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.DEFENSE_SPECIAL:
                    hero.ReduceEquipStatValue(StatType.DEFENSE_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
                case StatEffectType.LUCK:
                    hero.ReduceEquipStatValue(StatType.LUCK, item.GetStatValues().GetValueOrDefault(key));
                    usedForSomething = true;
                    continue;
            }
        }
        return usedForSomething;
    }

    /// <summary>
    /// Use all Stat Changes associated with this Item. Return false if no Stat Changes were used, or Equip.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddStatChanges(Item item)
    {
        if (item.IsEquipment())
        {
            return false;
        }
        Dictionary<StatEffectType, int>.KeyCollection keys = item.GetStatValues().Keys;
        HeroStats hero = GetComponent<HeroStats>();
        bool usedForSomething = false;
        foreach (StatEffectType key in keys)
        {
            switch (key)
            {
                case StatEffectType.POWER:
                    // Power should not be changed by Items
                    continue;
                case StatEffectType.HEALTH_MAX:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.MAX_HEALTH, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.MANA_MAX:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.MAX_MANA, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.HEALTH_CURRENT:
                    HealthRestore(item.GetStatValues().GetValueOrDefault(key));
                    continue;
                case StatEffectType.MANA_CURRENT:
                    //TODO: Add ManaRestore() method to UnitStats and implement it here
                    continue;
                case StatEffectType.ATTACK_PHYSICAL:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.ATTACK_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.ATTACK_SPECIAL:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.ATTACK_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.DEFENSE_PHYSICAL:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.DEFENSE_PHYSICAL, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.DEFENSE_SPECIAL:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.DEFENSE_SPECIAL, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
                case StatEffectType.LUCK:
                    if (hero != null)
                    {
                        hero.AddPermBonusStatValue(StatType.LUCK, item.GetStatValues().GetValueOrDefault(key));
                        usedForSomething = true;
                    }
                    continue;
            }
        }
        return usedForSomething;
    }

    /// <summary>
    /// Use all Stat Changes associated with this Item. Return false if no Stat Changes were used, or Equip.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ReduceStatChanges(Item item)
    {
        if (item.IsEquipment())
        {
            return false;
        }
        Dictionary<StatEffectType, int>.KeyCollection keys = item.GetStatValues().Keys;
        bool usedForSomething = false;
        foreach (StatEffectType key in keys)
        {
            if (key == StatEffectType.HEALTH_CURRENT)
            {
                TakeDamage(item.GetStatValues().GetValueOrDefault(key));
                usedForSomething = true;
                continue;
            }
            if (key == StatEffectType.MANA_CURRENT)
            {
                //TODO: Add ManaRestore() method to UnitStats and implement it here
                usedForSomething = true;
                continue;
            }
        }
        return usedForSomething;
    }

    /// <summary>
    /// Take in this much damage immediately.
    /// </summary>
    /// <param name="staticDamage"></param>
    public void TakeDamage(int staticDamage)
    {
        Debug.Log("taking damage!!");
        currentHealth -= staticDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    /// <summary>
    /// Take in this much damage immediately.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentStats"></param>
    /// <param name="statType"></param>
    public virtual void TakeDamage(int baseDamage, UnitStats opponentStats, StatType statType)
    {
        Debug.Log("taking damage!!");
        currentHealth -= CalculateDamage(baseDamage, opponentStats, statType);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }


    /// <summary>
    /// Take in guaranteed 5% to 25% reduced damage.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentStats"></param>
    /// <param name="statType"></param>
    public virtual void TakeDamageGuaranteeLuck(int baseDamage, UnitStats opponentStats, StatType statType)
    {
        Debug.Log("taking damage!!");
        int reducedDamage = (int)(baseDamage * UnityEngine.Random.Range(0.75f, 0.95f));
        currentHealth -= CalculateDamage(reducedDamage, opponentStats, statType);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackAccuracy"></param>
    /// <returns></returns>
    public virtual bool DidAttackLand(int attackAccuracy)
    {
        Debug.Log("TODO: AttackLand() accuracy. Always true ATM");
        //TODO: Accuracy on attack
        return true;
    }

    /// <summary>
    /// Return the front-end stat value based off of the base stat value and the Unit's level.
    /// </summary>
    /// <param name="statType"></param>
    /// <returns></returns>
    protected int GetStatFromWorkingAlgorithm(StatType statType)
    {
        int statValue = 1;
        switch (statType)
        {
            case StatType.ATTACK_PHYSICAL:
                statValue = statAttackPhysical;
                break;
            case StatType.DEFENSE_PHYSICAL:
                statValue = statDefensePhysical;
                break;
            case StatType.ATTACK_SPECIAL:
                statValue = statAttackSpecial;
                break;
            case StatType.DEFENSE_SPECIAL:
                statValue = statDefenseSpecial;
                break;
            case StatType.SPEED:
                statValue = statSpeed;
                break;
            case StatType.LUCK:
                statValue = statLuck;
                break;
            default:
                Debug.LogError("ERROR: Unit using an illegal stat type.");
                break;
        }
        int workingAlgorithmOutput = Mathf.FloorToInt((2 * Mathf.Clamp(statValue, 5, 100) * statLevel) / 50) + 5;
        return workingAlgorithmOutput;
    }

    /// <summary>
    /// Calculate damage to receive.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentStats"></param>
    /// <param name="statType"></param>
    /// <returns></returns>
    protected int CalculateDamage(int baseDamage, UnitStats opponentStats, StatType statType)
    {
        int defendingStat = 0;
        switch(statType)
        {
            case StatType.ATTACK_PHYSICAL:
                defendingStat = statDefensePhysical;
                if (opponentStats == null)
                {
                    Debug.Log("DMG Calc: " + baseDamage + ", Null Attack, " + statType);
                }
                else
                {
                    Debug.Log("DMG Calc: " + baseDamage + ", " + opponentStats.GetStatValue(StatType.ATTACK_PHYSICAL) + ", " + statType);
                }
                break;
            case StatType.ATTACK_SPECIAL:
                defendingStat = statDefenseSpecial;
                if (opponentStats == null)
                {
                    Debug.Log("DMG Calc: " + baseDamage + ", Null Special Attack, " + statType);
                }
                else
                {
                    Debug.Log("DMG Calc: " + baseDamage + ", " + opponentStats.GetStatValue(StatType.ATTACK_SPECIAL) + ", " + statType);
                }
                break;
            default:
                Debug.LogError("ERROR: Unit using an illegal stat type.");
                break;
        }
        //TODO: Look at Pokomon damage calculation
        return baseDamage;
    }
}
