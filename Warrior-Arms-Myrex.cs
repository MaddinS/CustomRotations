using System.Drawing;
using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class ArmsWarriorLe : CombatRoutine
    {
        public override string Name => "Arms Warrior";

        public override string Class => "Warrior";

        public override Form SettingsForm { get; set; }

		const string rendsupportedTalents = "1322322";
		
        public override void Initialize()
        {
            Log.Write("Welcome to Arms Warrior", Color.Green);
            Log.Write("Suggested build: Rend Build {rendsupportedTalents}", Color.Green);
            Log.Write("Written based on skyhold guide ", Color.Green);
        }

        public override void Stop()
        {
        }
				
        public override void Pulse()
        {
			
			//Talent Checker
				string currentTalents = WoW.Talent(1) + "" + WoW.Talent(2) + "" + WoW.Talent(3) + "" + WoW.Talent(4) + "" + WoW.Talent(5) + "" + WoW.Talent(6) + "" + WoW.Talent(7);
                
				if (WoW.Talent(3) != 2 && WoW.Talent(3) != 1)
                {
                    Log.Write("Talents are not 1322322", Color.Green);
					return;
                }
				
            
			if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
			            {
               
				 //Build one: Rend
				if (WoW.Talent(3) == 2 && WoW.IsSpellInRange("Slam") && WoW.TargetIsEnemy && WoW.IsInCombat && WoW.CountEnemyNPCsInRange < 8 )
					
				{
					if (WoW.TargetHealthPercent > 20)
					{
						if (WoW.CanCast("Rend") 
						&& !WoW.TargetHasDebuff("Rend") || WoW.TargetDebuffTimeRemaining("Rend") <= 30)
						{
							WoW.CastSpell("Rend");
							
						}
						
						if (WoW.CanCast("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") &&
                            !WoW.IsSpellOnCooldown("Colossus Smash"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            
                        }
						
						//MS 
                        if (WoW.CanCast("Mortal Strike") && !WoW.IsSpellOnCooldown("Mortal Strike") && WoW.PlayerHasBuff("Shattered Defenses"))
                        {
                            WoW.CastSpell("Mortal Strike");
                            
                        }
                        //Slam
                        if (WoW.CanCast("Slam") && WoW.Rage >= 18 && WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.CountEnemyNPCsInRange == 1)
                        {
                            WoW.CastSpell("Slam");
                            return;
                        }
						if (WoW.Rage >= 36 && WoW.CountEnemyNPCsInRange >= 2)
						{
						if (WoW.CanCast("Cleave") && !WoW.PlayerHasBuff("Cleave") && !WoW.IsSpellOnCooldown("Cleave") && WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses"))
                            {
                                WoW.CastSpell("Cleave");
                                return;
                            }
						
						if (WoW.CanCast("Whirlwind") && WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses")&& WoW.IsSpellOnCooldown("Cleave"))
						{
							WoW.CastSpell("Whirlwind");
							return;
						}
						}
						if (WoW.CanCast("Execute") && WoW.IsSpellOverlayed("Execute"))
                        {
                            WoW.CastSpell("Execute");
                            return;
                        }
					}
					if (WoW.TargetHealthPercent <= 20)
					{
					
					//MS if you have EP x2 and SD
					if (WoW.CanCast("Mortal Strike") 
						&& !WoW.IsSpellOnCooldown("Mortal Strike") 
						&& WoW.PlayerHasBuff("Shattered Defenses") 
						&& WoW.TargetHasDebuff("ExecutionerÃ¢â‚¬â„¢s Precision") 
						&& WoW.TargetDebuffStacks("ExecutionerÃ¢â‚¬â„¢s Precision") == 2)
                        {
                            WoW.CastSpell("Mortal Strike");
                            return;
                        }
					//CS if no SD
					if (WoW.CanCast("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") &&
                            !WoW.IsSpellOnCooldown("Colossus Smash"))
                        {
                            WoW.CastSpell("Colossus Smash");
                            return;
                        }
					// Execute
					if (WoW.CanCast("Execute"))
                        {
                            WoW.CastSpell("Execute");
                            return;
                        }
					//OP if talented

				
					}
				
				}
				
				//end of rend
				if (WoW.Talent(5) == 1)
				{
					
				}
				//Build two: Trauma/FoB
				
						
			if (WoW.CountEnemyNPCsInRange >= 8)
				{
					if (WoW.CanCast("Cleave") && !WoW.PlayerHasBuff("Cleave") && !WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Cleave");
                                return;
                            }
                    if (WoW.CanCast("Whirlwind") && WoW.Rage > 27 && WoW.IsSpellOnCooldown("Cleave"))
                            {
                                WoW.CastSpell("Whirlwind");
                                return;
                            }
                }
		}
	
            if (combatRoutine.Type == RotationType.AOE)
            {
                // AOE stuff here
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
AddonName=rotationface
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,12294,Mortal Strike,D1
Spell,167105,Colossus Smash,D2
Spell,1464,Slam,D3
Spell,207982,Focused Rage,D4
Spell,1719,Battle Cry,F2
Spell,107574,Avatar,F4
Spell,163201,Execute,D5
Spell,100,Charge,E
Spell,209577,Warbreaker,F1
Spell,845,Cleave,F3
Spell,1680,Whirlwind,D8
Spell,227847,Bladestorm,D9
Spell,772,Rend,F4
Aura,227847,Bladestorm
Aura,208086,Colossus Smash
Aura,209706,Shattered Defenses
Aura,1719,Battle Cry
Aura,207982,Focused Rage
Aura,845,Cleave
Aura,772,Rend
Aura,242188,ExecutionerÃ¢â‚¬â„¢s Precision
*/
