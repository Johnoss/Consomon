using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsomonApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start the loop
            Start();

            void Start()
            {
                //Initialize game
                Data.ScaleLevels();

                MusicPlayer.PlayTrack(0);
                Player player = new Player();
                player.ChangeLocation(Settings.StartingLocation);

                UI.LockConsole();

                StartTutorial(player);
                while(true) //MAIN GAME LOOP
                {
                    UI.InitializeScreen(player);
                    player.ReadInput();
                }
            }

            void StartTutorial(Player p)
            {
                Tutorial begin = new Tutorial("Welcome to Consomon", $"Let's get you set with your first {Data.MobLabel}.");
                p.ChangeLocation(begin);

                begin.Explore(p);
            }
        }
    }
}