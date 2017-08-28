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
        public static int[] Talents;

        public static int ShadowBolt = 686;
        public static int DemonBolt = 157695;
        public static int CallDreadstalkers = 104316;
        public static int HandOfGuldan = 105174;
        public static int DemonicEmpowerment = 193396;
        public static int Doom = 603;
        public static int HealthFunnel = 755;
        public static int DemonWrath = 193440;
        public static int LifeTap = 1454;
        public static int Darkglare = 205180;
        public static int GrimoireFelguard = 111898;
        public static int ThalkielsConsumption = 211714;
        public static int ShadowFlame = 205181;
        public static int Felguard = 30146;
        public static int Doomguard = 18540;
        public static int Felstorm = 119914;
        public static int SoulHarvest = 196098;
        public static int Implosion = 196277;
        public static int Shadowfury = 30283;
        public static int DarkPact = 108416;

        public override Form SettingsForm { get; set; }

        public override void Initialize(){
            Log.Write("Welcome to Jarl's Demonology Warlock rotation", Color.Purple);
            WildImps = 0;
            Dreadstalkers = 0;
            Talents = new int[5];
        }

        public override void Stop()
        {
        }

        public override void Pulse(){
            GetCurrentTalents();

            bool HasPet = WoW.HasPet;
            bool IsMoving = WoW.IsMoving;
            bool IsMounted = WoW.IsMounted;
            bool PlayerIsCasting = WoW.PlayerIsCasting;
            bool HasTarget = WoW.HasTarget;
            bool TargetIsEnemy = WoW.TargetIsEnemy;
            bool IsInCombat = WoW.IsInCombat;
            bool TargetHasDoom = WoW.TargetHasDebuff("Doom");
            bool IsSpellInRange = WoW.IsSpellInRange("Doom");
            int Mana = WoW.Mana;
            int HealthPercent = WoW.HealthPercent;
            int PetHealthPercent = WoW.PetHealthPercent;
            int CurrentSoulShards = WoW.CurrentSoulShards;
            int LastSpellCastedID = WoW.LastSpellCastedID;

            //Keep a pet out
            if ((!HasPet || HasPet && PetHealthPercent == 0) && !IsMounted && !PlayerIsCasting && LastSpellCastedID != Felguard){
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
            if (WoW.CanCast("Life Tap") && !IsMounted && Mana < 40 && HealthPercent > 40){
                WoW.CastSpell("Life Tap");
                return;
            }

            //Health Funnel
            if (WoW.CanCast("Health Funnel") && PetHealthPercent <= 30 && !IsMoving && !IsMounted && HasPet){
                WoW.CastSpell("Health Funnel");
                return;
            }

            //Doom
            if (HasTarget && TargetIsEnemy && IsInCombat && !IsMounted && !TargetHasDoom){
                WoW.CastSpell("Doom");
                return;
            }

            // Cast Demonwrath when in combat and moving
            if (HasTarget && TargetIsEnemy && IsInCombat && WoW.CanCast("Demonwrath") && Mana > 40 && IsMoving && !IsMounted){
                WoW.CastSpell("Demonwrath");
                return;
            }

            // Burst mode enabled
            if (WoW.CooldownsOn){
                if (HasTarget && TargetIsEnemy && IsInCombat && !PlayerIsCasting && !IsMounted && IsSpellInRange && CurrentSoulShards >= 1){
                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard") && Talents[3] == 2){
                        WoW.CastSpell("Grimoire: Felguard");
                        return;
                    }

                    //Doomguard
                    if (WoW.CanCast("Doomguard") && Talents[3] != 1){
                        WoW.CastSpell("Doomguard");
                        return;
                    }
                }
            }

            //Single target rotation
            if (WoW.RotationOn && !WoW.AoeOn && !WoW.CleaveOn){
                if (HasTarget && TargetIsEnemy && IsInCombat && !PlayerIsCasting && !IsMounted && !IsMoving && IsSpellInRange)
                {
                    Log.Write("Current Doombolt cast speed: " + GetCastTime(1.5));
                    //If consumption is ready combo up to get more demons out
                    if (WoW.CanCast("Talkiels Consumption")){
                        //Doomguard + Dreadstalkers combo
                        if (LastSpellCastedID == Doomguard && WoW.CanCast("Call Dreadstalkers") && (WoW.PlayerHasBuff("Demonic Calling") || CurrentSoulShards >= 2)){
                            WoW.CastSpell("Call Dreadstalkers");
                            DreadStalkersSummoned();
                            return;
                        }

                        //Doomguard + Hand of Guldan combo
                        if (LastSpellCastedID == Doomguard && CurrentSoulShards >= 2 && WoW.CanCast("Hand of Guldan")){
                            if (CurrentSoulShards > 4){
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(4);
                            }
                            else{
                                WoW.CastSpell("Hand of Guldan");
                                WildImpsSummoned(CurrentSoulShards);
                            }
                            return;
                        }

                        //Hand of Guldan + Dreadstalkers combo
                        if (LastSpellCastedID == HandOfGuldan && WoW.CanCast("Call Dreadstalkers") && (WoW.PlayerHasBuff("Demonic Calling") || CurrentSoulShards >= 2)){
                            WoW.CastSpell("Call Dreadstalkers");
                            DreadStalkersSummoned();
                            return;
                        }

                        //Demonic Empowerment
                        if (WoW.CanCast("Demonic Empowerment") && LastSpellCastedID != DemonicEmpowerment && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 200 || WoW.WasLastCasted("Call Dreadstalkers") || WoW.WasLastCasted("Grimoire: Felguard") || WoW.WasLastCasted("Doomguard") || WoW.WasLastCasted("Hand of Guldan"))){
                            WoW.CastSpell("Demonic Empowerment");
                            return;
                        }

                        //Talkiels Consumption combo with Demonic Empowerment
                        if (LastSpellCastedID == DemonicEmpowerment && WoW.CanCast("Talkiels Consumption") && TotalDemonsOut() >= 8){
                            WoW.CastSpell("Talkiels Consumption");
                            return;
                        }
                    }
                    

                    //Demonic Empowerment
                    if (WoW.CanCast("Demonic Empowerment") && LastSpellCastedID != DemonicEmpowerment && (!WoW.PetHasBuff("Demonic Empowerment") || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 200 || LastSpellCastedID == CallDreadstalkers || LastSpellCastedID == GrimoireFelguard || LastSpellCastedID == Doomguard || LastSpellCastedID == HandOfGuldan)){
                        WoW.CastSpell("Demonic Empowerment");
                        return;
                    }

                    //Dreadstalkers
                    if (WoW.CanCast("Call Dreadstalkers") && (CurrentSoulShards >= 2 || WoW.PlayerHasBuff("Demonic Calling"))){
                        WoW.CastSpell("Call Dreadstalkers");
                        DreadStalkersSummoned();
                        return;
                    }

                    //Hand of Guldan
                    if (WoW.CanCast("Hand of Guldan") && (CurrentSoulShards >= 4 || (CurrentSoulShards >= 2 && WoW.TargetDebuffTimeRemaining("Doom") <= 200))){
                        if (CurrentSoulShards > 4){
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(4);
                        }
                        else{
                            WoW.CastSpell("Hand of Guldan");
                            WildImpsSummoned(CurrentSoulShards);
                        }
                        return;
                    }
                    
                    //Shadowbolt/Demonbolt
                    if ((WoW.CanCast("Shadow Bolt") || WoW.CanCast("Demonbolt")) && CurrentSoulShards != 5){
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

        public void GetCurrentTalents(){
            Talents[0] = WoW.Talent(1); //Tier 1
            Talents[1] = WoW.Talent(2); //Tier 2
            Talents[2] = WoW.Talent(4); //Tier 4
            Talents[3] = WoW.Talent(6); //Tier 6
            Talents[4] = WoW.Talent(7); //Tier 7
            return;
        }

        public double GetCastTime (double castSpeed){
            double castTime = System.Convert.ToDouble(castSpeed / (1 + WoW.HastePercent / 100f) * 100f);
            return castTime;
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
