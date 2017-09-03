// winifix@gmail.com
// ReSharper disable UnusedMember.Global


using System.Drawing;
using System.Windows.Forms;
using Frozen.GUI;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class Priest_Holy : CombatRoutine
    {
     
        public override Form SettingsForm
        {
            get { return null; }

            set { }
        }


        const string supportedTalents = "1111111";

        public override void Initialize()
        {
            Log.Clear();
            Log.WriteFrozen("Welcome to Frozen Holy", Color.Black);
            Log.Write("Supported Talents: {supportedTalents}");
            Log.Write("Ensure you have setup healing keybinds before.", Color.Red);
            
            Log.DrawHorizontalLine();
            Log.Write("If you do proving grounds the Tank Id is 5 when it asks you.", Color.Red);
            Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
           
            /*if (WoW.TankId == 0)
            {
                string currentTalents = WoW.Talent(1) + "" + WoW.Talent(2) + "" + WoW.Talent(3) + "" + WoW.Talent(4) + "" + WoW.Talent(5) + "" + WoW.Talent(6) + "" + WoW.Talent(7);
                if (supportedTalents != currentTalents)
                {
                    MessageBox.Show($"You are not using the supported talents {supportedTalents}, your currnet talents are {currentTalents}.", "Frozen",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Log.Write("Talents are correct", Color.Green);
                }

                

                var f = new frmEnterTankId {TopMost = true};
                f.ShowDialog();
            }*/
            
            if (WoW.HealthPercent == 0 || WoW.IsMounted) return;
            if (WoW.PlayerIsCasting) return;

            var lowest = WoW.PartyLowestHealthPercent;

            int currentTargetId = WoW.PartyMemberIdWithLowestHealthPercent;
            
            if (WoW.PartyMemberIsNeedingADispel != 0)
            {
                currentTargetId = WoW.PartyMemberIsNeedingADispel;
            }

            if (currentTargetId == 0) return;
            if (lowest == 100) return;

           

            var averageHp = WoW.PartyAverageHealthPercent;
			
			
			
			
            //AoE Healing
            if (averageHp < 40 && WoW.CanCast("Divine Hymn"))
                WoW.CastSpell("Divine Hymn");

            if (averageHp < 60 && WoW.CanCast("Apotheosis"))
                WoW.CastSpell("Apotheosis");


            WoW.TargetMember(currentTargetId); // Target the lowest health party member for all abilities below

            if (averageHp < 80 && WoW.CanCast("Prayer of Healing")&& lowest > 80)
            { WoW.CastSpell("Prayer of Healing");
                return;

            }
           
            
            if (WoW.PartyMemberIsNeedingADispel != 0 && WoW.CanCast("Purify"))
            {
                WoW.CastSpell("Purify");
                return;
            }

            if (WoW.CanCast("Guardian Spirit") &&
                lowest <= 20 && // Use LOH if tanks health < 20 %
                !WoW.IsSpellOnCooldown("Guardian Spirit") && // Make sure we dont try and use it whilst on cooldown
                WoW.TankId == currentTargetId) // Make sure we only use LOH on tank
            {
                WoW.CastSpell("Guardian Spirit");
                return;
            }
			if (WoW.CanCast("Holy Word Serenity") &&
                lowest <= 50 && // Use LOH if tanks health < 20 %
                !WoW.IsSpellOnCooldown("Holy Word Serenity")) // Make sure we dont try and use it whilst on cooldown
            { 
                WoW.CastSpell("Holy Word Serenity");
                return;
            }
			
			 // Flash of Light use as an emergency heal to save players facing death. 
            if (WoW.CanCast("Flash Heal") && lowest <= 50 && !WoW.IsMoving)
            {
                WoW.CastSpell("Flash Heal");
                return;
            }

            // Holy Light use to heal moderate to high damage. 
            if (WoW.CanCast("Heal") && lowest <= 80 && !WoW.IsMoving)
            {
                WoW.CastSpell("Heal");
                return;
            }

            if (WoW.CanCast("Light of Tuure") &&
              lowest <= 85 && 
              !WoW.IsSpellOnCooldown("Light of Tuure") &&
			  !WoW.TargetHasBuff("Light of Tuure")) // Make sure we dont try and use it whilst on cooldown
			  
            { 
                WoW.CastSpell("Light of Tuure");
                return;
            }

            if (WoW.CanCast("Renew") &&
             lowest <= 90 && // Make sure we dont try and use it whilst on cooldown
             WoW.IsMoving)
            {
                WoW.CastSpell("Renew");
                return;
            }

            if (WoW.CanCast("Prayer of Mending") &&
             lowest <= 95 &&
             !WoW.IsSpellOnCooldown("Prayer of Mending") && // Make sure we dont try and use it whilst on cooldown
             !WoW.TargetHasBuff("Prayer of Mending")) // Make sure we only use PoM on tank
            {
                WoW.CastSpell("Prayer of Mending");
                return;
            }

            
            
          
            
           
			
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Anna
AddonName=rotationface
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,2050,Holy Word Serenity,E
Spell,34861,Holy Word Sanctify,D8
Spell,33076,Prayer of Mending,D2
Spell,2061,Flash Heal,D5
Spell,596,Prayer of Healing,D6
Spell,64843,Divine Hymn,D0
Spell,2060,Heal,D4
Spell,139,Renew,D1
Spell,47788,Guardian Spirit,F8
Spell,200183,Apotheosis,D9
Spell,208065,Light of Tuure,D7
Spell,527,Purify,F7
Aura,139,Renew
Aura,41635,Prayer of Mending
Aura,47788,Guardian Spirit
Aura,208065,Light of Tuure
Dispel,145206,Aqua Bomb,1
*/
