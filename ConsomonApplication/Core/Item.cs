using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public interface IUsable
    {
        void Use(Player player);
        void Deplete(Player player);
    }
    [Serializable]
    public abstract class Item : IUsable, ISupplyable
    {
        protected string name;
        protected string description;
        protected int price;

        public string Description { get { return description; } }
        public string Name { get { return name; } }
        public int Price { get { return price; } }

        public abstract void Use(Player player);

        public void Buy(Player player)
        {
            if (player.Inventory.Keys.Contains(this))
                player.Inventory[this]++;
            else
                player.Inventory.Add(this, 1);

            player.Money -= price;
        }

        public string GetLabel(Screen s)
        {

            string priceLabel = (s == Data.Screens[ScreenType.SelectMob]) ? "" : $"({price}{Output.CurrencyName[0]}) ";
            return $"{priceLabel}{name} - {description}";
        }

        public void Deplete(Player player)
        {
            if (player.Inventory.ContainsKey(this))
                player.Inventory[this]--;
        }

        protected string GetItemUsedMessage()
        {
            return Output.ComposeGenericText(new string[] { name, Output.ItemUsed });
        }
    }
    [Serializable]
    public class StatResetter : Item //reset stat(s) to player's selected Mob
    {
        StatType[] statsToReset; //for simplicity, only restore stat to full, not partially


        public StatResetter(string name, int price, string description, StatType[] statsToReset)
        {
            this.description = description;
            this.name = name;
            this.statsToReset = statsToReset;
            this.price = price;
        }


        public override void Use(Player player)
        {
            Mob selectedCSM = player.Champion;
            foreach(StatType s in statsToReset)
            {
                selectedCSM.ResetStat(s);
            }
            Output.WriteGenericText(GetItemUsedMessage());
            Deplete(player);
        }
    }

    [Serializable]
    public class MobContainer : Item
    {
        public MobContainer(string name, int price, string description)
        {
            this.description = description;
            this.name = name;
            this.price = price;
        }

        public override void Use(Player player)
        {
            Mob target = player.Champion.Target;

            if (target == null)
            {
                Output.WriteGenericText(Output.NoTarget);
            }
            else
            {
                if(GenericOperations.FetchRandomPercentage() <= (CalculateChance(target)))
                {
                    target.ResetMob();

                    Output.WriteGenericText(GetItemUsedMessage());

                    Deplete(player);

                    player.OwnedMobs.Add(Mob.InstantiateMob(target, false));
                    if(player.CurrentLocation is Encounter e)
                    {
                        e.FinishCombat(player, Output.ComposeGenericText(new string[] { target.Name, Output.CapturedLabel }), true);
                        return;
                    }
                    target = null;
                    Controls.GoBackAction(player);
                }
                else
                {
                    Output.WriteCleanPause(Output.CaptureFailureSentence);
                }
            }
        }

        private double CalculateChance(Mob target)
        {
            Stat health = target.Stats[StatType.Health];
            Stat defence = target.Stats[StatType.Defence];
            float coeficient = -2;
            double lifeRatio = GenericOperations.GetRatio(health.MinValue, health.MaxValue, health.Value);
            double baseChance = lifeRatio*(lifeRatio + coeficient) + 1;
            double defenceRatio = GenericOperations.GetRatio(defence.DefaultValue * .75, defence.MaxValue, defence.Value);
            double result = baseChance - defenceRatio;
            if (result < 0)
                result = 0;
            return result;
        }
    }
}
