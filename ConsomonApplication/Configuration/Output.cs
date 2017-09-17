using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    //String handeling class. The purpouse of this file is to have all the strings and all the Console... commands (but Console.Clear) in one file
    static class Output
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
        public static string SelectMobLabel = $"Select {Data.MobLabel}";
        public static string SelectItemLabel = $"Use {ItemLabel}";
        public static string HealMobsLabel = $"Heal all {Data.MobLabel}s";
        public const string FileType = "csm";
        public const string CancelLabel = "Cancel";

        public const string ItemLabel = "Item";

        public const string CapturedLabel = "captured";
        public const string BoughtLabel = "bought";
        public const string DefeatedLabel = "threw an exception";


        public const string ShopName = "Shop";
        public const string HealingCName = "Healing Centre";
        public static string MobDealerName = $"{Data.MobLabel} Dealer";

        public const string YouGotLabel = "You got";
        public static string YouHaveLabel = "You have";
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
        public static string ReturnedSentence = $"{LoadingLabel} interrupted. Rolling back to";
        public static string FleeSuccessSentence = "When danger reared it's ugly head, you bravely turned your tail and fled.";
        public static string FleeFailureSentence = "You weren't able to flee.";
        public static string CaptureFailureSentence = "You weren't able to capture the enemy.";

        public const string YourTurnLabel = "Your Turn:";
        public const string EnemyTurnLabel = "Enemy's Turn:";

        public static string MobsHealedSentence = $"All {Data.MobLabel}s healed.";
        public static string AllMobsDefeatedSentence = $"All your {Data.MobLabel}s {DefeatedLabel}. You should visit a healing centre";

        public const string YouLostSentence = "You Lost";
        public const string YouWonSentence = "You Won";

        public const string MobSelected = "Selected: ";
        public const string ItemUsed = "used";

        public const string NoTarget = "No target";

        public static string NotEnoughMoney = $"Not enough {CurrencyName}s";

        public const string Separator = "-------------------------------";
        private const char barPart = '█';

        public const string DidYouKnowSentence = "Did you know?";

        public static List<string> ProTips = new List<string>
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
            $"The less {Data.Stats[StatType.health].Title} the {Data.MobLabel} has, the easier it is to catch.",
        };

        #region Generic Outputs

        public static void WriteGenericText(string text)
        {
            Console.WriteLine(text);
        }

        public static string ComposeGenericText(string[] args)
        {
            StringBuilder result = new StringBuilder();
            foreach(string s in args)
            {
                result.Append($"{s} ");
            }
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
            StringBuilder result = new StringBuilder();
            foreach(KeyValuePair<ConsoleKey, string> ac in player.ControlScheme)
            {
                result.Append($"{ac.Key.ToString()} {ac.Value}   ");

            }
            WriteOnBottomLine(result.ToString());
        }

        public static void TranslateControls(Player player)
        {
            foreach (KeyValuePair<ConsoleKey, string> ac in player.ControlScheme)
            {
                StringBuilder result = new StringBuilder();
                result.Append(ac.Key.ToString());
                result.Append(" - ");
                result.Append(ac.Value);
                Console.WriteLine(result);
            }
        }

        public static void WriteOnBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;

            CursorBottom();
            string empty = new string(' ', Console.WindowWidth - 1);
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
            ConsoleColor mobColor = c.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;
            string nameString = string.Format("{0} ", c.Name);
            WriteColor(nameString, mobColor);
            Console.Write("has ");
            foreach(StatType s in c.Stats.Keys)
            {
                if(!Data.Stats[s].Hidden)
                {
                    bool portion = Data.Stats[s].Portion;
                    WriteLabeledStat(c, s, portion);
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }

        public static void WriteLabeledStat(Mob Mob, StatType stat, bool portion)
        {
            string result;
            if(portion)
                result = string.Format("{0}/{1} {2}", Mob.Stats[stat].Value, Mob.Stats[stat].MaxValue, Data.Stats[stat].Title);
            else
                result = string.Format("{0} {1}", Mob.Stats[stat].Value, Data.Stats[stat].Title);
            WriteColor(result, Data.Stats[stat].Color);
        }

        public static void WriteStatchange(Mob target, StatType type, int value)
        {
            string changeType = value < 0 ? "lost" : "gained";
            string who = string.Format("{0} {1} ", target.Name, changeType);
            ConsoleColor whoColor = target.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;
            string changedStat = string.Format("{0} {1}", Math.Abs(value), Data.Stats[type].Title);

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
            ConsoleColor whoColor = caster.Wild ? Settings.DefaulteEnemyColor : Settings.DefaultePlayerColor;

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
            ConsoleColor previousColor = Console.ForegroundColor;
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
            ConsoleColor barColor = encounter? Settings.DefaulteEnemyColor : Settings.DefaultTextColor;
            if(encounter)
            {
                Console.Clear();
                WritelineColor(String.Format($"Loading interrupted, {message} appeared"), barColor);
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
            StringBuilder bar = new StringBuilder();
            int goal = Settings.DefaultWildernessGoal;

            for (int i = 0; i < goal; i++)
            {
                char barChar = (i <= progress) ? barPart : '_';
                bar.Append(barChar);
            }
            return bar.ToString();
        }

        #endregion

        #region Town

        public static void WriteTownInfo(Town town)
        {
            Console.WriteLine($"You are in {town.Title}. The folks around here worship {town.TownType} gods.");
        }

        #endregion
    }
}
