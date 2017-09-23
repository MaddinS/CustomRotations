// winifix@gmail.com
// ReSharper disable UnusedMember.Global

// Original Rotation by Lesion
// Reccomended Talent Build 2223131

using Frozen.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Frozen.GUI;

namespace Frozen.Rotation
{
    public class ArcaneMage : CombatRoutine
    {
        public override Form SettingsForm { get; set; }

		
		private bool validtarget;
		
        public override void Initialize()
        {
            Log.Write("Icy Veins Arcane Mage Rotation", Color.Green);
            Log.Write("Welcome to Frozen");
			Log.Write("Use Talents 2223131")
            Log.Write("Based on original code by Myrex");
        }

        public override void Stop()
        {
        }

		public override void OutOfCombatPulse()
        {
			
            
        }
		
        public override void Pulse()
        {
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
				validtarget = WoW.HasTarget && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling && !WoW.PlayerHasBuff("Greater Invisibility") && !WoW.TargetHasDebuff("Polymorph") &&  WoW.RangeToTarget < 41;
                
                // Prismatic Barrier if health < 80%
                WoW.CastSpell("Prismatic Barrier", WoW.PlayerHealthPercent <= 80 && !WoW.PlayerHasBuff("Prismatic Barrier"));
                // Arcane Familiar if not up
                WoW.CastSpell("Arcane Familiar", WoW.Talent(1) == 1 && !WoW.PlayerIsCasting && !WoW.PlayerIsChanneling && !WoW.PlayerHasBuff("Arcane Familiar"));
                               
                // Single Target Combat Begins - No casting if Invisible or Target is Polymorphed
                if (validtarget)
                {
					
					// Arcane Barrage when moving without Ice Floes
                    WoW.CastSpell("Arcane Barrage", WoW.CanCast("Arcane Barrage") && WoW.IsMoving && !WoW.PlayerHasBuff("Ice Floes") && WoW.PlayerBuffStacks("Arcane Missiles") == 0);
                    
                    //Burn Phase
					//----------------------------------------------------------
					
                    // BURN BEGIN - Only use Burn if Burn is selected and Arcane Power is available or active
                    // Charged Up if zero arcane charges
					if (UseCooldowns)
					{
						// BURN - Arcane Missle if you can
						WoW.CastSpell("Arcane Missiles", WoW.PlayerHasBuff("Arcane Missiles!") && WoW.PlayerBuffStacks("Arcane Missiles!") == 3, true);
						WoW.CastSpell("Arcane Missiles", WoW.LastSpell == ("Arcane Missiles") && WoW.PlayerHasBuff("Arcane Missiles!") && WoW.PlayerBuffStacks("Arcane Missiles!") >= 1, true);
						// Mark Of Aluneth is up
						WoW.CastSpell("Mark Of Aluneth", WoW.CanCast("Mark Of Aluneth"));	
						// Cast Arcane Blast if < 4 charges
						WoW.CastSpell("Arcane Blast", WoW.CurrentArcaneCharges < 4);
						// BURN - Rune of Power at 4 Arcane Blast Charges
						WoW.CastSpell("Rune of Power", !WoW.PlayerHasBuff("Rune of Power") && WoW.CurrentArcaneCharges == 4);
                    	
						 // BURN - Rune of Power if >30% mana
						WoW.CastSpell("Rune of Power", WoW.PlayerHasBuff("Arcane Power") && !WoW.PlayerHasBuff("Rune of Power") && WoW.Mana >= 30);
                        // BURN - Arcane Power if Rune of Power down and 4 Arcane Blast Charges
						WoW.CastSpell("Arcane Power", WoW.PlayerHasBuff("Rune of Power") && WoW.CurrentArcaneCharges == 4);
						
						// Evocate if low on mana
						WoW.CastSpell("Evocation", WoW.Mana <= 5); 
						 // Arcane Blast otherwise
                    WoW.CastSpell("Arcane Blast", validtarget);

					}
                    // BURN END
					// Conserve Begin
                    if (!UseCooldowns)
					{
                    WoW.CastSpell("Rune of Power", WoW.PlayerSpellCharges("Rune of Power") == 2);
                    // Mark Of Aluneth is up - If no Rune of Power...cast Rune of Power - If Rune of Power...cast Mark Of Aluneth
                    WoW.CastSpell("Rune of Power", WoW.CanCast("Mark Of Aluneth") && !WoW.PlayerHasBuff("Rune of Power"));
                    WoW.CastSpell("Mark Of Aluneth", WoW.CanCast("Mark Of Aluneth") && WoW.PlayerHasBuff("Rune of Power"));
                    
                   
                    // Arcane Missles with 4 Arcane Blast Charges
                    WoW.CastSpell("Arcane Missiles", WoW.PlayerHasBuff("Arcane Missiles!") && WoW.CurrentArcaneCharges == 4);
                    // Arcane Missles with 3 stacks of Arcane Missiles! Buff - Not sure how to check buff stacks *NEED FIXED* 
                    WoW.CastSpell("Arcane Missiles", WoW.PlayerHasBuff("Arcane Missiles!") && WoW.PlayerBuffStacks("Arcane Missiles!") == 3);
					
                    WoW.CastSpell("Arcane Barrage", WoW.Mana < 55);
                    // Arcane Blast otherwise
                    WoW.CastSpell("Arcane Blast", validtarget);
                    
					}
                // SingleTarget Combat Ends
            }
			}
                

            if (combatRoutine.Type == RotationType.AOE)
            {
                // Do Single Target Cleave stuff here if applicable else ignore this one
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
AddonAuthor=Lesion
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,235450,Prismatic Barrier,S
Spell,205022,Arcane Familiar,D6
Spell,30451,Arcane Blast,D1
Spell,114923,Nether Tempest,E
Spell,12051,Evocation,F
Spell,5143,Arcane Missiles,D3
Spell,157980,Supernova,D7
Spell,116011,Rune of Power,Q
Spell,224968,Mark Of Aluneth,D4 
Spell,44425,Arcane Barrage,D2
Spell,12042,Arcane Power,A
Spell,157801,Slow,X
Spell,108839,Ice Floes,D8
Spell,153626,Arcane Orb,None
Spell,205032,Charged Up,None
Aura,110960,Greater Invisibility
Aura,157801,Slow
Aura,205022,Arcane Familiar
Aura,114923,Nether Tempest
Aura,79683,Arcane Missiles!
Aura,116014,Rune of Power
Aura,235450,Prismatic Barrier
Aura,108839,Ice Floes
Aura,118,Polymorph
Aura,205032,Charged Up
Aura,12042,Arcane Power
*/
