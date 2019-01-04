using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    [Serializable]
    public class Establishment : Location
    {
        private ISupplyable[] stock;

        public ISupplyable[] Stock { get => stock; set => stock = value; }

        public override ISupplyable[] GetDynamicCollection()
        {
            return stock;
        }

        public void FillStock(Town town)
        {
            switch(screen)
            {
                case ScreenType.Shop:
                    stock = Data.Items.ToArray();
                    break;
                case ScreenType.MobDealer:
                    stock = GetSellableMobs(town);
                    break;
            }
        }

        private Mob[] GetSellableMobs(Town town)
        {
            return Data.Mobs.Where(m => m.Type == town.TownType && m.Level <= town.Level).ToArray();
        }

        public override void UpdateDescription(Player player)
        {
            description = $"You have {player.Money} {Output.CurrencyName}";
        }
    }
}
