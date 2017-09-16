using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public struct ActionControl
    {
        public string Label;
        public Action<Player> Action;

        public ActionControl(string label, Action<Player> action)
        {
            Label = label;
            Action = action;
        }
    }

    public struct ActionControlDynamic
    {
        public string Label;
        public Action<Player, int> Action;
        public int Index;

        public ActionControlDynamic(string label, Action<Player, int> action, int index)
        {
            Label = label;
            Action = action;
            Index = index;
        }
    }

    public class KeyReadEventArgs : EventArgs
    {
        public ConsoleKey InputKey { get; set; }
    }

    public static class UI
    {

        public static void InitializeScreen(Player p)
        {
            Console.Clear();
            p.CurrentLocation.UpdateDescription(p);
            Output.WriteGenericText(p.CurrentLocation.Title);
            Output.WriteGenericText(p.CurrentLocation.Description);
            SpecificScreensInit(p);
            InitializeControls(p);
            if (p.CurrentScreen.BottomControls)
                Output.TranslateControlsBottom(p);
            else
                Output.TranslateControls(p);
        }

        private static void InitializeControls(Player p)
        {
            Screen s = p.CurrentScreen;
            ResetControls(p);
            p.ControlScheme = GetStaticControls(p.CurrentScreen); //firstly initialize static controls
            foreach(ActionControlDynamic acd in GetDynamicControls(s))
            {
                p.ControlScheme.Add(Controls.AddDynamicControl(acd), acd.Label);
            }
        }

        private static void ResetControls(Player p)
        {
            Controls.ResetControls();
            p.ControlScheme = new Dictionary<ConsoleKey, string>();
        }

        private static Dictionary<ConsoleKey, string> GetStaticControls(Screen s)
        {
            Dictionary<ConsoleKey, string> result = new Dictionary<ConsoleKey, string>();


            foreach (KeyValuePair<ConsoleKey, ActionControl> kvp in Settings.KeyBindings)
            {
                if(s.StaticControls.Contains(kvp.Key))
                    result.Add(kvp.Key, kvp.Value.Label);
            }
            return result;
        }

        private static List<ActionControlDynamic> GetDynamicControls(Screen s)
        {
            List<ActionControlDynamic> result = new List<ActionControlDynamic>();

            if(s.SourceColl?.Length > 0 )
                for (int i = 0; i < s.SourceColl.Length; i++)
                {
                    var row = s.SourceColl[i];
                    string label = $"{s.DynamicLabel} {row.GetLabel(s)}";
                    result.Add (new ActionControlDynamic(label, s.DynamicAction, i));
                }

            return result;
        }

        public static void Pause()
        {
            Output.WriteOnBottomLine(Output.ContinueSentence);
            Console.ReadKey(true);
        }

        public static void LockConsole()
        {
            Console.CursorVisible = false;
            int defaultHeight = Console.WindowHeight;
            Console.BufferHeight = defaultHeight;
        }

        private static void SpecificScreensInit(Player p)
        {
            if (p.CurrentLocation is Encounter e && p.CurrentScreen == Data.Screens[ScreenType.battle])
                e.WriteCombatantsInfo(p);
        }
    }
}
