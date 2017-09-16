using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{

    public class Player : IListable
    {
        private List<Mob> ownedMobs = new List<Mob>();
        private Dictionary<Item, int> inventory = new Dictionary<Item, int>();
        private Dictionary<ConsoleKey, string> controlScheme = new Dictionary<ConsoleKey, string>();


        private Town lastTown;
        private Location currentLocation;
        private Location previousLocation;

        private Screen currentScreen;

        private Mob champion;
        private Mob target;

        private Controller controller = new Controller();

        private int money = Settings.StartingMoney;

        public List<Mob> OwnedMobs { get => ownedMobs; set => ownedMobs = value; }
        public List<Mob> SelectableMobs { get => ownedMobs.Where(m => m.Stats[StatType.health].Value > m.Stats[StatType.health].MinValue).ToList(); } //list of mobs with health above 0
        public Dictionary<Item, int> Inventory { get => inventory; set => inventory = value; }
        public Dictionary<ConsoleKey, string> ControlScheme { get => controlScheme; set => controlScheme = value; }
        public Location CurrentLocation { get => currentLocation; set => currentLocation = value; }
        public Location PreviousLocation { get => previousLocation; set => previousLocation = value; }
        public Screen CurrentScreen { get => currentScreen; set => currentScreen = value; }
        public Mob Champion { get => champion; set => champion = value; }
        public int Money { get => money; set => money = value; }
        public Town LastTown { get => lastTown; set => lastTown = value; }
        public float AverageMobsLevel => ownedMobs.Average(m => m.Level);
        public Player()
        {
            controller.InputRead += OnInputRead;
        }

        public void ReadInput()
        {
            controller.ReadInput(this);
        }

        public void WipeControls()
        {
            controlScheme = new Dictionary<ConsoleKey, string>();
        }

        public void ChangeScreen(Screen newScreen, IListable supplier)
        {
            currentScreen = newScreen;
            ISupplyable[] collection = supplier.GetDynamicCollection();
            newScreen.SourceColl = collection;

        }

        public void ChangeLocation(Location location)
        {
            Location buffer = currentLocation;
            if (buffer is Town t)
                lastTown = t;
            currentLocation = location;
            previousLocation = buffer;
            Controls.ResetScreen(this);
        }

        public void Retreat()
        {
            ChangeLocation(lastTown);
        }

        public void OnInputRead(object source, KeyReadEventArgs args)
        {
            Controls.TranslateInput(args.InputKey, this);
        }

        public ISupplyable[] GetDynamicCollection()
        {
            if (Data.Screens[ScreenType.selectItem] == currentScreen)
                return inventory.Keys.ToArray();
            if (Data.Screens[ScreenType.selectMob] == currentScreen)
                return SelectableMobs.ToArray();
            return new ISupplyable[0];
        }
    }
}