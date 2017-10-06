using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;
using System.Diagnostics;
using System;

namespace Frozen.Rotation
{
    public class WarriorFury : CombatRoutine
    {

        public override Form SettingsForm { get; set; }

		//private bool validtargetmelee;

        public override void Initialize()
		{
			Log.Write("Welcome to Fury Warrior", Color.Green);
			Log.Write("Suggested build: 2x1x232", Color.Green);
		}

        public override void Stop()
		{
		}

        public override void Pulse()
		{
			//validtargetmelee = WoW.HasTarget && WoW.IsInCombat && !WoW.PlayerIsChanneling && WoW.TargetIsVisible && WoW.RangeToTarget < 6;

			if (WoW.PlayerHealthPercent == 0 || WoW.IsMounted) return;
            if (WoW.PlayerIsChanneling) return;

            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
			{
				WoW.CastSpell("BattleCry", !WoW.PlayerHasBuff("BattleCryBuff") && WoW.CooldownsOn == true);
				WoW.CastSpell("Avatar", WoW.Talent(3) == 3 && WoW.PlayerHasBuff("BattleCryBuff"));
				WoW.CastSpell("OdynsFury", WoW.PlayerHasBuff("BattleCryBuff") && WoW.PlayerHasBuff("Frothing"));
				WoW.CastSpell("Bloodthirst", WoW.PlayerHasBuff("Enraged Regeneration"));
				WoW.CastSpell("Furious Slash", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.PlayerHasBuff("Enraged Regeneration"));
				WoW.CastSpell("Raging Blow", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Furious Slash") && WoW.PlayerHasBuff("Enraged Regeneration"));
			}

			if (WoW.TargetHealthPercent == 0 && WoW.IsInCombat && WoW.RangeToTarget >= 5 && WoW.CountEnemyNPCsInRange >= 1 && !WoW.IsMounted|| !WoW.HasTarget && WoW.IsInCombat && WoW.CountEnemyNPCsInRange >= 1 && WoW.RangeToTarget >=1 && !WoW.IsMounted) 
			{
				WoW.TargetNearestEnemy();
				return;
			}

			if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent >= 21 && !WoW.PlayerHasBuff("BattleCryBuff")) // Do Single Target Stuff here
				if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
				{	
					WoW.CastSpell("Rampage", WoW.Rage >= 100);
					WoW.CastSpell("Bloodthirst", WoW.Rage < 100 && !WoW.PlayerHasBuff("Enrage") || WoW.IsSpellOnCooldown("Raging Blow") && WoW.Rage < 100);
					WoW.CastSpell("Raging Blow", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.Rage < 100);
					WoW.CastSpell("Whirlwind", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow") && WoW.PlayerHasBuff("Wrecking Ball") && WoW.Talent(3) == 1);
					WoW.CastSpell("Furious Slash", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow") && WoW.Rage < 90);
				}

			if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent >= 21 && WoW.PlayerHasBuff("BattleCryBuff")) // Do Single Target Stuff here
				if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
				{
					WoW.CastSpell("Rampage", WoW.Rage >= 100);
					WoW.CastSpell("Raging Blow", WoW.PlayerHasBuff("Enrage") && WoW.PlayerHasBuff("Frothing") && WoW.Rage < 100 || WoW.IsSpellOnCooldown("Bloodthirst"));
					WoW.CastSpell("Bloodthirst", WoW.Rage < 100 && WoW.IsSpellOnCooldown("Raging Blow"));
                }
				
			if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent <= 20 && WoW.SpellCooldownTimeRemaining("BattleCry") >= 1100) // Do Single Target Execute Stuff here
				if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
				{
					WoW.CastSpell("Execute", WoW.Rage >= 100);
					WoW.CastSpell("Bloodthirst", WoW.Rage < 100);
					WoW.CastSpell("Raging Blow", WoW.Rage < 100 && WoW.IsSpellOnCooldown("Bloodthirst"));
					WoW.CastSpell("Furious Slash", WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow"));
				}
				
			if (combatRoutine.Type == RotationType.SingleTarget && WoW.TargetHealthPercent <= 20 && WoW.SpellCooldownTimeRemaining("BattleCry") <= 900) // Do Single Target Execute Stuff here
				if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
				{
					WoW.CastSpell("Execute", WoW.PlayerHasBuff("SenseDeath") || WoW.Rage>=25);
					WoW.CastSpell("Bloodthirst", WoW.Rage < 25);
					WoW.CastSpell("Raging Blow", WoW.Rage < 25 && WoW.IsSpellOnCooldown("Bloodthirst"));
				}
				
			if (combatRoutine.Type == RotationType.AOE)
				if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsSpellInRange("Bloodthirst"))
				{
					WoW.CastSpell("Whirlwind", !WoW.PlayerHasBuff("Meat-Cleaver") || WoW.CanCast("Whirlwind") && WoW.PlayerHasBuff("Wrecking Ball") && WoW.Talent(3) == 1);
					WoW.CastSpell("Rampage", WoW.Rage >= 100 && WoW.PlayerHasBuff("Meat-Cleaver"));
					WoW.CastSpell("Bloodthirst", WoW.Rage < 100 && WoW.CountEnemyNPCsInRange < 8 && WoW.PlayerHasBuff("Meat-Cleaver") && !WoW.PlayerHasBuff("Enrage"));
					WoW.CastSpell("Raging Blow", WoW.PlayerHasBuff("Enrage") && WoW.CountEnemyNPCsInRange < 4);
					//WoW.CastSpell("Furious Slash",  WoW.IsSpellOnCooldown("Bloodthirst") && WoW.IsSpellOnCooldown("Raging Blow"));
                }
			
			if (combatRoutine.Type == RotationType.Cleave)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Lelefi
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,23881,Bloodthirst,D3
Spell,85288,Raging Blow,D4
Spell,100130,Furious Slash,D5
Spell,5308,Execute,D6
Spell,184367,Rampage,D7
Spell,190411,Whirlwind,D8
Spell,1719,BattleCry,R
Spell,118000,DragonRoar,-
Spell,107574,Avatar,D0
Spell,205545,OdynsFury,T
Spell,18499,Berserker Rage,U
Aura,118000,DragonRoarBuff
Aura,1719,BattleCryBuff
Aura,200863,SenseDeath
Aura,184362,Enrage
Aura,201009,Juggernaut
Aura,206316,Massacre
Aura,85739,Meat-Cleaver
Aura,215570,Wrecking Ball
Aura,215572,Frothing
Aura,184364,Enraged Regeneration
Aura,118699,Fear
Aura,107574,AvatarBuff
*/
