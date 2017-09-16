using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public abstract class Actor
    {
        protected string name;
        protected MobType type;
        protected Ability[] abilities;
        protected Mob target;
        protected int actionsLeft = Settings.defaultActions;
        protected int price;

        protected float level;

        protected bool wild;

        public Dictionary<StatType, Stat> Stats = new Dictionary<StatType, Stat>();

        //read only
        public Ability[] Abilities { get { return abilities; } }
        public MobType Type { get { return type; } }
        public float Level { get { return level; } }
        public string NameRaw { get { return name; } }
        public string Name
        {
            get
            {
                string result;
                string prefix;
                if (wild)
                    prefix = Output.WildLabel;
                else
                    prefix = Output.PlayersLabel;
                result = string.Format("{0} {1}", prefix, name);
                return result;
            }
        }
        //read-write
        public Mob Target { get { return target; } set { target = value; } }
        public bool Wild { get { return wild; } set { wild = value; } }
        public int ActionsLeft { get => actionsLeft; set => actionsLeft = value; }
        public int Price { get => price; set => price = value; }
    }
}
