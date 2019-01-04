using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsomonApplication
{
    //String handeling class. The purpouse of this file is to have all the strings and all the Console... commands (but Console.Clear) in one file
    internal static class Output
    {
        //string constants (possible translations if expanded)
        public const string AttackLabel = "Attack";
        public const string FleeLabel = "Flee";
        public const string UseLabel = "Use";
        public const string BuyLabel = "Buy";
        public const string TravelLabel = "Travel";
        public const string LeaveLabel = "Leave";
        public const string TravelToLabel = "Travel to";
        public const string VisitLabel = "Visit";
        public const string BackLabel = "Go back";
        public const string LoadingLabel = "Loading";
        public const string LeavingLabel = "Leaving";
        public const string FileType = "csm";
        public const string CancelLabel = "Cancel";

        public const string ItemLabel = "Item";

        public const string CapturedLabel = "captured";
        public const string BoughtLabel = "bought";
        public const string DefeatedLabel = "threw an exception";


        public const string ShopName = "Shop";
        public const string HealingCName = "Healing Centre";

        public const string YouGotLabel = "You got";
        public const string CurrencyName = "credits";

        public const string WildLabel = "Wild";
        public const string PlayersLabel = "Your";

        public const string EasyLabel = "easy";
        public const string MediumLabel = "challenging";
        public const string HardLabel = "dangerous";

        public const string StrongAttackSentence = "It's super effective!";
        public const string ContinueSentence = "Press any key to continue...";
        public const string WhereToSentence = "Where do you want to travel?";
        public const string CannotAffordSentence = "You cannot afford it.";

        public const string YourTurnLabel = "Your Turn:";
        public const string EnemyTurnLabel = "Enemy's Turn:";

        public const string YouLostSentence = "You Lost";
        public const string YouWonSentence = "You Won";

        private const string MobSelected = "Selected: ";
        public const string ItemUsed = "used";

        public const string NoTarget = "No target";

        public const string Separator = "-------------------------------";
        private const char barPart = '█';

        public const string DidYouKnowSentence = "Did you know?";
        public static readonly string SelectMobLabel = $"Select {Data.MobLabel}";
        public static readonly string SelectItemLabel = $"Use {ItemLabel}";
        public static readonly string HealMobsLabel = $"Heal all {Data.MobLabel}s";
        public static string MobDealerName = $"{Data.MobLabel} Dealer";
        public static readonly string YouHaveLabel = "You have";
        public static readonly string ReturnedSentence = $"{LoadingLabel} interrupted. Rolling back to";

        public static readonly string FleeSuccessSentence =
            "When danger reared it's ugly head, you bravely turned your tail and fled.";

        public static readonly string FleeFailureSentence = "You weren't able to flee.";
        public static readonly string CaptureFailureSentence = "You weren't able to capture the enemy.";

        public static readonly string MobsHealedSentence = $"All {Data.MobLabel}s healed.";

        public static readonly string AllMobsDefeatedSentence =
            $"All your {Data.MobLabel}s {DefeatedLabel}. You should visit a healing centre";

        public static string NotEnoughMoney = $"Not enough {CurrencyName}s";

        public static readonly List<string> ProTips = new List<string>
        {
            "Ascii is strong against energy.",
            "Energy is strong against component.",
            "Component is strong against antivirus.",
            "Antivirus is strong against malware.",
            "Malware is strong against ascii.",
            "If you don't win a battle, you can't advance.",
            $"You can only buy {Data.MobLabel}s of the town's type.",
            $"You can heal all your {Data.MobLabel} for free in every town.",
            $"Wild {Data.MobLabel}s tend to be of the same type as surrounding towns.",
            $"{Data.Mobs.Last().NameRaw} is the most powerful {Data.MobLabel} there is.",
            $"The farther you advance, the more difficult {Data.MobLabel}s you encounter.",
            $"The farther you advance, the more {LoadingLabel} interruptions you experience.",
            $"You should own a diverse selection of {Data.MobLabel}s' types, so you can adapt better.",
            $"The less {Data.Stats[StatType.Health].Title} the {Data.MobLabel} has, the easier it is to catch."
        };

        #region Town

        public static void WriteTownInfo(Town town)
        {
            Console.WriteLine($"You are in {town.Title}. The folks around here worship {town.TownType} gods.");
        }

        #endregion

        #region Generic Outputs

        public static void WriteGenericText(string text)
        {
            Console.WriteLine(text);
        }

        public static string ComposeGenericText(string[] args)
        {
            var result = new StringBuilder();
            foreach (var s in args) result.Append($"{s} ");
            return result.ToString();
        }

        public static void WriteCleanPause(string text)
        {
            Console.Clear();
            Console.WriteLine(text);
            UI.Pause();
        }

        public static void WritelineColor(string text, ConsoleColor color)
        {
            WriteColorTextMethod(text, color, false);
        }

        public static void WriteColor(string text, ConsoleColor color)
        {
            WriteColorTextMethod(text, color, true);
        }

        public static void TranslateControlsBottom(Player player)
        {
            var result = new StringBuilder();
            foreach (var ac in player.ControlScheme) result.Append($"{ac.Key.ToString()} {ac.Value}   ");
            WriteOnBottomLine(result.ToString());
        }

        public static void TranslateControls(Player player)
        {
            foreach (var ac in player.ControlScheme)
            {
                var result = new StringBuilder();
                result.Append(ac.Key);
                result.Append(" - ");
                result.Append(ac.Value);
                Console.WriteLine(result);
            }
        }

        public static void WriteOnBottomLine(string text)
        {
            var x = Console.CursorLeft;
            var y = Console.CursorTop;

            CursorBottom();
            var empty = new string(' ', Console.WindowWidth - 1);
            CursorBottom();
            Console.Write(text);
            // Restore previous position
            Console.SetCursorPosition(x, y);
        }

        public static void WriteBinary()
        {
            Console.Write(GenericOperations.GetRandom().Next(0, 2));
            Thread.Sleep(10);
        }

        #endregion

        #region Specific Outputs

        public static void WriteMobInfo(Mob c)
        {
            Console.Write("[{0}] ", c.Type.ToString());
            var mobColor = c.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;
            var nameString = $"{c.Name} ";
            WriteColor(nameString, mobColor);
            Console.Write("has ");
            foreach (var s in c.Stats.Keys)
                if (!Data.Stats[s].Hidden)
                {
                    var portion = Data.Stats[s].Portion;
                    WriteLabeledStat(c, s, portion);
                    Console.Write(" ");
                }

            Console.WriteLine();
        }

        public static void WriteLabeledStat(Mob Mob, StatType stat, bool portion)
        {
            string result;
            if (portion)
                result = $"{Mob.Stats[stat].Value}/{Mob.Stats[stat].MaxValue} {Data.Stats[stat].Title}";
            else
                result = $"{Mob.Stats[stat].Value} {Data.Stats[stat].Title}";
            WriteColor(result, Data.Stats[stat].Color);
        }

        public static void WriteStatchange(Mob target, StatType type, int value)
        {
            var changeType = value < 0 ? "lost" : "gained";
            var who = $"{target.Name} {changeType} ";
            var whoColor = target.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;
            var changedStat = $"{Math.Abs(value)} {Data.Stats[type].Title}";

            WriteColor(who, whoColor);
            WritelineColor(changedStat, Data.Stats[type].Color);
        }

        public static string IteBought(Item item)
        {
            return $"{item.Name} bought";
        }

        public static string MoBought(Mob mob)
        {
            return $"{mob.Name} bought";
        }

        public static string MoCaught(Mob mob)
        {
            return $"{mob.Name} caught";
        }

        public static string TownToFilename(Town town)
        {
            return $"{town.Title.Replace(' ', '_')}.{FileType}";
        }

        #endregion

        #region Output message

        public static void WriteSelectedMob(Player player)
        {
            Console.Write(MobSelected);
            WriteMobInfo(player.Champion);
        }

        public static void WriteItemUsed(Item item)
        {
            Console.WriteLine($"{item.Name} {ItemUsed}");
        }


        public static void WritelineUsedAction(Mob caster, string action)
        {
            var whoColor = caster.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;

            WriteColor(caster.Name, whoColor);
            Console.WriteLine(" used {0}.", action);
        }

        #endregion

        #region Private Methods

        private static void CursorBottom()
        {
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Console.CursorLeft = 0;
        }

        private static void WriteColorTextMethod(string text, ConsoleColor color, bool writeOnly)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (writeOnly)
                Console.Write(text);
            else
                Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }

        #endregion

        #region Loading Bar

        public static void InitializeLoadingScreen(int progress, string message, bool encounter = false)
        {
            var barColor = encounter ? Settings.DefaulteEnemyColor : Settings.DefaultTextColor;
            if (encounter)
            {
                Console.Clear();
                WritelineColor(string.Format($"Loading interrupted, {message} appeared"), barColor);
                Console.WriteLine();
            }

            WriteColor(ConstructLoadingBar(progress), barColor);
            Console.CursorLeft = progress;
        }

        public static void AnimateLoadingBar(int progress) //one frame of loading bar animation
        {
            Console.CursorLeft = progress;
            Console.Write(barPart);
        }

        private static string ConstructLoadingBar(int progress)
        {
            var bar = new StringBuilder();
            var goal = Settings.DefaultWildernessGoal;

            for (var i = 0; i < goal; i++)
            {
                var barChar = i <= progress ? barPart : '_';
                bar.Append(barChar);
            }

            return bar.ToString();
        }

        #endregion
    }
}