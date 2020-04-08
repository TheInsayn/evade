using System.Windows.Media;

namespace Evade
{
    static class Constants
    {
        public const float GAMESPEEDFACTOR = 0.2F; // magicnumber
        public const int BLOCKCREATETICKS = 15; // magicnumber
        public const int COUNTDOWNTIME = 2;

        public const string HIGHSCOREFILE = @"highscore.hs";

        public const int PLAYERSIZE = 30;
        public const int PLAYERMOVESPEED = 8;

        public const int BLOCKMINSIZE = 30;
        public const int BLOCKMAXSIZE = 60;
        public const int BLOCKMINSPEED = 5;
        public const int BLOCKMAXSPEED = 8;

        public static readonly Color PLAYERCOLOR = Colors.Yellow;
        public static readonly Color BLOCKCOLOR = Colors.Red;
    }
}
