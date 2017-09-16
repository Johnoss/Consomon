using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public abstract class Location : ISupplyable, IListable
    {
        protected float level;
        protected string title;
        protected string description;

        protected ScreenType screen;

        public float Level { get => level; set => level = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public ScreenType Screen { get => screen; set => screen = value; }

        public virtual string GetLabel(Screen s)
        {
            return Title;
        }

        public abstract void UpdateDescription(Player player);

        public abstract ISupplyable[] GetDynamicCollection();

    }

}
