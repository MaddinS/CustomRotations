using System.Windows.Forms;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class HunterBeastmasteryMooTauren : CombatRoutine
    {
        public override string Name => "Hunter BM";

        public override string Class => "Hunter";

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("### Hunter BM (MooTauren) ###");
            Log.Write("### Talents: 3-2-x-x-x-1-3 ###");
        }

        public override void Stop()
        {
        }

        private int GCD => (int)(150 / (1 + (double)WoW.HastePercent / 100));
        private double FocusRegenRate => ((double)WoW.HastePercent / 100 + 1) * 10;
        private int FocusTimeToMax => (int) ((120 - WoW.Focus) / FocusRegenRate * 100);

        public override void Pulse()
        {
            if (WoW.HasTarget && WoW.IsInCombat)
            {
                // Log.Write("### FocusTimeToMax - " + FocusTimeToMax + " ... RegenRate - " + FocusRegenRate);

                //actions +=/ a_murder_of_crows,if= cooldown.bestial_wrath.remains < 3 | cooldown.bestial_wrath.remains > 30 | target.time_to_die < 16
                if (WoW.SpellCooldownTimeRemaining("BestialWrath") < 300 || WoW.SpellCooldownTimeRemaining("BestialWrath") > 3000)
                {
                    if (WoW.CanCast("AMurderofCrows")) { WoW.CastSpell("AMurderofCrows"); return;}
                }

                //actions +=/ stampede,if= buff.bloodlust.up | buff.bestial_wrath.up | cooldown.bestial_wrath.remains <= 2 | target.time_to_die <= 14
                //actions +=/ dire_beast,if= cooldown.bestial_wrath.remains > 3

                //actions +=/ dire_frenzy,if= (pet.cat.buff.dire_frenzy.remains <= gcd.max * 1.2) | (charges_fractional >= 1.8) | target.time_to_die < 9
                if (WoW.PetBuffTimeRemaining("DireFrenzy") <= GCD * 1.2 || WoW.PlayerSpellCharges("DireFrenzy") >= 2)
                {
                    if (WoW.CanCast("DireFrenzy")) { WoW.CastSpell("DireFrenzy"); return; }
                }

                //actions +=/ aspect_of_the_wild,if= buff.bestial_wrath.remains > 7 | target.time_to_die < 12
                if (WoW.CooldownsOn)
                {
                    if (WoW.PlayerBuffTimeRemaining("BestialWrath") > 700)
                    {
                        if (WoW.CanCast("AspectoftheWild")) { WoW.CastSpell("AspectoftheWild"); return; }
                    }
                }

                //actions +=/ barrage,if= spell_targets.barrage > 1

                //actions +=/ bestial_wrath
                if (WoW.CooldownsOn)
                {
                    if (WoW.CanCast("BestialWrath")) { WoW.CastSpell("BestialWrath"); return; }
                }

                //actions +=/ titans_thunder,if= (talent.dire_frenzy.enabled & (buff.bestial_wrath.up | cooldown.bestial_wrath.remains > 35)) | cooldown.dire_beast.remains >= 3 | (buff.bestial_wrath.up & pet.dire_beast.active)
                if (WoW.PlayerHasBuff("BestialWrath") || WoW.SpellCooldownTimeRemaining("BestialWrath") > 3500)
                {
                    if (WoW.CanCast("TitansThunder")) { WoW.CastSpell("TitansThunder"); return; }
                }

                //actions +=/ multishot,if= spell_targets > 4 & (pet.cat.buff.beast_cleave.remains < gcd.max | pet.cat.buff.beast_cleave.down)

                //actions +=/ kill_command
                if (WoW.CanCast("KillCommand")) { WoW.CastSpell("KillCommand"); return; }

                //actions +=/ multishot,if= spell_targets > 1 & (pet.cat.buff.beast_cleave.remains < gcd.max * 2 | pet.cat.buff.beast_cleave.down)
                //actions +=/ chimaera_shot,if= focus < 90

                //actions +=/ cobra_shot,if= (cooldown.kill_command.remains > focus.time_to_max & cooldown.bestial_wrath.remains > focus.time_to_max) | (buff.bestial_wrath.up & focus.regen * cooldown.kill_command.remains > action.kill_command.cost) | target.time_to_die < cooldown.kill_command.remains | (equipped.parsels_tongue & buff.parsels_tongue.remains <= gcd.max * 2)
                if ((WoW.SpellCooldownTimeRemaining("KillCommand") > FocusTimeToMax && (WoW.CooldownsOn ? WoW.SpellCooldownTimeRemaining("BestialWrath") : 60000) > FocusTimeToMax) || (WoW.PetHasBuff("BestialWrath") && FocusRegenRate * (WoW.CooldownsOn ? WoW.SpellCooldownTimeRemaining("BestialWrath") : 60000) > 3000) )
                {
                    if (WoW.CanCast("CobraShot")) { WoW.CastSpell("CobraShot"); return; }
                }

                
            }
        }

    }
}

/*
[AddonDetails.db]
AddonAuthor=MooTauren
AddonName=Frozen
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,193455,CobraShot, D1
Spell,217200,DireFrenzy, D2
Spell,34026,KillCommand, D3
Spell,131894,AMurderofCrows, D4
Spell,2643,MultiShot,D5
Spell,207068,TitansThunder,D6
Spell,19574,BestialWrath,D7
Spell,193530,AspectoftheWild,D8
Aura,217200,DireFrenzy
Aura,19574,BestialWrath
*/
