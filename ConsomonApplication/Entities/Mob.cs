using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using System.Linq;

namespace ConsomonApplication
{
    [Serializable]
    public class Mob : Actor, ISupplyable, IListable
    {


        public Ability[] UsableAbilities { get { return abilities.Where(a => a.Cost <= Stats[StatType.Energy].Value).ToArray(); }  }

        public Mob(MobType type, string name, int health, int energy, int defence, int attack, int abilityMultiplyer, Ability[] abilities)
        {


            this.name = name;
            this.type = type;
            this.abilities = abilities;
            Stats.Add(StatType.Health, new Stat(health, health, 0));
            Stats.Add(StatType.Energy, new Stat(energy, energy, 0));
            Stats.Add(StatType.Defence, new Stat(Settings.MaxDefence, defence, Settings.MinDefence));
            Stats.Add(StatType.Attack, new Stat(Settings.MaxAttack, attack, Settings.MinAttack));
            Stats.Add(StatType.AbilityMp, new Stat(Settings.MaxAbilityMP, abilityMultiplyer, Settings.MinAbilityMP));
            target = null;
            wild = true;

            //calculate level - a simple arithmetic mean of all stats
            float sum = 0;
            foreach(Stat s in Stats.Values)
            {
                sum += s.Value;
            }
            level = sum / Stats.Count;

            price = (int)Math.Round(level * Settings.MobPriceMultiplier);
        }

        public void ModifyStat(StatType stat, int value)
        {
            Stats[stat].Value += value;
            Output.WriteStatchange(this, stat, value);
        }

        public void ResetStat(StatType stat)
        {
            int difference = Stats[stat].DefaultValue - Stats[stat].Value;
            ModifyStat(stat, difference);
        }

        public void ResetMob(bool battleStatsOnly = false)
        {
            foreach (KeyValuePair<StatType, Stat> s in Stats)
            {
                if (!(battleStatsOnly && (s.Key == StatType.Health || s.Key == StatType.Energy)))
                    s.Value.Value = s.Value.DefaultValue;
            }
            actionsLeft = Settings.defaultActions;
        }

        public string GetLabel(Screen s)
        {
            if(s == Data.Screens[ScreenType.Starting])
            {
                return type.ToString();
            }
            string priceLabel = (s == Data.Screens[ScreenType.SelectMob]) ? "" : $"({price}{Output.CurrencyName[0]}) ";
            return $"{priceLabel}[{type}] {NameRaw} {Stats[StatType.Health].Value}/{Stats[StatType.Energy].Value}/{Stats[StatType.Defence].Value}/{Stats[StatType.Attack].Value}";
        }

        public ISupplyable[] GetDynamicCollection()
        {
            List<ISupplyable> result = new List<ISupplyable>();
            foreach(Ability a in UsableAbilities)
            {
                result.Add(a);
            }
            return result.ToArray();
        }

        //Clone object from Data

        public static Mob InstantiateMob(Mob mold, bool wild = true)
        {
            Mob clone = new Mob(mold.Type, mold.NameRaw, mold.Stats[StatType.Health].MaxValue, mold.Stats[StatType.Energy].MaxValue, mold.Stats[StatType.Defence].Value, mold.Stats[StatType.Attack].Value, mold.Stats[StatType.AbilityMp].Value, mold.abilities)
            {
                Wild = wild
            };
            return clone;
        }

        //Combat related stuff
        #region Combat
   



        public void Attack()
        {
            int damage = Stats[StatType.Attack].Value - target.Stats[StatType.Defence].Value;


            bool strong = GenericOperations.CalculateTypeAdvantage(Type, Target.Type);

            Output.WritelineUsedAction(this, Output.AttackLabel);
            if (strong)
                Output.WritelineColor(Output.StrongAttackSentence, Settings.DefaultStrongColor);

            double randomizedDamage = new Normal(damage, Settings.DamageDeviation).Sample();
            damage = (int)Math.Round(randomizedDamage);

            if (damage < Settings.MinDamage)
                damage = Settings.MinDamage;

            int targetHealth = target.Stats[StatType.Health].Value;
            if (damage > targetHealth)
                damage = targetHealth;

            if(strong)
                damage *= (int)Math.Ceiling(Settings.StrongMP);

            target.ModifyStat(StatType.Health, -damage);
            actionsLeft--;
        }

        //use an ability (increase or decrease stats of target by given parameters)
        public void UseAbility(Ability ability)
        {
            Output.WritelineUsedAction(this, ability.Title);
            ModifyStat(StatType.Energy, -Settings.AbilityPrice);
            foreach (AbilityElement ae in Data.AbilityTemplates[ability.Template].Elements)
            {
                Mob tg = ae.Self ? this : target; //whom to target
                int baseValue = Settings.DefaultAbilityValue;  //default base value by game settings
                baseValue = ae.Increase ? baseValue : baseValue * -1; //increase or decrease?
                double modifier = Stats[StatType.AbilityMp].Value / 100.0; //apply Ability multiplier stat
                int calculatedValue = (int)Math.Ceiling(baseValue * modifier); //result value int
                tg.ModifyStat(ae.Stat, calculatedValue); //execute
            }
            actionsLeft--;
        }
        #endregion


        public void Buy(Player player, bool free = false)
        {
            player.OwnedMobs.Add(InstantiateMob(this, false));
            if(!free)
                player.Money -= Price;
        }

        //AI related stuff
        #region AI
        public void BattleDecideMove()
        {
            Random rnd = new Random();
            int percentage = rnd.Next(1, 100);
            if (Stats[StatType.Energy].Value >= Settings.AbilityPrice && percentage < Settings.EnemyAgression) 
            {
                UseAbility(DecideAbility());
            }
            else
            {
                Attack(); //enemy never flees
            }
        }

        public Ability DecideAbility()
        {
            Ability favoredAbility = new Ability();

            int leadingFavorPoints = int.MinValue; //used for storing the ability with highest favor rate
            Ability[] shuffledAbilities = GenericTypeOperations<Ability>.ShuffleArray(abilities); //shuffle abilities so we not always use the same one when the favorpoints equals

            foreach (Ability a in abilities) //get favor value of each ability, store the outmatching one
            {
                int favorPoints = CalculateFavorPoints(Data.AbilityTemplates[a.Template].Elements);
                if (leadingFavorPoints < favorPoints)
                {
                    leadingFavorPoints = favorPoints;
                    favoredAbility = a;
                }
            }
            return favoredAbility;
        }

        private int CalculateFavorPoints(AbilityElement[] elements)
        {
            int favorPoints = 0;
            foreach (AbilityElement ae in elements) //get favor value of each element in an ability
            {
                int selfMultiplier = ae.Self ? -1 : 1; //if the target is self, reverse favor gain
                int increaseMultiplier = ae.Increase ? -1 : 1; //if the ability is increasing values, reverse favor gain
                Mob tg = ae.Self ? this : Target ; //determine target
                if (ae.Increase)
                    favorPoints = tg.Stats[ae.Stat].MaxValue > tg.Stats[ae.Stat].Value ? 1 : -1;
                else
                    favorPoints = tg.Stats[ae.Stat].MinValue < tg.Stats[ae.Stat].Value ? 1 : -1;

                favorPoints *= selfMultiplier * increaseMultiplier; //apply multipliers
            }
            return favorPoints;
        }




        #endregion


    }

}
