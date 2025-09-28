using System.Collections.Generic;
using UnityEngine;
using static UnitStats;

/// <summary>
/// Create a sample Enemy Attack List.
/// </summary>
public class SampleEnemyAttack : MoveList
{
    
    public SampleEnemyAttack() : base()
    {
        moveList.Add(new Attack(1, "Single Slash", "A short and simple slash attack.", 0, 0, BaseItem.IntendedTarget.ENEMY_ONE, UnitType.NORMAL,
            new AttackStep[] {
                new (ActionCommand.ActionType.NONE,
                    "N/A",
                    null /** No Buttons Required **/,
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 40 } },
                    new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                    0) }
            ));

        moveList.Add(new Attack(2, "Hash Slinging Slasher", "Three quick slashes against one Enemy.", 0, 0, BaseItem.IntendedTarget.ENEMY_ONE, UnitType.NORMAL,
            new AttackStep[] {
                new (ActionCommand.ActionType.NONE,
                    "N/A",
                    null /** No Buttons Required **/,
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 12 } },
                    new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                    0),
                new (ActionCommand.ActionType.NONE,
                    "N/A",
                    null /** No Buttons Required **/,
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 12 } },
                    new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                    0),
                new (ActionCommand.ActionType.NONE,
                    "N/A",
                    null /** No Buttons Required **/,
                    new Dictionary<BaseItem.StatEffectType, int>() {
                        { BaseItem.StatEffectType.POWER, 20} },
                new Dictionary<StatusAilment, int>( /**No Status Ailments**/ ),
                0)}
            ));
    }
}
