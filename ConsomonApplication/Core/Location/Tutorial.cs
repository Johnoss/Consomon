using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    [Serializable]
    public class Tutorial : Location
    {
        public Tutorial(string title, string description)
        {
            this.title = title;
            this.description = description;
            screen = ScreenType.Starting;

            level = 0;
        }
        public override ISupplyable[] GetDynamicCollection()
        {
            return GetStartingMobs();
        }

        public static ISupplyable[] GetStartingMobs()
        {
            List<ISupplyable> result = new List<ISupplyable>();
            foreach (MobType mt in Enum.GetValues(typeof(MobType)))
            {
                result.Add(Data.Mobs.Where(m => m.Type == mt).Aggregate((a, b) => a.Level < b.Level ? a : b));
            }
            return result.ToArray();
        }

        public override void UpdateDescription(Player player)
        {

        }

        public void Explore(Player p) //Not generic enough
        {
            Output.WriteCleanPause($"Welcome to {Data.MobLabel}");
            Output.WriteGenericText($"This game uses the autosave feature.");
            Output.WriteGenericText($"I don't care if you turn off your pc while saving.");
            UI.Pause();
            Output.WriteCleanPause($"Let's get you set up with your first {Data.MobLabel}.");
            title ="Which of the following words you fancy the most?";
            description = "Press a corresponding key";
            UI.InitializeScreen(p);
            p.ReadInput();
            Console.Clear();
            Controls.SelectMobAction(p, 0);
            Output.WriteCleanPause($"Your new {Data.MobLabel} is {p.Champion.NameRaw}");
            Console.Clear();
            Output.WriteGenericText("Look how cute it is.");
            Output.WriteGenericText("This is it's binary representation:");
            Thread.Sleep(400);
            for (int i = 0; i < Console.WindowWidth * 2; i++)
            {
                Output.WriteBinary();

            }
            UI.Pause();
            Output.WriteCleanPause($"It has {p.Champion.Stats[StatType.Health].Value} {Data.Stats[StatType.Health].Title}");
            Output.WriteGenericText($"The {Data.Stats[StatType.Health].Title} represent it's overall health.");
            UI.Pause();
            Output.WriteGenericText($"If a {Data.MobLabel} runs out of pixels, it'll throw an exception");
            Output.WriteGenericText($"and won't be able to fight until healed.");
            UI.Pause();
            Output.WriteCleanPause($"It also has {p.Champion.Stats[StatType.Energy].Value} {Data.Stats[StatType.Energy].Title}");
            Output.WriteGenericText($"The {Data.Stats[StatType.Energy].Title} are used for special abilities.");
            UI.Pause();
            Output.WriteGenericText($"Every {Data.MobLabel} has special abilities and it needs {Data.Stats[StatType.Energy].Title} to use them.");
            UI.Pause();
            Output.WriteGenericText($"You will discover more abilities as you catch more {Data.MobLabel}.");
            UI.Pause();
            Output.WriteCleanPause($"It also has {p.Champion.Stats[StatType.Defence].Value} {Data.Stats[StatType.Defence].Title} and {p.Champion.Stats[StatType.Attack].Value} {Data.Stats[StatType.Attack].Title}");
            Output.WriteGenericText($"Those stats determine the damage your {Data.MobLabel} receives and inflicts.");
            UI.Pause();
            Output.WriteCleanPause($"Travel around, catch more {Data.MobLabel}s and become the master of this shit.");
            Output.WriteCleanPause("\"shit\"");
            Output.WritelineColor("ERROR: 'shit' threw an exception. Profanity filter not implemented.", Settings.DefaulteEnemyColor);
            UI.Pause();
            Output.WritelineColor("Aborting Tutorial...", Settings.DefaulteEnemyColor);
            UI.Pause();
            Output.WritelineColor("Good Luck", ConsoleColor.Green);
            UI.Pause();

            Wilderness w = new Wilderness(Data.Towns[0], Data.Towns[1]);
            p.ChangeLocation(w);
            w.Explore(p);
            
        }


    }
}
