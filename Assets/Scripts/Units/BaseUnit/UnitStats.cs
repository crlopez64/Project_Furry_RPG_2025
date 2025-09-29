using System;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// For referencing Stats if needed.
    /// </summary>
    public enum StatType
    {
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
    /// Return this Unit's Physical Attack Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatAttackPhysical()
    {
        return statAttackPhysical;
    }

    /// <summary>
    /// Return this Unit's Physical Defense Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatDefensePhysical()
    {
        return statDefensePhysical;
    }

    /// <summary>
    /// Return this Unit's Special Attack Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatAttackSpecial()
    {
        return statAttackSpecial;
    }

    /// <summary>
    /// Return this Unit's Special Defense Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatDefenseSpecial()
    {
        return statDefenseSpecial;
    }

    /// <summary>
    /// Return this Unit's Speed Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatSpeed()
    {
        return statSpeed;
    }

    /// <summary>
    /// Return this Unit's Luck Stat.
    /// </summary>
    /// <returns></returns>
    public int GetStatLuck()
    {
        return statLuck;
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
    /// Take in this much damage immediately.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentAttackStat"></param>
    /// <param name="statType"></param>
    public virtual void TakeDamage(int baseDamage, UnitStats opponentStats, StatType statType)
    {
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
        int reducedDamage = (int)(baseDamage * UnityEngine.Random.Range(0.75f, 0.95f));
        currentHealth -= CalculateDamage(reducedDamage, opponentStats, statType);
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    /// <summary>
    /// Calculate damage to receive.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentAttackStat"></param>
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
                    Debug.Log("DMG Calc: " + baseDamage + ", " + opponentStats.GetStatAttackPhysical() + ", " + statType);
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
                    Debug.Log("DMG Calc: " + baseDamage + ", " + opponentStats.GetStatAttackSpecial() + ", " + statType);
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
