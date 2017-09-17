using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace ConsomonApplication
{
    public static class MusicPlayer
    {
        private static string[] tracks = new string[] { "ConsomonTheme.wav", "ConsomonBattleTheme.wav" };
        private static SoundPlayer player = new SoundPlayer();

        public static string MainTheme = "ConsomonTheme.wav";
        public static string BattleTheme = "ConsomonBattleTheme.wav";

        public static void PlayTrack(string source)
        {
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "Properties\\" + source;
            if (File.Exists(player.SoundLocation))
            {
                player.PlayLooping();
            }
        }
    }
}
