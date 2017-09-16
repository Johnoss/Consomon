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

        public static void PlayTrack(int id)
        {
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "Properties\\" + tracks[id];
            if (File.Exists(player.SoundLocation))
            {
                player.PlayLooping();
            }
        }
    }
}
