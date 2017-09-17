using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public interface ISupplyable
    {
        string GetLabel(Screen s);
    }
    public interface IListable
    {
        ISupplyable[] GetDynamicCollection();
    }
    [Serializable]
    public class Screen
    {
        private string label;
        private string dynamicLabel;
        private bool bottomControls;
        private Action<Player, int> dynamicAction;
        private ISupplyable[] sourceColl;
        private ConsoleKey[] staticControls;

        public string Label { get => label; }
        public string DynamicLabel { get => dynamicLabel; }
        public Action<Player, int> DynamicAction { get => dynamicAction; }
        public ConsoleKey[] StaticControls { get => staticControls; }
        public ISupplyable[] SourceColl { get => sourceColl; set => sourceColl = value; }
        public bool BottomControls { get => bottomControls; set => bottomControls = value; }

        public Screen(string label, string dynamicLabel, Action<Player, int> dynamicAction, ConsoleKey[] staticControls, bool bottomControls = false)
        {
            this.label = label;
            this.dynamicLabel = dynamicLabel;
            this.dynamicAction = dynamicAction;
            this.staticControls = staticControls;
            this.BottomControls = bottomControls;
        }
    }
}
