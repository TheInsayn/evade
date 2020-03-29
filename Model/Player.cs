using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Evade
{
    class Player: Entity
    {
        public Player(int size, Color c) : base(size, c, 0, 0) { }
        public Player(int size, Color c, int x, int y) : base(size, c, x, y) { }
    }
}
