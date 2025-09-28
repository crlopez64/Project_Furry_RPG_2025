using UnityEngine;

/// <summary>
/// Script in charge of Hero Attacks.
/// </summary>
public class HeroAttack : UnitAttack
{
    //TODO: Eventually, make unique character classes that have their own unique Attack list.
    public bool useSampleAttackList;

    void Start()
    {
        if (useSampleAttackList)
        {
            attackList = new SampleOneAttackList();
            skillsList = new SampleOneSkillList();
        }
    }
}
