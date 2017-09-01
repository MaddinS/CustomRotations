using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using Frozen.Helpers;

namespace Frozen.Rotation
{
    public class WarlockDemonology : CombatRoutine
    {
        public override string Name => "Possessed";

        public override string Class => "Warlock";

        public static int WildImpsOut;
        public static int DreadstalkersOut;
        public static int GrimoireFelguardOut;
        public static int DoomguardOut;
        public static int[] Talents;

        public struct SpellHistory{
            public int lastSpell;
            public int secondToLastSpell;
            public int thirdToLastSpell;
        }
        public static SpellHistory spellHistory;

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

        public bool CheckSingleTarget(){
            if (WoW.IsInCombat
                && !WoW.AoeOn
                && !WoW.CleaveOn
                && WoW.HasTarget
                && WoW.TargetIsEnemy
                && (!WoW.PlayerIsCasting || spellHistory.lastSpell == DemonWrath)
                && !WoW.PlayerIsChanneling
                && !WoW.IsMounted
                && !WoW.IsMoving
                && WoW.IsSpellInRange("Doom")){
                    return true;
            }
            return false;
        }

        public bool CheckDoom(){
            //Doom
            if (WoW.HasTarget
                && WoW.TargetIsEnemy
                && WoW.IsInCombat
                && !WoW.IsMounted
                && !WoW.TargetHasDebuff("Doom")
                && (spellHistory.lastSpell != HandOfGuldan || spellHistory.secondToLastSpell != HandOfGuldan)){
                    return true;
            }
            return false;
        }

        public bool CheckDemonicEmpowerment(){
            if (WoW.CanCast("Demonic Empowerment")
                && spellHistory.lastSpell != DemonicEmpowerment
                && (!WoW.PetHasBuff("Demonic Empowerment")
                    || WoW.PetBuffTimeRemaining("Demonic Empowerment") <= 300
                    || spellHistory.lastSpell == CallDreadstalkers
                    || spellHistory.lastSpell == GrimoireFelguard
                    || spellHistory.lastSpell == Doomguard
                    || spellHistory.lastSpell == HandOfGuldan)){
                        return true;
            }
            return false;
        }

        public void KeepPetSummoned(){
            if ((!WoW.HasPet || (WoW.HasPet && WoW.PetHealthPercent == 0))
                && !WoW.IsMounted
                && !WoW.PlayerIsCasting
                && !WoW.PlayerIsChanneling
                && (spellHistory.lastSpell != Felguard || (spellHistory.lastSpell != Doomguard && WoW.Talent(6) == 1))){
                        if (WoW.Talent(6) != 1){
                            WoW.CastSpell("Felguard");
                            AddToSpellHistory(Felguard);
                            return;
                        }
                        if (WoW.Talent(6) == 1){
                            WoW.CastSpell("Doomguard");
                            AddToSpellHistory(Doomguard);
                            return;
                        }
            }
        }

        public void OpeningRotationSingleTarget(){
            return;
        }

        public void OpeningRotationAOE(){
            return;
        }

        public void AddToSpellHistory(int spell){
            if (spellHistory.lastSpell != 0){
                if (spellHistory.secondToLastSpell != 0){
                    spellHistory.thirdToLastSpell = spellHistory.secondToLastSpell;
                    spellHistory.secondToLastSpell = spellHistory.lastSpell;
                }
                else {
                    spellHistory.secondToLastSpell = spellHistory.lastSpell;
                }
            }
            spellHistory.lastSpell = spell;
            return;
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

        //Wild Imp control functions
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

        //Dreadstalker control functions
        public void DreadStalkersSummoned (){
            if (Talents[1] == 2){
                WildImpsSummoned(2);
            }
            DreadstalkersOut += 2;
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
            DreadstalkersOut -= 2;
            return;
        }

        //Grimoire Felguard control functions
        public void GrimoireSummoned (){
            GrimoireFelguardOut += 1;
            RunGrimoireTimer();
            return;
        }
        public bool RunGrimoireTimer (){
            System.Timers.Timer grimoireTimer = new System.Timers.Timer (12000);
            grimoireTimer.Elapsed += GrimoireTimedOut;
            grimoireTimer.AutoReset = false;
            grimoireTimer.Enabled = true;
        }
        public void GrimoireTimedOut (object sender, ElapsedEventArgs e){
            GrimoireFelguardOut -= 1;
            return;
        }

        //Doomguard control functions
        public void DoomguardSummoned (){
            DoomguardOut += 1;
            RunDoomguardTimer();
            return;
        }
        public bool RunDoomguardTimer (){
            System.Timers.Timer doomguardTimer = new System.Timers.Timer (12000);
            doomguardTimer.Elapsed += DoomguardTimedOut;
            doomguardTimer.AutoReset = false;
            doomguardTimer.Enabled = true;
        }
        public void DoomguardTimedOut (object sender, ElapsedEventArgs e){
            DoomguardOut -= 1;
            return;
        }

        public int TotalDemonsOut(){
            int total = 0;
            if (WoW.HasPet && WoW.PetHealthPercent > 0){
                total += 1;
            }
            total += WildImpsOut + DreadstalkersOut + GrimoireFelguardOut + DoomguardOut;
            return total;
        }

        public int TotalDemonWeight(){
            //This formula is based on Not's Demonology Guide
            int total = 0;
            if (WoW.HasPet && WoW.PetHealthPercent > 0){
                //Assumes you are using Felguard if you aren't specced for Doomguard
                if (WoW.Talent(6) != 1){
                    total += 156;
                }
                //If you are specced for Doomguard
                if (WoW.Talent(6) == 1){
                    total += 125;
                }
                total += (WildImpsOut * 47) + (DreadstalkersOut * 125) + (GrimoireFelguardOut * 156) + (DoomguardOut * 125);
            }
            return total;
        }

        public override void Initialize(){
            Log.Write("Welcome to Possessed (Free Version) - A Demonology rotation by JarlBrak", Color.FromArgb(148,130,201));
            Log.Write("The free version is limited to single target rotation only with no menu customizations.", Color.FromArgb(148,130,201));
            Log.Write("For information on how to purchase, please chat with me on Discord!", Color.FromArgb(148,130,201));
            Log.Write("---------------------------------------------------------------------------------------", Color.FromArgb(148,130,201));
            WildImps = 0;
            Dreadstalkers = 0;
            Talents = new int[5];
            GetCurrentTalents();
        }

        public override void Stop(){
        }

        public override void Pulse(){
            if (WoW.RotationOn){
                KeepPetSummoned();

                // Cast Life Tap when low
                if (WoW.CanCast("Life Tap")
                    && !WoW.IsMounted
                    && WoW.Mana < 20
                    && WoW.HealthPercent > 40){
                        WoW.CastSpell("Life Tap");
                        AddToSpellHistory(LifeTap);
                        return;
                }

                //Health Funnel
                if (WoW.CanCast("Health Funnel")
                    && WoW.PetHealthPercent <= 30
                    && !WoW.IsMoving
                    && !WoW.IsMounted
                    && WoW.HasPet){
                        WoW.CastSpell("Health Funnel");
                        AddToSpellHistory(HealthFunnel);
                        return;
                }
                
                // Cast Demonwrath when in combat and moving
                if (WoW.CanCast("Demonwrath")
                    && WoW.IsMoving
                    && !WoW.IsMounted
                    && WoW.HasTarget
                    && WoW.TargetIsEnemy
                    && WoW.IsInCombat
                    && WoW.Mana > 40){
                        WoW.CastSpell("Demonwrath");
                        AddToSpellHistory(DemonWrath);
                        return;
                }

                // Burst mode enabled
                if (WoW.CooldownsOn && CheckSingleTarget()){

                    //Doom
                    if (CheckDoom()){
                        WoW.CastSpell("Doom");
                        AddToSpellHistory(Doom);
                        return;
                    }

                    //Grimoire of Service
                    if (WoW.CanCast("Grimoire: Felguard") && WoW.Talent(6) == 2){
                        WoW.CastSpell("Grimoire: Felguard");
                        AddToSpellHistory(GrimoireFelguard);
                        return;
                    }

                    //Doomguard
                    if (WoW.CanCast("Doomguard") && WoW.Talent(6) != 1){
                        WoW.CastSpell("Doomguard");
                        AddToSpellHistory(Doomguard);
                        return;
                    }
                }

                //Single target rotation
                if (CheckSingleTarget()){

                    //Doom
                    if (CheckDoom()){
                        WoW.CastSpell("Doom");
                        AddToSpellHistory(Doom);
                        return;
                    }

                    //If consumption is ready combo up to get more demons out
                    if (WoW.CanCast("Thalkiels Consumption")){
                        //Doomguard/Felguard + Dreadstalkers combo
                        if (WoW.CanCast("Call Dreadstalkers")
                            && (spellHistory.lastSpell == Doomguard || spellHistory.lastSpell == Felguard)
                            && (WoW.PlayerHasBuff("Demonic Calling") || WoW.CurrentSoulShards >= 2)){
                                WoW.CastSpell("Call Dreadstalkers");
                                AddToSpellHistory(CallDreadstalkers);
                                DreadStalkersSummoned();
                                return;
                        }

                        //Doomguard/Felguard + Hand of Guldan combo
                        if (WoW.CanCast("Hand of Guldan")
                            && WoW.CurrentSoulShards >= 2
                            && (spellHistory.lastSpell == Doomguard || spellHistory.lastSpell == Felguard)){
                                if (WoW.CurrentSoulShards > 4){
                                    WoW.CastSpell("Hand of Guldan");
                                    AddToSpellHistory(HandOfGuldan);
                                    WildImpsSummoned(4);
                                }
                                else{
                                    WoW.CastSpell("Hand of Guldan");
                                    AddToSpellHistory(HandOfGuldan);
                                    WildImpsSummoned(WoW.CurrentSoulShards);
                                }
                                return;
                        }

                        //Hand of Guldan + Dreadstalkers combo
                        if (WoW.CanCast("Call Dreadstalkers")
                            && spellHistory.lastSpell == HandOfGuldan
                            && (WoW.PlayerHasBuff("Demonic Calling") || WoW.CurrentSoulShards >= 2)){
                                WoW.CastSpell("Call Dreadstalkers");
                                AddToSpellHistory(CallDreadstalkers);
                                DreadStalkersSummoned();
                                return;
                        }

                        //Demonic Empowerment
                        if (CheckDemonicEmpowerment()){
                            WoW.CastSpell("Demonic Empowerment");
                            AddToSpellHistory(DemonicEmpowerment);
                            return;
                        }

                        //Thalkiels Consumption combo with Demonic Empowerment
                        if (WoW.CanCast("Thalkiels Consumption")
                            && TotalDemonWeight() >= 688 //Equivalent of Felguard(Pet) + 6 Imps + 2 Dreadstalkers
                            && spellHistory.lastSpell == DemonicEmpowerment){
                                Log.Write("Total Demons Out: " + TotalDemonsOut(), Color.Purple);
                                Log.Write("Total Demon Weight: " + TotalDemonWeight(), Color.Purple);
                                WoW.CastSpell("Thalkiels Consumption");
                                AddToSpellHistory(ThalkielsConsumption);
                                return;
                        }
                    }

                    //Dreadstalkers
                    if (WoW.CanCast("Call Dreadstalkers")
                        && (WoW.CurrentSoulShards >= 2 || WoW.PlayerHasBuff("Demonic Calling"))){
                            WoW.CastSpell("Call Dreadstalkers");
                            AddToSpellHistory(CallDreadstalkers);
                            DreadStalkersSummoned();
                            return;
                    }

                    //Hand of Guldan
                    if (WoW.CanCast("Hand of Guldan")
                        && (WoW.CurrentSoulShards >= 4 || (WoW.CurrentSoulShards >= 2 && WoW.TargetDebuffTimeRemaining("Doom") <= 300))){
                            if (WoW.CurrentSoulShards > 4){
                                WoW.CastSpell("Hand of Guldan");
                                AddToSpellHistory(HandOfGuldan);
                                WildImpsSummoned(4);
                            }
                            else{
                                WoW.CastSpell("Hand of Guldan");
                                AddToSpellHistory(HandOfGuldan);
                                WildImpsSummoned(WoW.CurrentSoulShards);
                            }
                            return;
                    }

                    //Demonic Empowerment
                    if (CheckDemonicEmpowerment()){
                        WoW.CastSpell("Demonic Empowerment");
                        AddToSpellHistory(DemonicEmpowerment);
                        return;
                    }
                    
                    //Shadowbolt/Demonbolt
                    if (WoW.CanCast("Demonbolt")
                        && WoW.CurrentSoulShards != 5){
                            WoW.CastSpell("Demonbolt");
                            AddToSpellHistory(DemonBolt);
                            return;
                    }
                    if (WoW.CanCast("Shadow Bolt")
                        && WoW.CurrentSoulShards != 5){
                            WoW.CastSpell("Shadow Bolt");
                            AddToSpellHistory(ShadowBolt);
                            return;
                    }
                }
                if (WoW.AoeOn){
                    return;
                }
                if (WoW.CleaveOn){
                    return;
                }
            } 
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=JarlBrak
AddonName=Possessed
WoWVersion=Legion - 70300
[SpellBook.db]
Spell,686,Shadow Bolt,D1
Spell,157695,Demonbolt,D1
Spell,193396,Demonic Empowerment,D2
Spell,603,Doom,D3
Spell,104316,Call Dreadstalkers,D4
Spell,105174,Hand of Guldan,D5
Spell,211714,Thalkiels Consumption,D6
Spell,196277,Implosion,D7
Spell,193440,Demonwrath,D8
Spell,755,Health Funnel,D9
Spell,1454,Life Tap,D0
Spell,30146,Felguard,NumPad1
Spell,111898,Grimoire: Felguard,NumPad2
Spell,18540,Doomguard,NumPad3
Spell,205180,Darkglare,NumPad4
Spell,119914,Felstorm,NumPad5
Spell,205181,Shadowflame,NumPad6
Spell,196098,Soul Harvest,NumPad7
Spell,30283,Shadowfury,NumPad8
Spell,108416,Dark Pact,NumPad9
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
