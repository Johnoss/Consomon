using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    [Serializable]
    public struct Ability : ISupplyable
    {
        private string title;
        private int template; //id of GameData Abilities Tamplates
        private int cost;

        public string Title { get => title; set => title = value; }
        public int Template { get => template; set => template = value; }
        public int Cost { get => cost; set => cost = value; }

        public Ability (int template, string title, int cost = 1)
        {
            this.title = title;
            this.template = template;
            this.cost = cost;
        }

        public string GetLabel(Screen s)
        {
            return Title;
        }
    }

    public struct AbilityElement
    {
        public enum SpecialElement { none, selfdestruct, selfharm, reset }

        public StatType Stat;
        public bool Self;
        public bool Increase;
        public SpecialElement Special;

        public AbilityElement(StatType stat, bool self, bool increase, SpecialElement special = SpecialElement.none)
        {
            Stat = stat;
            Self = self;
            Increase = increase;
            Special = special;
        }
    }

    public struct AbilityTemplate
    {
        public AbilityElement[] Elements;
        public AbilityTemplate (AbilityElement[] elements)
        {
            Elements = elements;
        }
    }
    
}
