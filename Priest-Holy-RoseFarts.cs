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
        public override string Name => "Holy-Moly";

        public override string Class => "Priest";

        public override Form SettingsForm
        {
            get { return null; }

            set { }
        }


        public override void Initialize()
        {
            Log.Clear();
            Log.WriteFrozen("Welcome to Holy Priest by RoseFarts", Color.Black);
            Log.WriteFrozen("Thanks to @ Myrex for some inspiration", Color.Black);
            Log.WriteFrozen("Recommanded Talents: 1111111", Color.Black);
            Log.DrawHorizontalLine();
            Log.Write("If you do proving grounds the Tank Id is 5 when it asks you.", Color.Red);
            Log.DrawHorizontalLine();
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {

            if (WoW.HealthPercent == 0 || WoW.IsMounted) return;
            if (WoW.PlayerIsCasting) return;

            var TargetHp = WoW.PartyLowestHealthPercent;

            int TargetId = WoW.PartyMemberIdWithLowestHealthPercent;

            /*
            if (WoW.PartyMemberIsNeedingADispel != 0 &&
                WoW.CanCast("Purify"))
            {
                WoW.TargetMember(WoW.PartyMemberIsNeedingADispel);
                WoW.CastSpell("Purify");
                return;
            }
            */

            if (TargetId == 0) return;
            if (TargetHp == 100) return;

            var AverageHp = WoW.PartyAverageHealthPercent;

            //Damage when CleaveON toggle activated or when nothing to do (averageHp > 97)
            if (WoW.CleaveOn == true &&
				!WoW.IsMoving)
            {
                WoW.TargetNearestEnemy();
                if (WoW.CanCast("Holy Fire"))
                {
                    WoW.CastSpell("Holy Fire");
                }

                WoW.CastSpell("Smite");
                return;
            }

            //AoE Healing Cooldowns
            if (WoW.CanCast("Divine Hymn") &&
                AverageHp < 40)
            {
                WoW.CastSpell("Divine Hymn");
            }

            if (WoW.CanCast("Apotheosis") &&
                AverageHp < 60)
            {
                WoW.CastSpell("Apotheosis");
            }

            //Light of Tuure if not already on the target and target low
            if (!WoW.TargetHasBuff("Light of Tuure") &&
                WoW.CanCast("Light of Tuure") &&
                TargetHp <= 50)
            {
                WoW.CastSpell("Light of Tuure");
            }

            // Target the lowest health party member for all abilities below
            WoW.TargetMember(TargetId);

            if (WoW.CanCast("Prayer of Healing") &&
                AverageHp < 85 &&
                TargetHp > 70 &&
                !WoW.IsMoving)
            {
                WoW.CastSpell("Prayer of Healing");
                return;
            }

            // Guardian Spirit on Tank only!
            if (WoW.CanCast("Guardian Spirit") &&
                WoW.TankHealth <= 20)
            {
                WoW.TargetMember(WoW.TankId);
                WoW.CastSpell("Guardian Spirit");

                return;
            }

			//Serenity as a quick boost
            if (WoW.CanCast("Holy Word Serenity") &&
                TargetHp <= 50)
            {
                WoW.CastSpell("Holy Word Serenity");
                return;
            }

            // Flash Heal use on high damage or on Surge of Light proc 
            if ((WoW.CanCast("Flash Heal") && TargetHp <= 51 && !WoW.IsMoving) || 
                (WoW.CanCast("Flash Heal") && TargetHp <= 81 && WoW.PlayerHasBuff("Surge of Light")))
            {
                WoW.CastSpell("Flash Heal");
                return;
            }

            // Heal use to heal moderate to high damage. 
            if (WoW.CanCast("Heal") && 
                TargetHp <= 85 && 
                !WoW.IsMoving)
            {
                WoW.CastSpell("Heal");
                return;
            }

			//Renew if no one needs proper healing and we have the mana to spare
            if (WoW.CanCast("Renew") && 
                !WoW.TargetHasBuff("Renew") &&
                TargetHp <= 95 && 
                WoW.Mana >= 80 &&
                AverageHp >=80)
            {
                WoW.CastSpell("Renew");
                return;
            }

			//Prayer of Medning (almost) on cooldown
            if (WoW.CanCast("Prayer of Mending") &&
                !WoW.IsSpellOnCooldown("Prayer of Mending") &&
                !WoW.TargetHasBuff("Prayer of Mending") &&
                TargetHp < 95)
            {
                WoW.CastSpell("Prayer of Mending");
                return;
            }
        }
    }
}
/*
[AddonDetails.db]
AddonAuthor=RoseFarts
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,2050,Holy Word Serenity,D1
Spell,34861,Holy Word Sanctify,D2
Spell,33076,Prayer of Mending,D3
Spell,2061,Flash Heal,D4
Spell,596,Prayer of Healing,D5
Spell,64843,Divine Hymn,D6
Spell,2060,Heal,D7
Spell,139,Renew,D8
Spell,47788,Guardian Spirit,D9
Spell,200183,Apotheosis,D0
Spell,208065,Light of Tuure,NumPad1
Spell,527,Purify,NumPad2
Spell,14914,Holy Fire,NumPad4
Spell,585,Smite,NumPad5
Aura,139,Renew
Aura,41635,Prayer of Mending
Aura,47788,Guardian Spirit
Aura,208065,Light of Tuure
Aura,114255,Surge of Light
*/
