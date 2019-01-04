using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    //"Database" (or something of sorts, may (and should) be in text files in the future, modifiable by custom editors)
    public enum MobType { Energy, Component, Antivirus, Malware, Ascii }
    public enum StatType { Health, Energy, Attack, Defence, AbilityMp }

    public enum ScreenType { Battle, Town, Shop, Healing, MobDealer, Travel, SelectMob, SelectItem, Wilderness, Starting }
    public static class Data
    {

        public const string MobLabel = "Consomon";

        //All stats of Mob
        public static readonly Dictionary<StatType, StatInfo> Stats = new Dictionary<StatType, StatInfo>
        {
            { StatType.Health, new StatInfo("pixel", ConsoleColor.DarkRed, true, true) },
            { StatType.Energy, new StatInfo("bit", ConsoleColor.Blue, true, true) },
            { StatType.Defence, new StatInfo("defence",ConsoleColor.DarkYellow, false, false) },
            { StatType.Attack, new StatInfo("attack", ConsoleColor.DarkCyan, false, false) },
            { StatType.AbilityMp, new StatInfo("ability multiplier", ConsoleColor.Gray, false, false, true) }
        };

        //All Mobs used in game
        public static readonly List<Mob> Mobs = new List<Mob>
        {
            { new Mob( MobType.Ascii, "Undead Pixel", 20, 1, 15, 15, 40, new Ability[]{ new Ability(3, "Rigor Mortis") } ) },
            { new Mob( MobType.Ascii, "Underscore", 45, 2, 19, 17, 55, new Ability[]{ new Ability(6, "Blink"), new Ability(4, "Move Cursor") }) },
            { new Mob( MobType.Ascii, "Semicolonoscopy", 70, 4, 22, 21, 70, new Ability[]{ new Ability(1, "Expecting Semicolon"), new Ability(2, "Throw Dot"), new Ability(10, "Threatening Pose") }) },
            { new Mob( MobType.Energy, "Discharge", 21, 1, 15, 16, 45, new Ability[]{ new Ability(7, "Paralyse") }) },
            { new Mob( MobType.Energy, "Burnt Socket", 50, 3, 20, 16, 60, new Ability[]{ new Ability(0, "Recharge"), new Ability(5, "Discharge") }) },
            { new Mob( MobType.Energy, "On/On Switch", 72, 4, 22, 24, 70, new Ability[]{ new Ability(9, "Switch On"), new Ability(10, "Switch On"), new Ability(1, "Redirect") }) },
            { new Mob( MobType.Component, "Wireless Cable", 25, 2, 18, 15, 50, new Ability[]{ new Ability(8, "Ouroboros") }) },
            { new Mob( MobType.Component, "Any Key", 50, 3, 22, 18, 65, new Ability[]{ new Ability(6, "Stuck"), new Ability(5, "Spill Drink") }) },
            { new Mob( MobType.Component, "Broken Home Button", 80, 2, 20, 25, 70, new Ability[]{ new Ability(7, "Stuck"), new Ability(0, "Warranty"), new Ability(7, "Slide To Unlock") }) }, //
            { new Mob( MobType.Antivirus, "Backdoor Lock", 25, 2, 20, 12, 50, new Ability[]{ new Ability(3, "Keyhole Peek") }) },
            { new Mob( MobType.Antivirus, "Strong Password Generator", 49, 3, 22, 17, 60, new Ability[]{ new Ability(10, "Force Uppercase"), new Ability(11, "Recover Password") }) },
            { new Mob( MobType.Antivirus, "Defragmentation", 89, 5, 27, 21, 75, new Ability[]{ new Ability(4, "Organise Fragments"), new Ability(2, "Shoot Fragment"), new Ability(0, "Start Over") } ) },
            { new Mob( MobType.Malware, "Annoying Ad", 24, 2, 12, 19, 55, new Ability[]{ new Ability(6, "Pop-up") }) },
            { new Mob( MobType.Malware, "Phisherman", 35, 3, 15, 23, 65, new Ability[]{ new Ability(12, "Obtain Password"), new Ability(7, "Spoof") }) },
            { new Mob( MobType.Malware, "Trojan Mule", 100, 5, 20, 29, 80, new Ability[]{ new Ability(6, "Infect"), new Ability(6, "BSOD"), new Ability(8, "Rickroll") }) }
        };

        //All items
        public static readonly List<Item> Items = new List<Item>
        {
            //restorers
            { new StatResetter("Retouching kit", 10, $"Restores {Stats[StatType.Health].Title}", new StatType[] { StatType.Health }) },
            { new StatResetter("Cache cleaner", 15, $"Restores {Stats[StatType.Energy].Title}", new StatType[] { StatType.Energy }) },
            { new StatResetter("Pick-me-up", 20, "Restores battle stats", new StatType[] { StatType.Attack, StatType.Defence, StatType.AbilityMp }) },
            //container
            { new MobContainer("Memory Stick", 25, $"Attempts to catch currently targeted {MobLabel}") }
        };
        //Default Town establsihments
        public static readonly List<Establishment> DefaultEstablishments = new List<Establishment>
        {
            {new Establishment{Screen = ScreenType.Shop, Title = "Shop",} },
            {new Establishment{Screen = ScreenType.MobDealer, Title = $"{MobLabel} Dealer"} },
            {new Establishment{Screen = ScreenType.Healing, Title = "Healing Centre"} },
        };

        //All towns
        public static readonly List<Town> Towns = new List<Town>
        {
            { new Town("Charset Town", .1f, MobType.Ascii, new int[] { 2 }) }, //0
            { new Town("Output City", .3f, MobType.Ascii, new int[] { 2, 3, 6 }) }, //1
            { new Town("Circuit Town", .15f, MobType.Energy, new int[] { 0, 1, 3 }) }, //2
            { new Town("PSU", .25f, MobType.Energy, new int[] { 1, 2, 4 }) }, //3
            { new Town("Assembly Line", .35f, MobType.Component, new int[] { 3, 5 }) }, //4
            { new Town("Industrial Zone", .6f, MobType.Component, new int[] { 4, 7 }) }, //5
            { new Town("Avast Town", .4f, MobType.Antivirus, new int[] { 1, 7, 8 }) }, //6
            { new Town("Sandbox", .70f, MobType.Antivirus, new int[] { 5, 6 }) }, //7
            { new Town("Worm's Hole", .6f, MobType.Malware, new int[] { 6, 9 }) }, //8
            { new Town("Troja", 1, MobType.Malware, new int[] { 8 }) }, //9
        };


        //All Screens
        public static readonly Dictionary<ScreenType, Screen> Screens = new Dictionary<ScreenType, Screen>
        {
            { ScreenType.Battle, new Screen("Battle", Output.UseLabel, Controls.UseAbilityAction, new ConsoleKey[] { ConsoleKey.A, ConsoleKey.F, ConsoleKey.S, ConsoleKey.U }, true ) },
            { ScreenType.Town, new Screen("Town", Output.VisitLabel, Controls.VisitAction, new ConsoleKey[] { ConsoleKey.T }, true ) }, 
            { ScreenType.Shop, new Screen("Shop", Output.BuyLabel, Controls.BuyItemAction, new ConsoleKey[] { ConsoleKey.B } ) }, 
            { ScreenType.Starting, new Screen("Starting", "", Controls.GetMobAction, new ConsoleKey[] { } ) },
            { ScreenType.MobDealer, new Screen($"{MobLabel} Dealer", Output.BuyLabel, Controls.BuyMobAction, new ConsoleKey[] { ConsoleKey.B } ) }, 
            { ScreenType.Healing, new Screen("Healing Centre", "", null, new ConsoleKey[] { ConsoleKey.H, ConsoleKey.B }, true ) }, 
            { ScreenType.Travel, new Screen("Travel Screen", Output.TravelToLabel, Controls.TravelAction, new ConsoleKey[] { ConsoleKey.B } ) },
            { ScreenType.SelectMob, new Screen($"Select {MobLabel}", Output.SelectMobLabel, Controls.SelectMobAction, new ConsoleKey[] { ConsoleKey.C } ) },
            { ScreenType.SelectItem, new Screen($"Select {Output.ItemLabel}", Output.SelectItemLabel, Controls.UseItemAction, new ConsoleKey[] { ConsoleKey.C } ) },
            { ScreenType.Wilderness, new Screen("Wilderness", "", null, new ConsoleKey[] { } ) },
        };

        //All ability templates
        public static readonly List<AbilityTemplate> AbilityTemplates = new List<AbilityTemplate>
        {
            {
                new AbilityTemplate //0 -att +def +hp
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, true, true),
                        new AbilityElement(StatType.Attack, true, false),
                        new AbilityElement(StatType.Defence, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //1 -hp +hp
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, false, false, AbilityElement.SpecialElement.selfharm),
                        new AbilityElement(StatType.Health, true, true, AbilityElement.SpecialElement.selfharm),
                    }
                )
            },
            {
                new AbilityTemplate //2 -hp (self) -hp (opponent)
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, true, false),
                        new AbilityElement(StatType.Health, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //3 +def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Defence, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //4 +AMP
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.AbilityMp, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //5 -hp selfdestruct
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, false, false, AbilityElement.SpecialElement.selfdestruct)
                    }
                )
            },
            {
                new AbilityTemplate //6 -def -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Defence, false, false),
                        new AbilityElement(StatType.Attack, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //7 -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Attack, false, false)
                    }
                )
            },
            {
                new AbilityTemplate //8 +att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Attack, true, true)
                    }
                )
            },
            {
                new AbilityTemplate //9 +att -def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Attack, true, true),
                        new AbilityElement(StatType.Defence, true, false)
                    }
                )
            },
            {
                new AbilityTemplate //10 +def -att
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Defence, true, true),
                        new AbilityElement(StatType.Attack, true, false)
                    }
                )
            },
            {
                new AbilityTemplate //11 reset all stats, bits are 0
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, true, true, AbilityElement.SpecialElement.reset)
                    }
                )
            },
            {
                new AbilityTemplate //12 +hp -def
                (
                    new AbilityElement[]
                    {
                        new AbilityElement(StatType.Health, true, true),
                        new AbilityElement(StatType.Defence, false, false)
                    }
                )
            }
        };

        public static void ScaleLevels()
        {
            Settings.MinLevel = Mobs.Aggregate((m1, m2) => m1.Level < m2.Level ? m1 : m2).Level;
            Settings.MaxLevel = Mobs.Aggregate((m1, m2) => m1.Level > m2.Level ? m1 : m2).Level;

            foreach(var t in Towns)
            {
                t.Level = (float)GenericOperations.GetProportion(Settings.MinLevel, Settings.MaxLevel, t.LevelRatio);
                t.InitializeEstablishments();
            }
        }
    }
}