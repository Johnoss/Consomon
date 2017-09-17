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
            Player p;
            
            //Start the game
            Start();

            void Start()
            {
                InitializeGame();

                while(true) //MAIN GAME LOOP
                {
                    UI.InitializeScreen(p);
                    p.ReadInput();
                }
            }

            void InitializeGame()
            {
                Data.ScaleLevels();

                MusicPlayer.PlayTrack(MusicPlayer.MainTheme);
                UI.LockConsole();


                if(Serialization.CheckSaveFile(Settings.SavePath, Settings.SaveFile))
                {
                    p = Player.LoadPlayer();
                    p.ChangeLocation(p.CurrentLocation);
                }
                else
                {
                    p = new Player();
                    StartTutorial();
                }
            }

            void StartTutorial()
            {
                Tutorial begin = new Tutorial("Welcome to Consomon", $"Let's get you set with your first {Data.MobLabel}.");
                p.ChangeLocation(begin);

                begin.Explore(p);
            }
        }
    }
}