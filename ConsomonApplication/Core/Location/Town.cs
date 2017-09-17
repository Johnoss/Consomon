using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    [Serializable]
    public class Town : Location
    {
        protected List<Establishment> establishments;
        protected MobType townType;
        private Crossroads crossroad;
        private float levelRatio;

        public MobType TownType { get { return townType; } }
        public List<Establishment> Establishments { get { return establishments; } }

        public Crossroads Crossroad { get => crossroad; set => crossroad = value; }
        public float LevelRatio { get => levelRatio; set => levelRatio = value; }

        public Town(string title, float levelRatio, MobType townType, int[] connectedTowns)
        {
            this.title = title;
            this.levelRatio = levelRatio;
            this.townType = townType;

            description = $"You are in {title}. The folks around here worship {townType} gods.";

            screen = ScreenType.town;

            crossroad = new Crossroads(connectedTowns, this);
        }

        public override ISupplyable[] GetDynamicCollection()
        {
            return Data.DefaultEstablishments.ToArray();
        }

        public override string GetLabel(Screen s)
        {
            string difficultyLabel;
            double levelDifference = GenericOperations.GetRatio(Settings.MinLevel, Settings.MaxLevel, level);
            if (levelDifference <= Settings.MediumTravelRatio)
                difficultyLabel = Output.EasyLabel;
            else if (levelDifference <= Settings.HardTravelRatio)
                difficultyLabel = Output.MediumLabel;
            else
                difficultyLabel = Output.HardLabel;

            return $"{title} [{townType}] ({difficultyLabel})";
        }

        public void InitializeEstablishments()
        {
            establishments = new List<Establishment>();
            foreach(Establishment e in Data.DefaultEstablishments)
            {
                establishments.Add(new Establishment() { Screen = e.Screen });
            }
            foreach(Establishment e in establishments)
            {
                e.FillStock(this);
            }
        }

        public override void UpdateDescription(Player p)
        {

        }
    }
}
