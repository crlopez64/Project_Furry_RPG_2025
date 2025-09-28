using System.Collections.Generic;
using UnityEngine;
using static UnitStats;

/// <summary>
/// Create a sample Skill List.
/// </summary>
public class SampleOneSkillList : MoveList
{
    public SampleOneSkillList() : base()
    {
        moveList.Add(new Attack(1, BaseItem.ItemClassification.SKILL, "Magnum Blast", "Charge and blast a powerful Energy Projectile from your sword.", 0, 50, BaseItem.IntendedTarget.ENEMY_ONE, UnitType.NORMAL,
            new AttackStep[] {
                new (ActionCommand.ActionType.RAPID_PRESS,
                "MEDIUM",
                    new ActionCommand.ActionButtonPressed[] {
                        ActionCommand.ActionButtonPressed.BUTTON_WEST },
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 0 } },
                new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                0),
                 new (ActionCommand.ActionType.TIMELY_PRESS,
                 "QUICK",
                    ActionCommand.GetAllButtonsRequired(),
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 60 } },
                new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                0)}
            ));
        moveList.Add(new Attack(2, BaseItem.ItemClassification.SKILL, "Sword Dance", "Charge your sword and expel that energy into the User. Increase Attack and Luck by 1 Level.", 0, 30, BaseItem.IntendedTarget.SELF, UnitType.NORMAL,
            new AttackStep[] {
                new (ActionCommand.ActionType.RAPID_PRESS,
                "LONG",
                    ActionCommand.GetAllButtonsRequired(),
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.ATTACK_PHYSICAL, 1 },
                        { BaseItem.StatEffectType.LUCK, 1 } },
                new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                0)}
            ));
    }
}
