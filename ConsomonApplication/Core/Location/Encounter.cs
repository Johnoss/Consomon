using System;

namespace ConsomonApplication
{
    [Serializable]
    public class Encounter : Location
    {
        private Mob enemy = null;
        public Mob Enemy { get => enemy; set => enemy = value; }
        private bool combatInProgress = true;
        Player p;

        ISupplyable[] abilities;

        public Encounter(Mob enemy, Player p)
        {
            this.p = p;
            screen = ScreenType.Battle;
            this.enemy = enemy;
            Title = $"Encounter with {enemy.Name}";
        }

        public override ISupplyable[] GetDynamicCollection()
        {
            return p.Champion.GetDynamicCollection();
        }
        public void SetTargets(Player p, Mob enemy)
        {
            Mob champion = p.Champion;

            enemy.Target = champion;
            champion.Target = enemy;

        }

        public void BeginCombat(Player p)
        {
            while(combatInProgress)
            {
                DoTurn(p);
            }
        }

        private void DoTurn(Player p) //does turn and returns true if the combat is finished
        {
            //UI.InitializeScreen(p);
            Mob champion = p.Champion;

            SetTargets(p, enemy);

            champion.ActionsLeft = Settings.defaultActions;
            enemy.ActionsLeft = Settings.defaultActions;

            while (CheckActions(champion) > 0)
            {
                if (!CheckPulse(champion))
                {
                    if (p.SelectableMobs.Count > 0)
                    {
                        Output.WriteGenericText(Output.ComposeGenericText(new string[] { champion.Name, Output.DefeatedLabel }));
                        UI.Pause();
                        Controls.SelectMobScreen(p);
                        p.ReadInput();
                        break;
                    }
                    else
                    {
                        FinishCombat(p, Output.AllMobsDefeatedSentence);
                        return;
                    }
                }
                if (CheckActions(champion) > 0)
                {
                    UI.InitializeScreen(p);
                    p.ReadInput();
                }
                else
                    break;
            }

            //Enemy turn
            Output.WriteGenericText(Output.EnemyTurnLabel);
            while (CheckActions(enemy) > 0)
            {
                if (!CheckPulse(enemy))
                {
                    FinishCombat(p, Output.ComposeGenericText(new string[] { enemy.Name, Output.DefeatedLabel }), true);
                    return;
                }
                enemy.BattleDecideMove();
                UI.Pause();
            }
            return;
        }

        public void FinishCombat(Player p, string message, bool won = false)
        {
            combatInProgress = false;
            MusicPlayer.PlayTrack(MusicPlayer.MainTheme);
            p.Champion.ResetMob(true);
            Console.Clear();
            Output.WriteGenericText(message);
            UI.Pause();
            if (won)
            {
                Output.WriteGenericText(Output.YouWonSentence);
                GenerateRewards(p, enemy);
            }
            else
            {
                Output.WriteGenericText(Output.YouLostSentence);
                Output.WriteGenericText($"{Output.ReturnedSentence} {p.LastTown.Title}");
                UI.Pause();
                p.Retreat();
                return;
            }
            Console.Clear();
            Controls.GoBackAction(p);
            UI.InitializeScreen(p);

            if (p.CurrentLocation is Wilderness w)
                w.Explore(p);

        }

        public override void UpdateDescription(Player player)
        {

        }

        public void WriteCombatantsInfo(Player player)
        {
            Output.WriteMobInfo(player.Champion);
            Output.WriteMobInfo(enemy);
        }

        private static bool CheckPulse(Mob patient)
        {
            Stat health = patient.Stats[StatType.Health];
            return health.Value > health.MinValue;
        }

        private static int CheckActions(Mob m)
        {
            return m.ActionsLeft;
        }

        private static void GenerateRewards(Player p, Mob m)
        {
            //generate money
            int money = CalculateRewardMoney(m.Level);
            Output.WriteCleanPause( Output.ComposeGenericText(new string[] { Output.YouGotLabel, money.ToString(), Output.CurrencyName }));
            p.Money += money;
        }

        private static int CalculateRewardMoney(float level)
        {
            float baseAmount = level * Settings.RewardLevelMultiplier + Settings.RewardDeviation;
            return GenericOperations.RoundToStepInt(baseAmount);
        }
    }
}
