using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarriorFury1chu : CombatRoutine
    {
		private int BattleCryRotationSequence = 0;
		
        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
			Log.Clear();
            Log.Write("Welcome to 1chu's Fury Warrior", Color.Purple);
			Log.DrawHorizontalLine();
            Log.Write("Supported Talents: 2333232", Color.Black);
            Log.Write("Written based on icy-veins guide", Color.Black);
			Log.DrawHorizontalLine();
			Log.Write("Bot can be paused by holding down left SHIFT key, AOE mode is activate while holding down left CTRL key", Color.Purple);
			Log.Write("Please note that for this to work you need to set your keybindings so that all spells can be used with CTRL key pressed as well", Color.Purple);
			Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }

        public override void Pulse() 
        {
			if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_LSHIFT) < 0) return;
			
			// Interrupt
			if (WoW.TargetIsCastingAndSpellIsInterruptible && WoW.CanCast("Pummel") && WoW.IsSpellInRange("Pummel") && WoW.TargetPercentCast > Random.Next(20, 40))
			{
				WoW.CastSpell("Pummel");
				return;
			}
			
			// Defensive CD's
			if (WoW.CanCast("Enraged Regeneration") && WoW.HealthPercent < 30)
			{
				WoW.CastSpell("Enraged Regeneration");
				WoW.CastSpell("Bloodthirst", WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst"));
				return;
			}
			if (WoW.CanCast("Commanding Shout") && (WoW.HealthPercent < 15))
			{
				WoW.CastSpell("Commanding Shout");
				WoW.CastSpell("Bloodthirst", WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst"));
				return;
			}
			if (WoW.PlayerHasDebuff("Fear"))
			{
				if (WoW.CanCast("Berserker Rage"))
				{
					WoW.CastSpell("Berserker Rage");
				}
				else if (WoW.CanCast("Will of the Forsaken"))
				{
					WoW.CastSpell("Will of the Forsaken");
				}
			}
					
			// Targeting
            if (!WoW.HasTarget || !WoW.TargetIsEnemy) WoW.TargetNearestEnemy();
			
			if (combatRoutine.Type != RotationType.SingleTarget || DetectKeyPress.GetKeyState(DetectKeyPress.VK_LCONTROL) < 0)
			{
				// Multitarget Rotation
				BattleCryRotationSequence = 0;
				if (UseCooldowns)
				{
					if (WoW.CanCast("Battle Cry") && WoW.IsSpellInRange("Rampage") && !WoW.IsSpellOnCooldown("Rampage"))
					{
						WoW.CastSpell("Battle Cry");
						WoW.CastSpell("Avatar", WoW.CanCast("Avatar"));
					}
				}
					
				if (WoW.CanCast("Odyns Fury") && WoW.IsSpellInRange("Bloodthirst") && WoW.PlayerHasBuff("Battle Cry"))
                {
                    WoW.CastSpell("Odyns Fury");
                    return;
                }
                if (!WoW.PlayerHasBuff("Meat Cleaver") && WoW.CanCast("Whirlwind") && WoW.IsSpellInRange("Bloodthirst"))
                {
                    WoW.CastSpell("Whirlwind");
                    return;
                }
                if (WoW.CanCast("Rampage") && WoW.IsSpellInRange("Rampage") && ((!WoW.PlayerHasBuff("Enrage") && WoW.Rage >= 85) || WoW.Rage >= 100))
                {
                    WoW.CastSpell("Rampage");
                    return;
                }
                if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && !WoW.PlayerHasBuff("Enrage"))
                {
                    WoW.CastSpell("Bloodthirst");
                    return;
                }
                if (combatRoutine.Type == RotationType.Cleave && WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
                {
                    WoW.CastSpell("Raging Blow");
                    return;
                }
                if (WoW.CanCast("Whirlwind") && WoW.IsSpellInRange("Bloodthirst"))
                {
                    WoW.CastSpell("Whirlwind");
                    return;
                }
			}
			
			else
			{
				// Singletarget Rotation
				if (UseCooldowns)
				{
					if (BattleCryRotationSequence == 0)
					{
						if (WoW.CanCast("Battle Cry") && WoW.IsSpellInRange("Rampage") && !WoW.IsSpellOnCooldown("Rampage"))
						{
							if (WoW.SpellCooldownTimeRemaining("Raging Blow") > 150)
							{
								//wait for raging blow to come up before using cooldowns
								WoW.CastSpell("Furious Slash", WoW.CanCast("Furious Slash") && WoW.IsSpellInRange("Furious Slash"));
								return;
							}
							WoW.CastSpell("Battle Cry");
							WoW.CastSpell("Avatar", WoW.CanCast("Avatar"));
							WoW.CastSpell("Rampage");
							BattleCryRotationSequence++;
							return;
						}
					}
					else if (BattleCryRotationSequence == 1)
					{
						if (WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
						{
							WoW.CastSpell("Raging Blow");
							BattleCryRotationSequence++;
						}
						return;
					}
					else if (BattleCryRotationSequence == 2)
					{
						if (WoW.CanCast("Odyns Fury"))
						{
							WoW.CastSpell("Odyns Fury");
							BattleCryRotationSequence++;
						}
						return;
					}
					else if (BattleCryRotationSequence == 3)
					{
						if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst"))
						{
							WoW.CastSpell("Bloodthirst");
							BattleCryRotationSequence++;
						}
						return;
					}
					else if (BattleCryRotationSequence == 4)
					{
						if (WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
						{
							WoW.CastSpell("Raging Blow");
							BattleCryRotationSequence = 0;
						}
						return;
					}
					
					if (WoW.CanCast("Avatar") && (WoW.SpellCooldownTimeRemaining("Battle Cry") < 1200 || WoW.SpellCooldownTimeRemaining("Battle Cry") > 2200))
					{
						WoW.CastSpell("Avatar");
						return;
					}
				}
				else
				{
					BattleCryRotationSequence = 0;
				}
				if (WoW.CanCast("Rampage") && WoW.IsSpellInRange("Rampage") && ((!WoW.PlayerHasBuff("Enrage") && WoW.Rage >= 85) || WoW.Rage >= 100))
				{
					WoW.CastSpell("Rampage");
					return;
				}
				if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && !WoW.PlayerHasBuff("Enrage"))
				{
					WoW.CastSpell("Bloodthirst");
					return;
				}
				if (WoW.CanCast("Odyns Fury") && WoW.PlayerHasBuff("Enrage") && WoW.PlayerHasBuff("Battle Cry"))
				{
					WoW.CastSpell("Odyns Fury");
					return;
				}
				if (WoW.CanCast("Execute") && WoW.IsSpellInRange("Execute") && WoW.PlayerHasBuff("Enrage") && WoW.TargetHealthPercent <= 20)
				{
					WoW.CastSpell("Execute");
					return;
				}
				if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst"))
				{
					WoW.CastSpell("Bloodthirst");
					return;
				}
				if (WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
				{
					WoW.CastSpell("Raging Blow");
					return;
				}
				if (WoW.CanCast("Whirlwind") && WoW.PlayerHasBuff("Wrecking Ball"))
				{
					WoW.CastSpell("Whirlwind");
					return;
				}			
				WoW.CastSpell("Furious Slash", WoW.CanCast("Furious Slash") && WoW.IsSpellInRange("Furious Slash"));
			}
		}
		public override void OutOfCombatPulse()
        {
         
        }

        public override void MountedPulse()
        {
         
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=1chu
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,23881,Bloodthirst,Z
Spell,85288,Raging Blow,G
Spell,100130,Furious Slash,H
Spell,184367,Rampage,J
Spell,205545,Odyns Fury,Y
Spell,1719,Battle Cry,U
Spell,107574,Avatar,K
Spell,5308,Execute,T
Spell,190411,Whirlwind,D5
Spell,184364,Enraged Regeneration,D6
Spell,97642,Commanding Shout,D7
Spell,18499,Berserker Rage,D8
Spell,7744,Will of the Forsaken,D9
Spell,59752,Every Man For Himself,D9
Spell,6552,Pummel,E
Aura,184364,Enraged Regeneration
Aura,1719,Battle Cry
Aura,184362,Enrage
Aura,215572,Frothing Berserker
Aura,215570,Wrecking Ball
Aura,238574,Stunned
Aura,118699,Fear
Aura,186305,Mount
Aura,85739,Meat Cleaver
*/
