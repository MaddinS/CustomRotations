using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarlockDemonology : CombatRoutine
    {
        public override string Name => "Demonology Warlock";

        public override string Class => "Warlock";

        public static int WildImps;

        public static int Dreadstalkers;

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
            Log.Write("Welcome to Jarl's Demonology Warlock rotation", Color.Purple);
            if (WoW.Talent(1) != 3 || WoW.Talent(2) != 2 || WoW.Talent(4) != 1 || WoW.Talent(6) != 2 || WoW.Talent(7) != 2){
                Log.Write("Please check your talents! This rotation only supports 3/2/*/1/*/2/2 at the moment!", Color.Red);
            }
            WildImps = 0;
            Dreadstalkers = 0;
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            //Keep a pet out
            if ((!WoW.HasPet || WoW.HasPet && WoW.PetHealthPercent == 0) && !WoW.IsMounted && !WoW.PlayerIsCasting){
                if (WoW.Talent(6) != 1){
                    WoW.CastSpell("Felguard");
                    return;
                }
                if (WoW.Talent(6) == 1){
                    WoW.CastSpell("Doomguard");
                    return;
                }
            }
            // Cast Life Tap when low
            if (WoW.CanCast("Life Tap") && !WoW.IsMounted && WoW.Mana < 40 && WoW.HealthPercent > 40){
                WoW.CastSpell("Life Tap");
                return;
            }

            //Health Funnel
            if (WoW.CanCast("Health Funnel") && WoW.PetHealthPercent <= 30 && !WoW.IsMoving && !WoW.IsMounted && WoW.HasPet){
                WoW.CastSpell("Health Funnel");
                return;
            }

            //Doom
            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.IsMounted && !WoW.TargetHasDebuff("Doom")){
                WoW.CastSpell("Doom");
                return;
            }

            // Cast Demonwrath when in combat and moving
            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && WoW.CanCast("Demonwrath") && WoW.Mana > 40 && WoW.IsMoving){
                WoW.CastSpell("Demonwrath");
                return;
            }

            // Burst mode enabled
            if (WoW.CooldownsOn){
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted && WoW.IsSpellInRange("Doom") && WoW.CurrentSoulShards >= 1){
                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard") && WoW.Talent(6) == 2){
                        WoW.CastSpell("Grimoire: Felguard");
                        return;
                    }

                    //Doomguard
                    if (WoW.CanCast("Doomguard") && (WoW.Talent(6) == 0 || WoW.Talent(6) == 2 || WoW.Talent(6) == 3)){
                        WoW.CastSpell("Doomguard");
                        return;
                    }
                }
            }

            //Single target rotation
            if (WoW.RotationOn && !WoW.AoeOn && !WoW.CleaveOn){
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted && !WoW.IsMoving && WoW.IsSpellInRange("Doom"))
                {
                    int currentSoulShards = WoW.CurrentSoulShards;

                    //If consumption is ready combo up to get more demons out
                    if (WoW.CanCast("Talkiels Consumption")){
                        //Doomguard + Dreadstalkers combo
                        if (WoW.WasLastCasted("Doomguard") && WoW.CanCast("Call Dreadstalkers") && (WoW.PlayerHasBuff("Demonic Calling") || currentSoulShards >= 2)){
                            WoW.CastSpell("Call Dreadstalkers");
                            DreadStalkersSummoned();
                            return;
                        }

                        //Doomguard + Hand of Guldan combo
                        if (WoW.WasLastCasted("Doomguard") && currentSoulShards >= 2 && WoW.CanCast("Hand of Guldan")){
                            if (currentSoulShards > 4){
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(4);
                            }
                            else{
                                WoW.CastSpell("Hand of Guldan");
                                WildImpsSummoned(currentSoulShards);
                            }
                            return;
                        }

                        //Hand of Guldan + Dreadstalkers combo
                        if (WoW.WasLastCasted("Hand of Guldan") && WoW.CanCast("Call Dreadstalkers") && (WoW.PlayerHasBuff("Demonic Calling") || currentSoulShards >= 2)){
                            WoW.CastSpell("Call Dreadstalkers");
                            DreadStalkersSummoned();
                            return;
                        }

                        //Demonic Empowerment
                        if (WoW.CanCast("Demonic Empowerment") && !WoW.WasLastCasted("Demonic Empowerment") && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 200 || WoW.WasLastCasted("Call Dreadstalkers") || WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") || WoW.WasLastCasted("Hand of Guldan"))){
                            WoW.CastSpell("Demonic Empowerment");
                            return;
                        }

                        //Talkiels Consumption combo with Demonic Empowerment
                        if (WoW.WasLastCasted("Demonic Empowerment") && WoW.CanCast("Talkiels Consumption") && TotalDemonsOut() >= 8){
                            WoW.CastSpell("Talkiels Consumption");
                            return;
                        }
                    }
                    

                    //Demonic Empowerment
                    if (WoW.CanCast("Demonic Empowerment") && !WoW.WasLastCasted("Demonic Empowerment") && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 200 || WoW.WasLastCasted("Call Dreadstalkers") || WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") || WoW.WasLastCasted("Hand of Guldan"))){
                        WoW.CastSpell("Demonic Empowerment");
                        return;
                    }

                    //Dreadstalkers
                    if (WoW.CanCast("Call Dreadstalkers") && (currentSoulShards >= 2 || WoW.PlayerHasBuff("Demonic Calling"))){
                        WoW.CastSpell("Call Dreadstalkers");
                        DreadStalkersSummoned();
                        return;
                    }

                    //Hand of Guldan
                    if (WoW.CanCast("Hand of Guldan") && (currentSoulShards >= 4 || (currentSoulShards >= 2 && WoW.TargetDebuffTimeRemaining("Doom") <= 200))){
                        if (currentSoulShards > 4){
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(4);
                        }
                        else{
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(currentSoulShards);
                        }
                        return;
                    }
                    
                    //Shadowbolt/Demonbolt
                    if ((WoW.CanCast("Shadow Bolt") || WoW.CanCast("Demonbolt")) && !WoW.IsMoving && currentSoulShards != 5){
                        WoW.CastSpell("Shadow Bolt");
                        WoW.CastSpell("Demonbolt");
                        return;
                    }
                }
            }
            if (WoW.AoeOn){
                //Felstorm
                if (WoW.CanCast("Felstorm") && WoW.PetHasBuff("Demonic Empowerment") && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6){
                    WoW.CastSpell("Felstorm");
                    return;
                }
            }
            if (WoW.CleaveOn){
                //Felstorm
                if (WoW.CanCast("Felstorm") && WoW.PetHasBuff("Demonic Empowerment") && WoW.PetBuffTimeRemaining("Demonic Empowerment") >= 6){
                    WoW.CastSpell("Felstorm");
                    return;
                }
            }
        }

        public void WildImpsSummoned (int shardsUsed){
            WildImps += shardsUsed;
            RunImpTimer(shardsUsed);
            return;
        }

        public void RunImpTimer(int shardsUsed){
            System.Timers.Timer impTimer = new System.Timers.Timer (12000);
            impTimer.Elapsed += (sender, e) => WildImpsTimedOut(sender, e, shardsUsed);
            impTimer.AutoReset = false;
            impTimer.Enabled = true;
        }

        public void WildImpsTimedOut (object sender, ElapsedEventArgs e, int shardsUsed){
            WildImps -= shardsUsed;
            return;
        }

        public void DreadStalkersSummoned (){
            if (WoW.Talent(2) == 2){
                WildImpsSummoned(2);
            }
            Dreadstalkers += 2;
            RunDreadTimer();
            return;
        }

        public void RunDreadTimer(){
            System.Timers.Timer dreadTimer = new System.Timers.Timer (12000);
            dreadTimer.Elapsed += DreadstalkerTimedOut;
            dreadTimer.AutoReset = false;
            dreadTimer.Enabled = true;
        }

        public void DreadstalkerTimedOut (object sender, ElapsedEventArgs e){
            Dreadstalkers -= 2;
            return;
        }

        public bool GrimoireFelguardOut (){
            if (WoW.IsSpellOnCooldown("Grimoire: Felguard")){
                Log.Write("Grimoire: Felguard CD time remaining: " + WoW.SpellCooldownTimeRemaining(111898));
                int timeSinceCast = 9000 - WoW.SpellCooldownTimeRemaining(111898);
                if (timeSinceCast > 2500){
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool DoomguardOut (){
            if (WoW.IsSpellOnCooldown("Doomguard")){
                Log.Write("Doomguard CD time remaining: " + WoW.SpellCooldownTimeRemaining(18540));
                int timeSinceCast = 1800 - WoW.SpellCooldownTimeRemaining(18540);
                if (timeSinceCast > 250){
                    return false;
                }
                return true;
            }
            return false;
        }

        public int TotalDemonsOut(){
            int total = 0;
            if (GrimoireFelguardOut()){
                total += 1;
                Log.Write("Grimoire: Felguard out");
            }
            if (DoomguardOut()){
                total += 1;
                Log.Write("Doomguard out");
            }
            total += WildImps + Dreadstalkers;
            Log.Write("Dreadstalkers out " + Dreadstalkers);
            Log.Write("Wild Imps out " + WildImps);
            Log.Write("Total Demons Out " + total);
            return total;
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=/kb
AddonName=Frozen
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,686,Shadow Bolt,NumPad1
Spell,157695,Demonbolt,NumPad1
Spell,104316,Call Dreadstalkers,NumPad2
Spell,105174,Hand of Guldan,NumPad3
Spell,193396,Demonic Empowerment,NumPad4
Spell,603,Doom,NumPad5
Spell,755,Health Funnel,Divide
Spell,193440,Demonwrath,NumPad6
Spell,1454,Life Tap,NumPad7
Spell,205180,Darkglare,NumPad8
Spell,111898,Grimoire: Felguard,NumPad9
Spell,211714,Talkiels Consumption,Add
Spell,205181,Shadowflame,NumPad0
Spell,30146,Felguard,D9
Spell,18540,Doomguard,Decimal
Spell,119914,Felstorm,D4
Spell,196098,Soul Harvest,D0
Spell,196277,Implosion,D7
Spell,30283,Shadowfury,D3
Spell,108416,Dark Pact,Multiply
Aura,2825,Bloodlust
Aura,32182,Heroism
Aura,80353,Time Warp
Aura,160452,Netherwinds
Aura,230935,Drums of War
Aura,603,Doom
Aura,193396,Demonic Empowerment
Aura,205146,Demonic Calling
Aura,205181,Shadowflame
Aura,127271,Mount
*/
