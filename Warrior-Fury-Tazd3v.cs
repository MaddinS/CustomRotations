using System;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarriorFuryTazd3v : CombatRoutine
    {
        public override string Name
        {
            get { return "Fury Warrior"; }
        }

        public override string Class
        {
            get { return "Warrrior"; }
        }

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to Fury Warrior by taz", Color.Purple);
            Log.Write("READ ME: This rotation will work perfectly with the following talent build: 2333232", Color.Red);
            Log.Write("READ ME: This talent build maximizes dps on single target fights", Color.Red);
            Log.Write("READ ME: You will be controlling Charge/Leap", Color.Red);
            Log.Write("This is the best fury warrior routine in 7.2.5", Color.Red);
        }

        public override void Stop()
        {
        }

        public override void Pulse() 
        {
            if (WoW.PlayerHasBuff("Mount")) return;
            if (UseCooldowns)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                {
                    if (WoW.CanCast("BattleCry"))
                    {
                        WoW.CastSpell("BattleCry");
                        return;
                    }
                    if (WoW.CanCast("Avatar") && (WoW.SpellCooldownTimeRemaining("BattleCry") > 14))
                    {
                        WoW.CastSpell("Avatar") ;
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat)
                {
                    /* interrupting */
                    if (WoW.TargetIsCastingAndSpellIsInterruptible && WoW.CanCast("Pummel") && WoW.IsSpellInRange("Pummel"))
                    {
                        WoW.CastSpell("Pummel");
                        return;
                    }
                    /* --------------------- end of interrupting--------------------*/
                    /* defensive CD */
                    if (WoW.CanCast("Enraged Regeneration") && (WoW.HealthPercent < 20))
                    {
                        WoW.CastSpell("Enraged Regeneration");
                        if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && WoW.PlayerHasBuff("Enraged Regeneration"))
                        {
                            WoW.CastSpell("Bloodthirst");
                            return;
                        }
                        return;
                    }
                    if (WoW.CanCast("Commanding Shout") && (WoW.HealthPercent < 15))
                    {
                        WoW.CastSpell("Commanding Shout");
                        if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst"))
                        {
                            WoW.CastSpell("Bloodthirst");
                            return;
                        }
                        return;
                    }
                    if (WoW.PlayerHasDebuff("Fear") && WoW.CanCast("Berserker Rage"))
                    {
                        WoW.CastSpell("Berserker Rage");
                        return;
                    }
                    if (WoW.PlayerHasDebuff("Stunned") && WoW.CanCast("Every Man for Himself"))
                    {
                        WoW.CastSpell("Every Man for Himself");
                        return;
                    }
                    /* --------------------- end of defensive CD--------------------*/
                    /* dps-ing */
                    if (!WoW.PlayerHasBuff("BattleCry"))
                    { 
                        if (WoW.CanCast("Rampage") && WoW.IsSpellInRange("Rampage") && (WoW.Rage>=100) && !WoW.PlayerHasBuff("Enrage"))
                        {
                            WoW.CastSpell("Rampage");
                            return;
                        }
                        if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && !WoW.PlayerHasBuff("Enrage"))
                        {
                            WoW.CastSpell("Bloodthirst");
                            return;
                        }
                        if (WoW.CanCast("OdynsFury") && WoW.IsSpellInRange("OdynsFury") && (WoW.PlayerHasBuff("BattleCry") || WoW.HealthPercent < 10))
                        {
                            WoW.CastSpell("OdynsFury");
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
                        if (WoW.CanCast("Whirlwind") && WoW.PlayerHasBuff("Wrecking Ball") && WoW.IsSpellInRange("Whirlwind"))
                        {
                            WoW.CastSpell("Whirlwind");
                            return;
                        }
                        if (WoW.CanCast("Furious Slash") && WoW.IsSpellInRange("Furious Slash") && (WoW.SpellCooldownTimeRemaining("Raging Blow") > 0.5))
                        {
                            WoW.CastSpell("Furious Slash");
                            return;
                        }
                    }
                    if (WoW.PlayerHasBuff("BattleCry"))
                    {
                        if (WoW.CanCast("Rampage") && WoW.IsSpellInRange("Rampage") && (WoW.Rage >= 100) && (!WoW.PlayerHasBuff("Enrage") || (WoW.PlayerBuffTimeRemaining("Enrage") <= 2)))
                        {
                            WoW.CastSpell("Rampage");
                            return;
                        }
                        if (WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
                        {
                            WoW.CastSpell("Raging Blow");
                            return;
                        }
                        if (WoW.CanCast("OdynsFury") && WoW.IsSpellInRange("OdynsFury") && (WoW.PlayerHasBuff("BattleCry") || WoW.HealthPercent < 10))
                        {
                            WoW.CastSpell("OdynsFury");
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

                    }
                    /* --------------------- end of dps-ing--------------------*/
                }
            }
            if ((combatRoutine.Type == RotationType.AOE) || (combatRoutine.Type == RotationType.Cleave))
            {
                if (WoW.CanCast("OdynsFury") && WoW.IsSpellInRange("OdynsFury") && (WoW.PlayerHasBuff("BattleCry") || WoW.HealthPercent < 10))
                {
                    WoW.CastSpell("OdynsFury");
                    return;
                }
                if (!WoW.PlayerHasBuff("Meat-Cleaver"))
                {
                    WoW.CastSpell("Whirlwind");
                    return;
                }
                if (WoW.CanCast("Rampage") && WoW.IsSpellInRange("Rampage") && (WoW.Rage >= 100) && !WoW.PlayerHasBuff("Enrage") && WoW.PlayerHasBuff("Meat-Cleaver"))
                {
                    WoW.CastSpell("Rampage");
                    return;
                }
                if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && !WoW.PlayerHasBuff("Enrage") && WoW.PlayerHasBuff("Meat-Cleaver"))
                {
                    WoW.CastSpell("Bloodthirst");
                    return;
                }

                if (WoW.CanCast("Bloodthirst") && WoW.IsSpellInRange("Bloodthirst") && WoW.PlayerHasBuff("Meat-Cleaver"))
                {
                    WoW.CastSpell("Bloodthirst");
                    return;
                }
                if (WoW.CanCast("Raging Blow") && WoW.IsSpellInRange("Raging Blow"))
                {
                    WoW.CastSpell("Raging Blow");
                    return;
                }
                if (WoW.CanCast("Whirlwind") && WoW.PlayerHasBuff("Wrecking Ball") && WoW.IsSpellInRange("Whirlwind"))
                {
                    WoW.CastSpell("Whirlwind");
                    return;
                }
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=tazd3v
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,23881,Bloodthirst,D1
Spell,85288,Raging Blow,D2
Spell,100130,Furious Slash,D3
Spell,184367,Rampage,D4
Spell,5308,Execute,D5
Spell,190411,Whirlwind,D6
Spell,205545,OdynsFury,D7
Spell,184364,Enraged Regeneration,D8
Spell,97642,Commanding Shout,D9
Spell,18499,Berserker Rage,D0
Spell,6552,Pummel,Y
Spell,59752,Every Man For Himself,U
Spell,1719,BattleCry,R
Spell,107574,Avatar,T
Aura,184364,Enraged Regeneration
Aura,1719,BattleCry
Aura,184362,Enrage
Aura,215572,Frothing Berserker
Aura,215560,Wrecking Ball
Aura,238574,Stunned
Aura,118699,Fear
Aura,186305,Mount
Aura,85739,Meat-Cleaver
*/
