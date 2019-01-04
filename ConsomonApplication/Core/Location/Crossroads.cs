using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    [Serializable]
    public class Crossroads : Location
    {
        private int[] connectedTowns;
        private Town town;

        public Town Town { get => town; set => town = value; }
        public int[] ConnectedTowns { get => connectedTowns; set => connectedTowns = value; }

        public Crossroads(int[] connectedTowns, Town town)
        {
            this.ConnectedTowns = connectedTowns;
            this.town = town;
            title = $"{Output.LeaveLabel} {town.Title} [{town.TownType}]";

            description = "Choose your destination:";

            screen = ScreenType.Travel;
        }

        public Wilderness GenerateWilderness(int destination)
        {
            return new Wilderness(town, Data.Towns[destination]);
        }

        public override ISupplyable[] GetDynamicCollection()
        {
            List<ISupplyable> result = new List<ISupplyable>();
            foreach (int townIndex in ConnectedTowns)
            {
                result.Add(Data.Towns[townIndex]);
            }
            return result.ToArray();
        }

        public override void UpdateDescription(Player player)
        {

        }
    }
}
