using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MathNet.Numerics.Distributions;

namespace ConsomonApplication
{
    [Serializable]
    public class Wilderness : Location
    {
        private int exploredCurrent;
        private Town[] endpoints = new Town[2];

        private int[] encountersAt;
        private int[] loadingStuckAt;

        private int encounterIndex = 0;

        private int EncounterIndex
        {
            get
            {
                if (encounterIndex > encountersAt.Length)
                    return encountersAt.Length;
                return encounterIndex;
            }
            set
            {
                encounterIndex = value;
            }
        }

        public Town[] Endpoints { get { return endpoints; } }



        public Wilderness(Town start, Town finish) //parameters are not in a form of a collection for better usability and readability
        {
            endpoints[0] = start;
            endpoints[1] = finish;

            level = CalculateLevel(endpoints[0], endpoints[1]);
            title = $"{Output.LoadingLabel} {Output.TownToFilename(finish)}...";

            encountersAt = CalculateLoadingEvents(CalculateEncounterAmount());
            loadingStuckAt = CalculateLoadingEvents(GenericOperations.GetRandom().Next(0, Settings.MaxStuckTimes));

            description = $"{Output.DidYouKnowSentence} {Output.ProTips[GenericOperations.GetRandom().Next(0, Output.ProTips.Count)]}";

            screen = ScreenType.wilderness;
            
        }

        public static float CalculateLevel(Town town1, Town town2)
        {
            //simple average fo two endpoints levels
            return (town1.Level + town2.Level) / 2f;
        }

        private double CalculateMean()
        {
            double levelRatio = GenericOperations.GetRatio(Settings.MinLevel, Settings.MaxLevel, Level);
            return GenericOperations.GetProportion(Settings.MinEncounters, Settings.MaxEncounters, levelRatio);
        }

        private int[] CalculateLoadingEvents(int amount)
        {
            int []result = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                while(true) //ensure there are no duplicate values
                {
                    int encIndex = GenericOperations.GetRandom().Next(0, Settings.DefaultWildernessGoal);
                    if(!result.Contains(encIndex))
                    {
                        result[i] = encIndex;
                        break;
                    }
                }
            }
            Array.Sort(result);
            return result;
        }


        public void Explore(Player player)
        {
            Output.InitializeLoadingScreen(exploredCurrent, "");
            int loadingDelay = GenericOperations.GetRandom().Next(Settings.MinLoadingDelay, Settings.MaxLoadingDelay);

            int nextStuck = 0;

            int nextEncounter = (encounterIndex < encountersAt.Length) ? encountersAt[encounterIndex] : 0;

            if (loadingStuckAt.Length > 0)
                nextStuck = loadingStuckAt[0];

            while (exploredCurrent <= Settings.DefaultWildernessGoal)
            {
                Output.AnimateLoadingBar(exploredCurrent);
                Thread.Sleep(loadingDelay);
                exploredCurrent++;
                if (nextEncounter == exploredCurrent)
                {
                    EncounterIndex++;
                    GenerateEncounter(player);
                    return;
                }

                if (exploredCurrent == nextStuck)
                {
                    Thread.Sleep(Settings.LoadingStuckDelay);
                    loadingDelay = GenericOperations.GetRandom().Next(Settings.MinLoadingDelay, Settings.MaxLoadingDelay);
                    loadingStuckAt = loadingStuckAt.Skip(1).ToArray();
                    if (loadingStuckAt.Length > 0)
                        nextStuck = loadingStuckAt[0];
                }
            }
            player.ChangeLocation(endpoints[1]);
        }


        private void GenerateEncounter(Player p)
        {
            MusicPlayer.PlayTrack(MusicPlayer.BattleTheme);
            Mob enemy = Mob.InstantiateMob(SelectEncounterMob());
            Encounter encounter = new Encounter( enemy, p);

            Console.Clear();
            Output.InitializeLoadingScreen(exploredCurrent, enemy.Name, true);
            UI.Pause();

            p.ChangeLocation(encounter);
            encounter.BeginCombat(p);
        }

        private int CalculateEncounterAmount()
        {
            //gauss distribution
            double mean = CalculateMean();
            double deviation = Settings.EncountersDeviation;
            Normal normalDistribution = new Normal(mean, deviation);

            int result = (int)Math.Round(normalDistribution.Sample());
            if (result < Settings.MinEncounters)
                result = Settings.MinEncounters;
            return result;
        }


        private Mob SelectEncounterMob()
        {
            float percentage = GenericOperations.FetchRandomPercentage();

            List<Mob> encounterable;

            if(percentage < Settings.NativeWildlifeChance / 2f)
            {
                encounterable = Data.Mobs.Where(t => t.Type == endpoints[0].TownType).ToList();
            }
            else if(percentage < Settings.NativeWildlifeChance)
            {
                encounterable = Data.Mobs.Where(t => t.Type == endpoints[1].TownType).ToList();
            }
            else
            {
                encounterable = Data.Mobs;
            }
            return PickMob(encounterable);
        }

        private Mob PickMob(List<Mob> list) //Picks csm with appropriate level from filtered and sorted list
        {
            //gauss distribution
            double mean = Level;
            double deviation = Settings.EncounterLevelDeviation;
            Normal normalDistribution = new Normal(mean, deviation);

            float desiredLevel = (float)normalDistribution.Sample();

            Mob result = list.Aggregate((a, b) => Math.Abs(a.Level - desiredLevel) < Math.Abs(b.Level - desiredLevel) ? a : b); //finds csm closest to desired level
            return result;
        }

        public override ISupplyable[] GetDynamicCollection()
        {
            return new ISupplyable[0];
        }

        public override void UpdateDescription(Player p)
        {
            
        }
    }
}
