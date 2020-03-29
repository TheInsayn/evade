using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Evade
{
    class Player : Entity
    {
        public Player() : this(0, 0) { }
        public Player(int x, int y) : base(Constants.PLAYERSIZE, Constants.PLAYERCOLOR, x, y) { }
    }
}
