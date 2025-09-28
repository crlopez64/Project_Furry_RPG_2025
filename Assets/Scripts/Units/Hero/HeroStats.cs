using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One of the playable character's stats.
/// </summary>
public class HeroStats : UnitStats
{
    private HudHeroMaster hudHeroMaster;
    private const float healthEstimateTimerResetTo = 0.25f;
    private float healthEstimateTimer = healthEstimateTimerResetTo;
    /// <summary>
    /// How fast the HP will roll up or down. The higher the value, the faster it will roll.
    /// </summary>
    private int healthEstimateRollerSpeed;
    /// <summary>
    /// The estimated health to report for currentHealth to reach to, before Luck stat.
    /// </summary>
    private int currentHealthEstimate;
    /// <summary>
    /// The true estimated health to report to for currentHealth to reach to, after Luck stat.
    /// </summary>
    private int trueCurrentHealthEstimate;

    public string setName;
    public int setMaxHealth;
    public int setMaxMana;
    public int setAttackPhysical;
    public int setDefensePhysical;
    public int setAttackSpecial;
    public int setDefenseSpecial;
    public int setSpeed;
    public int setLuck;
    [Range(1, 6)]
    public int setDamageRollerSpeed;
    
    void Awake()
    {
        // Set testing Stats
        unitName = setName;
        statMaxHealth = setMaxHealth;
        currentHealth = setMaxHealth;
        statMaxMana = setMaxMana;
        currentMana = setMaxMana;
        statAttackPhysical = setAttackPhysical;
        statDefensePhysical = setDefensePhysical;
        statAttackSpecial = setAttackSpecial;
        statDefenseSpecial = setDefenseSpecial;
        statSpeed = setSpeed;
        statLuck = setLuck;
        // Set the Health Estimate Roller
        currentHealthEstimate = currentHealth;
        trueCurrentHealthEstimate = currentHealth;
        healthEstimateRollerSpeed = setDamageRollerSpeed;
    }
    void Start()
    {
        statusAilments = new List<StatusAilment>();
    }

    void Update()
    {
        if (HealthEstimateAligned())
        {
            HealthEstimateTimerReset();
            HealthEstimateClear();
        }
        else
        {
            healthEstimateTimer -= (Time.deltaTime * healthEstimateRollerSpeed);
            if (HealthEstimateTimerReached())
            {
                if (currentHealth < currentHealthEstimate)
                {
                    //Roll up
                    CurrentHealthRollUp();
                    HealthEstimateTimerReset();
                    if ((currentHealth >= trueCurrentHealthEstimate) && (!TrueHealthEstimateAligned(true)))
                    {
                        currentHealthEstimate = trueCurrentHealthEstimate;
                        HealthEstimateCorrect();
                    }
                }
                else if (currentHealth > currentHealthEstimate)
                {
                    //Roll down
                    CurrentHealthRollDown();
                    HealthEstimateTimerReset();
                    if ((currentHealth <= trueCurrentHealthEstimate) && (!TrueHealthEstimateAligned(false)))
                    {
                        currentHealthEstimate = trueCurrentHealthEstimate;
                        HealthEstimateCorrect();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set this Hero's own HUD Master.
    /// </summary>
    /// <param name="hudHeroMaster"></param>
    public void SetHudHeroMaster(HudHeroMaster hudHeroMaster)
    {
        this.hudHeroMaster = hudHeroMaster;
        this.hudHeroMaster.StartHud(this);
    }

    /// <summary>
    /// Show that it's this Hero's turn.
    /// </summary>
    public void ThisHerosTurn()
    {
        hudHeroMaster.SetCurrentTurnOn();
    }

    /// <summary>
    /// Show that it's no longer this Hero's turn.
    /// </summary>
    public void NoLongerHerosTurn()
    {
        hudHeroMaster.SetCurrentTurnOff();
    }

    /// <summary>
    /// Stop the Health Estimate Roller  in place.
    /// </summary>
    public void DamageRollerStop()
    {
        currentHealthEstimate = currentHealth;
        trueCurrentHealthEstimate = currentHealth;
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthEstimateFlash(currentHealthEstimate.ToString());
        }
    }

    /// <summary>
    /// Reset the Health Estimate Roller Speed to 3.
    /// </summary>
    public void ResetHealthEstimateRollerSpeed()
    {
        healthEstimateRollerSpeed = 3;
    }

    /// <summary>
    /// Increase the Health Estimate Roller Speed by step count (e.g. speed up taking damage and make the kill quick), up to 6.
    /// </summary>
    /// <param name="steps"></param>
    public void HealthEstimateRollerSpeedIncrease(int steps)
    {
        healthEstimateRollerSpeed += steps;
        if (healthEstimateRollerSpeed >= 6)
        {
            healthEstimateRollerSpeed = 6;
        }
    }

    /// <summary>
    /// Decrease the Health Estimate Roller  Speed by step count (e.g. slow down taking damage), minimum 1.
    /// </summary>
    /// <param name="steps"></param>
    public void HealthEstimateRollerSpeedDecrease(int steps)
    {
        healthEstimateRollerSpeed -= steps;
        if (healthEstimateRollerSpeed <= 1)
        {
            healthEstimateRollerSpeed = 1;
        }
    }

    /// <summary>
    /// Restore Hero's Health to max; set Estimate to max health to avoid weird rolling.
    /// </summary>
    public override void HealthRestore()
    {
        base.HealthRestore();
        currentHealthEstimate = statMaxHealth;
        trueCurrentHealthEstimate = statMaxHealth;
    }

    /// <summary>
    /// Add the current Health Estimate for Current Health to roll to.
    /// </summary>
    /// <param name="restoreValue"></param>
    public override void HealthRestore(int restoreValue)
    {
        currentHealthEstimate += restoreValue;
        trueCurrentHealthEstimate += restoreValue;
        if (currentHealthEstimate >= statMaxHealth)
        {
            currentHealthEstimate = statMaxHealth;
            trueCurrentHealthEstimate = statMaxHealth;
        }
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthEstimate(currentHealthEstimate.ToString());
            hudHeroMaster.SetCurrentHealthColorSplash("RESTORE");
        }
    }

    /// <summary>
    /// Reduce the Current Health Estimate for Current Health to roll to.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentAttackStat"></param>
    /// <param name="statType"></param>
    public override void TakeDamage(int baseDamage, UnitStats opponentStats, StatType statType)
    {
        //TODO: Apply luck for hidden Health estimate that's above currentHealthEstimate
        currentHealthEstimate -= CalculateDamage(baseDamage, opponentStats, statType);
        trueCurrentHealthEstimate = currentHealthEstimate;
        if (currentHealthEstimate <= 0)
        {
            currentHealthEstimate = 0;
        }
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthEstimate(currentHealthEstimate.ToString());
            hudHeroMaster.SetCurrentHealthColorSplash("DAMAGE");
        }
    }

    /// <summary>
    /// Reduce the Current Health Estimate by a reduced amount.
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <param name="opponentStats"></param>
    /// <param name="statType"></param>
    public override void TakeDamageGuaranteeLuck(int baseDamage, UnitStats opponentStats, StatType statType)
    {
        int reducedDamageEstimate = (int)(baseDamage * Random.Range(0.75f, 0.95f));
        currentHealthEstimate -= CalculateDamage(baseDamage, opponentStats, statType);
        trueCurrentHealthEstimate -= CalculateDamage(reducedDamageEstimate, opponentStats, statType);
        if (currentHealthEstimate <= 0)
        {
            currentHealthEstimate = 0;
        }
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthEstimate(currentHealthEstimate.ToString());
            hudHeroMaster.SetCurrentHealthColorSplash("DAMAGE");
        }
    }

    /// <summary>
    /// Have Hero survive a fatal hit by secretely making them survive to 1 HP.
    /// Does nothing if trueHealthEstimate is at least 1 HP or Health is not rolling down.
    /// </summary>
    public void SurviveFatalHit()
    {
        if (HealthEstimateAligned())
        {
            return;
        }
        if (trueCurrentHealthEstimate == 0)
        {
            trueCurrentHealthEstimate = 1;
        }
    }

    /// <summary>
    /// Does Health Estimate equal currentHealth?
    /// </summary>
    /// <returns></returns>
    public bool HealthEstimateAligned()
    {
        return currentHealthEstimate == currentHealth;
    }

    /// <summary>
    /// Are the True Health Estimate and Health Estimate equal?
    /// </summary>
    /// <param name="goingDown"></param>
    /// <returns></returns>

    public bool TrueHealthEstimateAligned(bool goingDown)
    {
        if (goingDown)
        {
            return currentHealthEstimate <= trueCurrentHealthEstimate;
        }
        return currentHealthEstimate >= trueCurrentHealthEstimate;
    }

    /// <summary>
    /// Did this Hero take a hit that would take them down?
    /// Does not interpret via trueHealthEstimate
    /// </summary>
    /// <returns></returns>
    public bool TookLethalDamage()
    {
        return currentHealthEstimate == 0;
    }

    /// <summary>
    /// Return the Health Estimate for Current Health to roll to.
    /// </summary>
    /// <returns></returns>
    public override int GetCurrentHealth()
    {
        return currentHealthEstimate;
    }

    /// <summary>
    /// Roll up the Current Health.
    /// </summary>
    private void CurrentHealthRollDown()
    {
        currentHealth--;
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthCurrent(currentHealth.ToString(), DetermineHealthStatus());
        }
    }

    /// <summary>
    /// Roll down the Current Health.
    /// </summary>
    private void CurrentHealthRollUp()
    {
        currentHealth++;
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthCurrent(currentHealth.ToString(), DetermineHealthStatus());
        }
    }

    /// <summary>
    /// Correct the Health Estimate and flash it.
    /// </summary>
    private void HealthEstimateCorrect()
    {
        currentHealthEstimate = trueCurrentHealthEstimate;
        if (hudHeroMaster != null)
        {
            hudHeroMaster.SetHealthEstimateFlash(currentHealthEstimate.ToString());
        }
    }

    /// <summary>
    /// Clear out the Health Estimate.
    /// </summary>
    private void HealthEstimateClear()
    {
        if (hudHeroMaster != null)
        {
            hudHeroMaster.ClearHealthEstimate();
        }
    }

    /// <summary>
    /// Reset the Health Estimate Roller Timer back to 10.
    /// </summary>
    private void HealthEstimateTimerReset()
    {
        healthEstimateTimer = healthEstimateTimerResetTo;
    }

    /// <summary>
    /// Has the Health Estimate Timer reached zero?
    /// </summary>
    /// <returns></returns>
    private bool HealthEstimateTimerReached()
    {
        return healthEstimateTimer <= 0;
    }
    
    /// <summary>
    /// Determine health status.
    /// </summary>
    /// <returns></returns>
    private string DetermineHealthStatus()
    {
        float remainingHealth = (float)currentHealth / statMaxHealth;
        if ((remainingHealth <= 0.3f) && (remainingHealth > 0.15f))
        {
            return "WARNING";
        }
        else if (remainingHealth <= 0.15f)
        {
            return "DANGER";
        }
        return "GOOD";
    }

}
