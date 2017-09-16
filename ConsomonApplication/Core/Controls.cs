using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public static class Controls
    {
        private static Dictionary<ConsoleKey, ActionControlDynamic> dynamicControls = new Dictionary<ConsoleKey, ActionControlDynamic>();

        private static int startingKeyIndex = (int)ConsoleKey.F1;
        private static int currentKeyIndex = startingKeyIndex;

        public static void ResetControls()
        {
            dynamicControls = new Dictionary<ConsoleKey, ActionControlDynamic>();
            currentKeyIndex = startingKeyIndex;
        }

        public static ConsoleKey AddDynamicControl(ActionControlDynamic acd)
        {
            ConsoleKey key = (ConsoleKey)currentKeyIndex;
            dynamicControls.Add((ConsoleKey)currentKeyIndex, acd);
            currentKeyIndex++;
            return key;
        }

        public static void InvokeStaticAction(ConsoleKey key, Player p)
        {
            if(Settings.KeyBindings.ContainsKey(key))
            {
                Settings.KeyBindings[key].Action.Invoke(p);
                return;
            }
            if (dynamicControls.ContainsKey(key))
            {
                dynamicControls[key].Action.Invoke(p, dynamicControls[key].Index);
                return;
            }
        }

        public static void TranslateInput(ConsoleKey input, Player p)
        {
            if (Settings.KeyBindings.ContainsKey(input) || dynamicControls.ContainsKey(input))
                InvokeStaticAction(input, p);
        }

        #region Actions

        //non-dynamic actions

        public static void ResetScreen(Player p)
        {
            p.ChangeScreen(Data.Screens[p.CurrentLocation.Screen], p.CurrentLocation);
            UI.InitializeScreen(p);
        }
        public static void GoBackAction(Player p)
        {
            p.ChangeLocation(p.PreviousLocation);
            return;
        }

        public static void AttackAction(Player p)
        {
            Console.Clear();
            Output.WriteGenericText(Output.YourTurnLabel);
            p.Champion.Attack();
            Output.WritelineColor(Output.Separator, ConsoleColor.White);
            UI.Pause();
        }

        public static void HealMobsAction(Player p)
        {
            foreach (Mob csm in p.OwnedMobs)
            {
                csm.ResetMob();
            }
            Output.WriteCleanPause(Output.MobsHealedSentence);
        }

        public static void FleeAction(Player p)
        {
            if(GenericOperations.FetchRandomPercentage() <  Settings.FleeChance)
            {
                Console.Clear();
                if(p.CurrentLocation is Encounter e)
                {
                    e.FinishCombat(p, Output.FleeSuccessSentence);
                }
            }
            else
            {
                Output.WriteCleanPause(Output.FleeFailureSentence);
                p.Champion.ActionsLeft--;
            }
        }


        public static void SelectMobScreen(Player p)
        {
            p.ChangeScreen(Data.Screens[ScreenType.selectMob], p);
            UI.InitializeScreen(p);
            p.ReadInput();
            ResetScreen(p);
        }


        public static void SelectItemScreen(Player p)
        {
            p.ChangeScreen(Data.Screens[ScreenType.selectItem], p);
            UI.InitializeScreen(p);
            p.ReadInput();
            ResetScreen(p);
        }

        public static void TravelScreen(Player p)
        {
            if (p.CurrentLocation is Town town)
            {
                p.ChangeLocation(town.Crossroad);
            }
        }

        //dynamic actions


        public static void UseAbilityAction(Player p, int index)
        {
            Console.Clear();
            Output.WriteGenericText(Output.YourTurnLabel);
            p.Champion.UseAbility(p.Champion.Abilities[index]);
            Output.WritelineColor(Output.Separator, ConsoleColor.White);
            UI.Pause();
        }
        public static void BuyItemAction(Player p, int index)
        {
            if (p.CurrentLocation is Establishment e)
            {
                Item item = (Item)e.Stock[index];
                if (CanAfford(p, item.Price))
                {
                    item.Buy(p);
                    Output.WriteCleanPause(Output.ComposeGenericText(new string[] { item.Name, Output.BoughtLabel }));
                }
                else
                    Output.WriteCleanPause(Output.CannotAffordSentence);

            }
        }

        public static void TravelAction(Player p, int index)
        {
            if(p.CurrentLocation is Crossroads cr)
            {
                int destinationIndex = cr.ConnectedTowns[index];
                Wilderness wilderness = cr.GenerateWilderness(destinationIndex);
                p.ChangeLocation(wilderness);
                UI.InitializeScreen(p);
                wilderness.Explore(p);
            }
        }

        public static void VisitAction(Player p, int index)
        {
            if(p.CurrentLocation is Town town)
            {
                p.PreviousLocation = p.CurrentLocation;
                Establishment est = town.Establishments[index];
                p.CurrentLocation = est;
                est.Description = Output.ComposeGenericText(new string[] { Output.YouHaveLabel, p.Money.ToString(), Output.CurrencyName });
                p.ChangeScreen(Data.Screens[est.Screen], est);
            }
        }

        public static void BuyMobAction(Player p, int index)
        {
            if (p.CurrentLocation is Establishment e)
            {
                Mob mob = (Mob)e.Stock[index];
                if (CanAfford(p, (int)Math.Round(mob.Level)))
                {
                    mob.Buy(p);
                    Output.WriteCleanPause(Output.ComposeGenericText(new string[] { mob.Name, Output.BoughtLabel }));
                }
                else
                    Output.WriteCleanPause(Output.CannotAffordSentence);

            }
        }

        public static void GetMobAction(Player p, int index)
        {
            ISupplyable mob = p.CurrentLocation.GetDynamicCollection()[index];
            if (mob is Mob m)
                m.Buy(p, true);
        }

        public static void UseItemAction(Player p, int index)
        {
            int i = 0;
            foreach(Item item in p.Inventory.Keys)
            {
                if (i == index)
                {
                    p.Champion.ActionsLeft--;
                    item.Use(p);
                    return;
                }
                i++;
            }
        }

        public static void SelectMobAction(Player p, int index)
        {
            p.Champion = p.SelectableMobs[index];


            p.Champion.ActionsLeft = 0;

            Output.WriteSelectedMob(p);
            UI.Pause();
            ResetScreen(p);
            if(p.CurrentLocation is Encounter cp)
            {
                cp.BeginCombat(p);
            }
        }



        #endregion

        private static bool CanAfford(Player p, int price)
        {
            return p.Money >= price;
        }
    }
}
