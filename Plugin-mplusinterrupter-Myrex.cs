using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Plugins
{
    public class InterruptPlugz : Plugin
    {
        private readonly Stopwatch InterruptDelay = new Stopwatch();
		
		public override string Name
        {
            get
            {
                return "Interrupt Plugin";
            }
        } 
		
		public override Form SettingsForm { get; set; }
        		
		private bool validtarget;
		private bool interrupter;
		private bool interrupterCD;

        public override void Initialize()
        {
           Log.Write("M+ interrupts enabled");
        }

        public override void Stop()
        {
            Log.Write("No longer interrupting");
        }

        public override void Pulse()
        {
			if (!WoW.TargetIsCastingAndSpellIsInterruptible)
			{
				return;
			}
			// Pummel, Mind Freeze, Consume Magic, Skull Bash, Counter Shot, Muzzle, Counterspell, Spear Hand Strike, Rebuke, Silence, Kick, Wind Shear, Spell Lock
			interrupter = WoW.CanCast("Pummel", false, true, false, false, false) || WoW.CanCast("Mind Freeze", false, true, false, false, false) || WoW.CanCast("Consume Magic", false, true, false, false, false) || WoW.CanCast("Skull Bash", false, true, false, false, false) || WoW.CanCast("Counter Shot", false, true, false, false, false) || WoW.CanCast("Spell Lock", false, true, false, false, false) || WoW.CanCast("Wind Shear", false, true, false, false, false) || WoW.CanCast("Kick", false, true, false, false, false) || WoW.CanCast("Silence", false, true, false, false, false) || WoW.CanCast("Rebuke", false, true, false, false, false) || WoW.CanCast("Spear Hand Strike", false, true, false, false, false) || WoW.CanCast("Counterspell", false, true, false, false, false) || WoW.CanCast("Muzzle", false, true, false, false, false);
            //interrupterCD = WoW.IsSpellOnCooldown("Pummel") || WoW.IsSpellOnCooldown("Mind Freeze") || WoW.IsSpellOnCooldown("Consume Magic") || WoW.IsSpellOnCooldown("Skull Bash") || WoW.IsSpellOnCooldown("Counter Shot") || WoW.IsSpellOnCooldown("Spell Lock") || WoW.IsSpellOnCooldown("Wind Shear") || WoW.IsSpellOnCooldown("Kick") || WoW.IsSpellOnCooldown("Silence") || WoW.IsSpellOnCooldown("Rebuke") || WoW.CanCast("Spear Hand Strike") || WoW.CanCast("Counterspell") || WoW.CanCast("Muzzle");
			validtarget = WoW.HasTarget && WoW.IsInCombat && !WoW.PlayerIsChanneling && WoW.TargetIsVisible;
			
          
		   if (interrupter && WoW.TargetIsCasting && WoW.TargetIsCastingAndSpellIsInterruptible)
                {
                    //int spell list for all important spells in M+                        
                    if (WoW.TargetCastingSpellID == 200248
                        //Court Of Stars Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 225573 || WoW.TargetCastingSpellID == 208165 || WoW.TargetCastingSpellID == 211401 || WoW.TargetCastingSpellID == 21147 ||
                        WoW.TargetCastingSpellID == 211299 || WoW.TargetCastingSpellID == 2251 || WoW.TargetCastingSpellID == 209413 || WoW.TargetCastingSpellID == 209404 ||
                        WoW.TargetCastingSpellID == 215204 || WoW.TargetCastingSpellID == 210261
                        //Darkheart Thicket Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 200658 || WoW.TargetCastingSpellID == 200631 || WoW.TargetCastingSpellID == 204246 || WoW.TargetCastingSpellID == 2014
                        //Eye of Azshara Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 19687 || WoW.TargetCastingSpellID == 218532 || WoW.TargetCastingSpellID == 195129 || WoW.TargetCastingSpellID == 195046 ||
                        WoW.TargetCastingSpellID == 197502 || WoW.TargetCastingSpellID == 196027 || WoW.TargetCastingSpellID == 196175 || WoW.TargetCastingSpellID == 192003 ||
                        WoW.TargetCastingSpellID == 191848
                        //Halls of Valor Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 198595 || WoW.TargetCastingSpellID == 198962 || WoW.TargetCastingSpellID == 198931 || WoW.TargetCastingSpellID == 192563 ||
                        WoW.TargetCastingSpellID == 192288 || WoW.TargetCastingSpellID == 199726
                        //Maw of Souls Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 198495 || WoW.TargetCastingSpellID == 195293 || WoW.TargetCastingSpellID == 199589 || WoW.TargetCastingSpellID == 194266 ||
                        WoW.TargetCastingSpellID == 198405 || WoW.TargetCastingSpellID == 199514 || WoW.TargetCastingSpellID == 194657
                        //Neltharions Lair Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 193585 || WoW.TargetCastingSpellID == 202181
                        //The Arcway Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 226269 || WoW.TargetCastingSpellID == 211007 || WoW.TargetCastingSpellID == 211757 || WoW.TargetCastingSpellID == 226285 ||
                        WoW.TargetCastingSpellID == 226206 || WoW.TargetCastingSpellID == 211115 || WoW.TargetCastingSpellID == 196392
                        // Advisor Vandros (Interrupt manually) Spell,203176,Accelerating Blast
                        || WoW.TargetCastingSpellID == 203957
                        //Vault of the Wardens Mythic+ Interrupt list
                        || WoW.TargetCastingSpellID == 193069 || WoW.TargetCastingSpellID == 191823 || WoW.TargetCastingSpellID == 202661 || WoW.TargetCastingSpellID == 201488 ||
                        WoW.TargetCastingSpellID == 195332
                    //Raid Interrupts
                    || WoW.TargetCastingSpellID == 209485 || WoW.TargetCastingSpellID == 209410 || WoW.TargetCastingSpellID == 211470 || WoW.TargetCastingSpellID == 225100 ||
                    WoW.TargetCastingSpellID == 207980 || WoW.TargetCastingSpellID == 196870 || WoW.TargetCastingSpellID == 195284 || WoW.TargetCastingSpellID == 192005 ||
                    WoW.TargetCastingSpellID == 228255 || WoW.TargetCastingSpellID == 228239 || WoW.TargetCastingSpellID == 227917 || WoW.TargetCastingSpellID == 228625 ||
                    WoW.TargetCastingSpellID == 228606 || WoW.TargetCastingSpellID == 229714 || WoW.TargetCastingSpellID == 227592 || WoW.TargetCastingSpellID == 229083 ||
                    WoW.TargetCastingSpellID == 228025 || WoW.TargetCastingSpellID == 228019 || WoW.TargetCastingSpellID == 227987 || WoW.TargetCastingSpellID == 227420 ||
                    WoW.TargetCastingSpellID == 200905)

                    {
                        if (interrupter && WoW.TargetPercentCast >= 40)
                        {
							//Warrior
                            if (WoW.PlayerClassSpec == "Warrior-Protection" || WoW.PlayerClassSpec == "Warrior-Fury" || WoW.PlayerClassSpec == "Warrior-Arms")
                            {
                                WoW.CastSpell("Pummel");
								return;
                            }
							
							//Dk
                            if (WoW.PlayerClassSpec == "DeathKnight-Blood" || WoW.PlayerClassSpec == "DeathKnight-Frost" || WoW.PlayerClassSpec == "DeathKnight-Unholy")
                            {
                                WoW.CastSpell("Mind Freeze");
								return;
                            }
							//DH
                            if (WoW.PlayerClassSpec == "DemonHunter-Vengeance" || WoW.PlayerClassSpec == "DemonHunter-Havoc")
                            {
                                WoW.CastSpell("Consume Magic");
								return;
                            }
							
							//Drooid
							if (WoW.PlayerClassSpec == "Druid-Feral" || WoW.PlayerClassSpec == "Druid-Guardian")
                            {
                                WoW.CastSpell("Skull Bash");
								return;
                            }
							
							//manually as its an AoE interrupt
							/*if (WoW.PlayerClassSpec == "Druid-Balance")
                            {
                                WoW.CastSpell("Consume Magic");
								return;
                            }*/
							//Paladin
							if (WoW.PlayerClassSpec == "Paladin-Protection" || WoW.PlayerClassSpec == "Paladin-Retribution")
                            {
                                WoW.CastSpell("Rebuke");
								return;
                            }
							
							//Monk
							if (WoW.PlayerClassSpec == "Monk-Brewmaster" || WoW.PlayerClassSpec == "Monk-Windwalker")
                            {
                                WoW.CastSpell("Spear Hand Strike");
								return;
                            }
							
							//Priest
							if (WoW.PlayerClassSpec == "Priest-Shadow")
                            {
                                WoW.CastSpell("Silence");
								return;
                            }
							//Mage
							if (WoW.PlayerClassSpec == "Mage-Frost" || WoW.PlayerClassSpec == "Mage-Arcane" || WoW.PlayerClassSpec == "Mage-Fire")
                            {
                                WoW.CastSpell("Counterspell");
								return;
                            }
							//Warlock
							//Hunter
							if (WoW.PlayerClassSpec == "Hunter-Marksman" || WoW.PlayerClassSpec == "Hunter-Beastmastery")
                            {
                                WoW.CastSpell("Counter Shot");
								return;
                            }
							if (WoW.PlayerClassSpec == "Hunter-Survival")
                            {
                                WoW.CastSpell("Muzzle");
								return;
                            }
							//Rogue
							if (WoW.PlayerClassSpec == "Rogue-Assassination" || WoW.PlayerClassSpec == "Rogue-Outlaw" || WoW.PlayerClassSpec == "Rogue-Subtlety")
                            {
                                WoW.CastSpell("Kick");
								return;
                            }
							//Shaman
							if (WoW.PlayerClassSpec == "Shaman-Elemental" || WoW.PlayerClassSpec == "Shaman-Enhancement")
                            {
                                WoW.CastSpell("Wind Shear");
								return;
                            }

                        }

                    }
                }
		  
		  
		  
		  
		  
		  
		  
		  
        }
    }
}
