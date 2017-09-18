using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Vengeance1chu : CombatRoutine
    {   
        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.DrawHorizontalLine();
            Log.WriteFrozen("Welcome to 1chu's Vengeance Demon Hunter", Color.Black);
			Log.WriteFrozen("Holding down the shift key temporarily disables the rotation bot\nNote that active mitigation spells as well as spell interrupts are not included in the rotation and have to be used manually!", Color.Purple);
			Log.DrawHorizontalLine();
		}

        public override void Stop()
        {
			
        }

        public override void Pulse()
        {
			if (DetectKeyPress.GetKeyState(DetectKeyPress.VK_LSHIFT) < 0) return;
            if (!WoW.HasTarget || !WoW.TargetIsEnemy) WoW.TargetNearestEnemy();

			if (WoW.IsSpellInRange("Soul Carver"))
			{
				if (combatRoutine.Type == RotationType.SingleTarget)
				{
					SurvivalRotation();
				}
				else
				{
					BombRotation();
				}
			}			
			
			else if (WoW.IsSpellInRange("Throw Glaive"))
			{
				WoW.CastSpell("Throw Glaive", !WoW.IsSpellOnCooldown("Throw Glaive"));
			}
        }

        public override void OutOfCombatPulse()
        {
         
        }

        public override void MountedPulse()
        {
         
        }
		
		private void SurvivalRotation()
		{
			WoW.CastSpell("Immolation Aura", !WoW.IsSpellOnCooldown("Immolation Aura"));
			WoW.CastSpell("Spirit Bomb", !WoW.IsSpellOnCooldown("Spirit Bomb") && WoW.PlayerHasBuff("Soul Fragments") && (!WoW.TargetHasDebuff("Frailty") || WoW.TargetDebuffTimeRemaining("Frailty") <= 200));
			WoW.CastSpell("Soul Cleave", WoW.PlayerHealthPercent < 80 && !WoW.PlayerHasBuff("Soul Fragments") && WoW.Pain >= 70);
			WoW.CastSpell("Soul Carver", !WoW.IsSpellOnCooldown("Soul Carver") && WoW.TargetHasDebuff("Frailty") && WoW.TargetDebuffTimeRemaining("Frailty") > 1000);
			WoW.CastSpell("Sigil of Flame", !WoW.IsSpellOnCooldown("Sigil of Flame") && !WoW.TargetHasDebuff("Sigil of Flame"));
			if (WoW.Pain < 70)
			{
				WoW.CastSpell("Shear", !WoW.PlayerHasBuff("Metamorphasis"));
				WoW.CastSpell("Sever", WoW.PlayerHasBuff("Metamorphasis"));
			}
			WoW.CastSpell("Soul Cleave", WoW.PlayerHealthPercent < 35);
			WoW.CastSpell("Fracture", true);
		}
		
		private void BombRotation()
		{
			WoW.CastSpell("Soul Carver", true);
			WoW.CastSpell("Fiery Brand", !WoW.TargetHasDebuff("Fiery Demise") && !WoW.IsSpellOnCooldown("Fiery Brand"));
			WoW.CastSpell("Immolation Aura", true);
			WoW.CastSpell("Shear", WoW.Pain < 30, !WoW.PlayerHasBuff("Metamorphasis")); // generate pain
			WoW.CastSpell("Sever", WoW.Pain < 30, WoW.PlayerHasBuff("Metamorphasis"));
			WoW.CastSpell("Fracture", !WoW.PlayerHasBuff("Soul Fragments") || WoW.PlayerBuffStacks("Soul Fragments") <= 3);
			WoW.CastSpell("Soul Cleave", WoW.PlayerHealthPercent < 50);
			WoW.CastSpell("Spirit Bomb", WoW.PlayerHasBuff("Soul Fragments") && WoW.PlayerBuffStacks("Soul Fragments") >= 4);
			WoW.CastSpell("Sigil of Flame", !WoW.TargetHasDebuff("Sigil of Flame"));
			WoW.CastSpell("Shear", !WoW.PlayerHasBuff("Metamorphasis")); // filler
			WoW.CastSpell("Sever", WoW.PlayerHasBuff("Metamorphasis"));
		}
    }
}

/*
[AddonDetails.db]
AddonAuthor=1chu
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,203782,Shear,T
Spell,235964,Sever,T
Spell,228477,Soul Cleave,Y
Spell,207407,Soul Carver,H
Spell,247454,Spirit Bomb,Z
Spell,204021,Fiery Brand,F
Spell,209795,Fracture,G
Spell,178740,Immolation Aura,D5
Spell,204513,Sigil of Flame,D6
Spell,204157,Throw Glaive,D7
Aura,212818,Fiery Demise
Aura,200175,Mount
Aura,207472,Magnum Opus
Aura,187827,Metamorphasis
Aura,204598,Sigil of Flame
Aura,203981,Soul Fragments
Aura,247456,Frailty
Aura,234143,Temptation
*/
