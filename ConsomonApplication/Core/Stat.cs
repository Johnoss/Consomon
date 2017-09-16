using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    public class Stat
    {
        StatType Type { get => type; }
        private StatType type;

        private int maxValue;
        private int defaultValue;
        private int value;
        private int minValue;

        public int MaxValue { get { return maxValue; } }
        public int DefaultValue { get { return defaultValue; } }
        public int MinValue { get { return minValue; } }

        public int Value
        {
            get { return value; }
            set
            {
                this.value = value > maxValue ? maxValue : ( value < minValue ? minValue : value );
            }
        }

        public Stat(int maxValue, int value, int minValue)
        {
            type = StatType.health;

            defaultValue = value;
            this.maxValue = maxValue;
            this.value = value;
            this.minValue = minValue;
        }
    }

    public struct StatInfo
    {
        private string title;
        public string Title
        {
            get
            {
                string suffix = "";
                if (Plural)
                    suffix = "s";
                return string.Format("{0}{1}", title, suffix);
            }
        }
        public ConsoleColor Color;
        public bool Plural;
        public bool Portion;
        public bool Hidden;

        public StatInfo(string title, ConsoleColor color, bool plural, bool portion)
        {
            this.title = title;
            Color = color;
            Plural = plural;
            Portion = portion;
            Hidden = false;
        }

        public StatInfo(string title, ConsoleColor color, bool plural, bool portion, bool hidden)
        {
            this.title = title;
            Color = color;
            Plural = plural;
            Portion = portion;
            Hidden = hidden;
        }
    }
}
