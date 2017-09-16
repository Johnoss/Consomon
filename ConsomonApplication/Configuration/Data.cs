using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    //"Database" (or something of sorts, may (and should) be in text files in the future, modifiable by custom editors)
    public enum MobType { energy, component, antivirus, malware, ascii }
    public enum StatType { health, energy, attack, defence, abilityMP }

    public enum ScreenType { battle, town, shop, healing, mobDealer, travel, selectMob, selectItem, wilderness, starting }
    public static class Data
    {

        public const string MobLabel = "Consomon";

        //All stats of Mob
        public static readonly Dictionary<StatType, StatInfo> Stats = new Dictionary<StatType, StatInfo>
        {
            { StatType.health, new StatInfo("pixel", ConsoleColor.DarkRed, true, true) },
            { StatType.energy, new StatInfo("bit", ConsoleColor.Blue, true, true) },
            { StatType.defence, new StatInfo("defence",ConsoleColor.DarkYellow, false, false) },
            { StatType.attack, new StatInfo("attack", ConsoleColor.DarkCyan, false, false) },
            { StatType.abilityMP, new StatInfo("ability multiplier", ConsoleColor.Gray, false, false, true) }
        };

        //All Mobs used in game
        public static readonly List<Mob> Mobs = new List<Mob>
        {
            { new Mob( MobType.ascii, "Undead Pixel", 20, 1, 15, 15, 40, new Ability[]{ new Ability(3, "Rigor Mortis") } ) },
            { new Mob( MobType.ascii, "Underscore", 45, 2, 19, 17, 55, new Ability[]{ new Ability(6, "Blink"), new Ability(4, "Move Cursor") }) },
            { new Mob( MobType.ascii, "Semicolonoscopy", 70, 4, 22, 21, 70, new Ability[]{ new Ability(1, "Expecting Semicolon"), new Ability(2, "Throw Dot"), new Ability(10, "Threatening Pose") }) },
            { new Mob( MobType.energy, "Discharge", 21, 1, 15, 16, 45, new Ability[]{ new Ability(7, "Paralyse") }) },
            { new Mob( MobType.energy, "Burnt Socket", 50, 3, 20, 16, 60, new Ability[]{ new Ability(0, "Recharge"), new Ability(5, "Discharge") }) },
            { new Mob( MobType.energy, "On/On Switch", 72, 4, 22, 24, 70, new Ability[]{ new Ability(9, "Switch On"), new Ability(10, "Switch On"), new Ability(1, "Redirect") }) },
            { new Mob( MobType.component, "Wireless Cable", 25, 2, 18, 15, 50, new Ability[]{ new Ability(8, "Ouroboros") }) },
            { new Mob( MobType.component, "Any Key", 50, 3, 22, 18, 65, new Ability[]{ new Ability(6, "Stuck"), new Ability(5, "Spill Drink") }) },
            { new Mob( MobType.component, "Broken Home Button", 80, 2, 20, 25, 70, new Ability[]{ new Ability(7, "Stuck"), new Ability(0, "Warranty"), new Ability(7, "Slide To Unlock") }) }, //
            { new Mob( MobType.antivirus, "Backdoor Lock", 25, 2, 20, 12, 50, new Ability[]{ new Ability(3, "Keyhole Peek") }) },
            { new Mob( MobType.antivirus, "Strong Password Generator", 49, 3, 22, 17, 60, new Ability[]{ new Ability(10, "Force Uppercase"), new Ability(11, "Recover Password") }) },
            { new Mob( MobType.antivirus, "Defragmentation", 89, 5, 27, 21, 75, new Ability[]{ new Ability(4, "Organise Fragments"), new Ability(2, "Shoot Fragment"), new Ability(0, "Start Over") } ) },
            { new Mob( MobType.malware, "Annoying Ad", 24, 2, 12, 19, 55, new Ability[]{ new Ability(6, "Pop-up") }) },
            { new Mob( MobType.malware, "Phisherman", 35, 3, 15, 23, 65, new Ability[]{ new Ability(12, "Obtain Password"), new Ability(7, "Spoof") }) },
            { new Mob( MobType.malware, "Trojan Mule", 100, 5, 20, 29, 80, new Ability[]{ new Ability(6, "Infect"), new Ability(6, "BSOD"), new Ability(8, "Rickroll") }) }
        };

        //All items
        public static readonly List<Item> Items = new List<Item>
        {
            //restorers
            { new StatResetter("Retouching kit", 10, $"Restores {Stats[StatType.health].Title}", new StatType[] { StatType.health }) },
            { new StatResetter("Cache cleaner", 15, $"Restores {Stats[StatType.energy].Title}", new StatType[] { StatType.energy }) },
            { new StatResetter("Pick-me-up", 20, "Restores battle stats", new StatType[] { StatType.attack, StatType.defence, StatType.abilityMP }) },
            //container
            { new MobContainer("Memory Stick", 25, $"Attempts to catch currently targeted {MobLabel}") }
        };
        //Default Town establsihments
        public static readonly List<Establishment> DefaultEstablishments = new List<Establishment>
        {
            {new Establishment{Screen = ScreenType.shop, Title = "Shop",} },
            {new Establishment{Screen = ScreenType.mobDealer, Title = $"{MobLabel} Dealer"} },
            {new Establishment{Screen = ScreenType.healing, Title = "Healing Centre"} },
        };

        //All towns
        public static readonly List<Town> Towns = new List<Town>
        {
            { new Town("Charset Town", .1f, MobType.ascii, new int[] { 2 }) }, //0
            { new Town("Output City", .3f, MobType.ascii, new int[] { 2, 3, 6 }) }, //1
            { new Town("Circuit Town", .15f, MobType.energy, new int[] { 0, 1, 3 }) }, //2
            { new Town("PSU", .25f, MobType.energy, new int[] { 1, 2, 4 }) }, //3
            { new Town("Assembly Line", .35f, MobType.component, new int[] { 3, 5 }) }, //4
            { new Town("Industrial Zone", .6f, MobType.component, new int[] { 4, 7 }) }, //5
            { new Town("Avast Town", .4f, MobType.antivirus, new int[] { 1, 7, 8 }) }, //6
            { new Town("Sandbox", .70f, MobType.antivirus, new int[] { 5, 6 }) }, //7
            { new Town("Worm's Hole", .6f, MobType.malware, new int[] { 6, 9 }) }, //8
            { new Town("Troja", 1, MobType.malware, new int[] { 8 }) }, //9
        };


        //All Screens
        public static readonly Dictionary<ScreenType, Screen> Screens = new Dictionary<ScreenType, Screen>
        {
            { ScreenType.battle, new Screen("Battle", Output.UseLabel, Controls.UseAbilityAction, new ConsoleKey[] { ConsoleKey.A, ConsoleKey.F, ConsoleKey.S, ConsoleKey.U }, true ) },
            { ScreenType.town, new Screen("Town", Output.VisitLabel, Controls.VisitAction, new ConsoleKey[] { ConsoleKey.T }, true ) }, 
            { ScreenType.shop, new Screen("Shop", Output.BuyLabel, Controls.BuyItemAction, new ConsoleKey[] { ConsoleKey.B } ) }, 
            { ScreenType.starting, new Screen("Starting", "", Controls.GetMobAction, new ConsoleKey[] { } ) },
            { ScreenType.mobDealer, new Screen($"{MobLabel} Dealer", Output.BuyLabel, Controls.BuyMobAction, new ConsoleKey[] { ConsoleKey.B } ) }, 
            { ScreenType.healing, new Screen("Healing Centre", "", null, new ConsoleKey[] { ConsoleKey.H, ConsoleKey.B }, true ) }, 
            { ScreenType.travel, new Screen("Travel Screen", Output.TravelToLabel, Controls.TravelAction, new ConsoleKey[] { ConsoleKey.B } ) },
            { ScreenType.selectMob, new Screen($"Select {MobLabel}", Output.SelectMobLabel, Controls.SelectMobAction, new ConsoleKey[] { ConsoleKey.C } ) },
            { ScreenType.selectItem, new Screen($"Select {Output.ItemLabel}", Output.SelectItemLabel, Controls.UseItemAction, new ConsoleKey[] { ConsoleKey.C } ) },
            { ScreenType.wilderness, new Screen("Wilderness", "", null, new ConsoleKey[] { } ) },
        };

        //All ability templates
        public static readonly List<AbilityTemplate> AbilityTemplates = new List<AbilityTemplate>
        {
            {
                new AbilityTemplate //0 -att +def +hp
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, true, true),
                        new AbilityElement(StatType.attack, true, false),
                        new AbilityElement(StatType.defence, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //1 -hp +hp
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, false, false, AbilityElement.SpecialElement.selfharm),
                        new AbilityElement(StatType.health, true, true, AbilityElement.SpecialElement.selfharm),
                    }
                )
            },
            {
                new AbilityTemplate //2 -hp (self) -hp (opponent)
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, true, false),
                        new AbilityElement(StatType.health, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //3 +def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.defence, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //4 +AMP
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.abilityMP, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //5 -hp selfdestruct
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, false, false, AbilityElement.SpecialElement.selfdestruct)
                    }
                )
            },
            {
                new AbilityTemplate //6 -def -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.defence, false, false),
                        new AbilityElement(StatType.attack, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //7 -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.attack, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //8 +att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.attack, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //9 +att -def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.attack, true, true),
                        new AbilityElement(StatType.defence, true, false)
                    }
                )
            },
            {
                new AbilityTemplate //10 +def -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.defence, true, true),
                        new AbilityElement(StatType.attack, true, false)
                    }
                )
            },
            {
                new AbilityTemplate //11 reset all stats, bits are 0
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, true, true, AbilityElement.SpecialElement.reset)
                    }
                )
            },
            {
                new AbilityTemplate //12 +hp -def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.health, true, true),
                        new AbilityElement(StatType.defence, false, false)
                    }
                )
            }
        };

        public static void ScaleLevels()
        {
            Settings.MinLevel = Mobs.Aggregate((m1, m2) => m1.Level < m2.Level ? m1 : m2).Level;
            Settings.MaxLevel = Mobs.Aggregate((m1, m2) => m1.Level > m2.Level ? m1 : m2).Level;

            foreach(Town t in Towns)
            {
                t.Level = (float)GenericOperations.GetProportion(Settings.MinLevel, Settings.MaxLevel, t.LevelRatio);
                t.InitializeEstablishments();
            }
        }
    }
}