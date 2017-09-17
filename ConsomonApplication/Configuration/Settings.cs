using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public static class Settings //Game settings, controls and default values (may be in config files in the future)
    {
        //hardcoded combat limits
        public const int MinDefence = 1;
        public const int MinAttack = 1;
        public const int MinAbilityMP = 20;

        public const int MaxDefence = 90;
        public const int MaxAttack = 100;
        public const int MaxAbilityMP = 200;

        public const int MinDamage = 3;
        public const int DamageDeviation = 3;

        public const float StrongMP = 1.5f;

        public const int defaultActions = 1;

        public const float FleeChance = .5f;

        //abilities
        public const int MinAbilityValue = 1;
        public const int DefaultAbilityValue = 5;

        public const int EnemyAgression = 30; //how likely (in percent) is enemy to attempt to use an ability

        public const int AbilityPrice = 1;

        //Key bindings - may be configurable in future (but prolly won't)

        public static Dictionary<ConsoleKey, ActionControl> KeyBindings = new Dictionary<ConsoleKey, ActionControl>
        {
            { ConsoleKey.A, new ActionControl( Output.AttackLabel, Controls.AttackAction ) }, 
            { ConsoleKey.F, new ActionControl( Output.FleeLabel, Controls.FleeAction ) }, 
            { ConsoleKey.S, new ActionControl( Output.SelectMobLabel, Controls.SelectMobScreen ) }, 
            { ConsoleKey.T, new ActionControl( Output.TravelLabel, Controls.TravelScreen ) },
            { ConsoleKey.B, new ActionControl( Output.BackLabel, Controls.GoBackAction ) },
            { ConsoleKey.H, new ActionControl( Output.HealMobsLabel, Controls.HealMobsAction ) },
            { ConsoleKey.U, new ActionControl( Output.SelectItemLabel, Controls.SelectItemScreen ) },
            { ConsoleKey.C, new ActionControl( Output.CancelLabel, Controls.ResetScreen ) }
        };


        //Difficulity scaling
        public static float MinLevel = 0;
        public static float MaxLevel = 0;

        public static int MinEncounters = 0;
        public static int MaxEncounters = 10;
        public static int EncountersDeviation = 2; //deviation of encounter amount
        public static float EncounterLevelDeviation = 3; //deviation of encountered Mobs' level

        public static float MediumTravelRatio = .2f; //min difference between [wilderness level] and [player's mobs average level] to label the travel medium
        public static float HardTravelRatio = .7f;

        //Traveling and encounters
        public static float NativeWildlifeChance = .4f; //chance to encounter Mob of the same type as connected towns
        public static int DefaultWildernessGoal = 100;

        //Console settings
        public static ConsoleColor DefaultTextColor = ConsoleColor.Gray;
        public static ConsoleColor DefaulteEnemyColor = ConsoleColor.Red;
        public static ConsoleColor DefaultePlayerColor = ConsoleColor.Green;

        public static ConsoleColor DefaultStrongColor = ConsoleColor.Yellow;

        //Misc stuff
        public const int MaxLoadingDelay = 100;
        public const int MinLoadingDelay = 25;
        public const int LoadingStuckDelay = 1000;
        public const int MaxStuckTimes = 5;

        //Money Settings
        public const int StartingMoney = 50;
        public const float MobPriceMultiplier = 20;

        //Rewards
        public const int RoundRewards = 25;
        public const int RewardDeviation = 50;

        public const float RewardLevelMultiplier = 3;

        public static string SavePath = AppDomain.CurrentDomain.BaseDirectory + $"Properties";
        public static string SaveFile = $"Player.{Output.FileType}";

    }
}
